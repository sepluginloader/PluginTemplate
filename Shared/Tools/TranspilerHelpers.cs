using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using HarmonyLib;

namespace Shared.Tools
{
    // Useful methods in transpiler patches.
    // For usage examples please search this repo:
    // https://github.com/viktor-ferenczi/performance-improvements
    public static class TranspilerHelpers
    {
        private static readonly int DotNetMajorVersion = Environment.Version.Major; 
        
        public delegate bool OpcodePredicate(OpCode opcode);

        public delegate bool CodeInstructionPredicate(CodeInstruction ci);

        public delegate bool FieldInfoPredicate(FieldInfo fi);

        public delegate bool PropertyInfoPredicatePredicate(PropertyInfo pi);

        public static List<int> FindAllIndex(this IEnumerable<CodeInstruction> il, CodeInstructionPredicate predicate)
        {
            return il.Select((instruction, index) => new { Instruction = instruction, Index = index })
                .Where(pair => predicate(pair.Instruction))
                .Select(pair => pair.Index)
                .ToList();
        }

        public static FieldInfo GetField(this List<CodeInstruction> il, FieldInfoPredicate predicate)
        {
            var ci = il.Find(i => (i.opcode == OpCodes.Ldfld || i.opcode == OpCodes.Stfld) && i.operand is FieldInfo fi && predicate(fi));
            if (ci == null)
                throw new CodeInstructionNotFound("No code instruction found loading or storing a field matching the given predicate");

            return (FieldInfo) ci.operand;
        }

        public static MethodInfo FindPropertyGetter(this List<CodeInstruction> il, string name)
        {
            var ci = il.Find(i => i.opcode == OpCodes.Call && i.operand is MethodInfo fi && fi.Name == $"get_{name}");
            if (ci == null)
                throw new CodeInstructionNotFound("No code instruction found getting or setting a property matching the given predicate");

            return (MethodInfo) ci.operand;
        }

        public static MethodInfo FindPropertySetter(this List<CodeInstruction> il, string name)
        {
            var ci = il.Find(i => i.opcode == OpCodes.Call && i.operand is MethodInfo fi && fi.Name == $"set_{name}");
            if (ci == null)
                throw new CodeInstructionNotFound("No code instruction found getting or setting a property matching the given predicate");

            return (MethodInfo) ci.operand;
        }

        public static Label GetLabel(this List<CodeInstruction> il, OpcodePredicate predicate)
        {
            var ci = il.Find(i => i.operand is Label && predicate(i.opcode));
            if (ci == null)
                throw new CodeInstructionNotFound("No label found matching the opcode predicate");

            return (Label) ci.operand;
        }

        public static void RemoveFieldInitialization(this List<CodeInstruction> il, string name)
        {
            var i = il.FindIndex(ci => ci.opcode == OpCodes.Stfld && ci.operand is FieldInfo fi && fi.Name.Contains(name));
            if (i < 2)
                throw new CodeInstructionNotFound($"No code instruction found initializing field: {name}");

            Debug.Assert(il[i - 2].opcode == OpCodes.Ldarg_0);
            Debug.Assert(il[i - 1].opcode == OpCodes.Newobj);

            il.RemoveRange(i - 2, 3);
        }

        private static string FormatCode(this List<CodeInstruction> il)
        {
            var sb = new StringBuilder();

            var hash = il.HashInstructions().CombineHashCodes().ToString("x8");
            sb.Append($"// {hash}\r\n");

            foreach (var ci in il)
            {
                sb.Append(ci.ToCodeLine());
                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        private static string ToCodeLine(this CodeInstruction ci)
        {
            var sb = new StringBuilder();

            foreach (var label in ci.labels)
                sb.Append($"L{label.GetHashCode()}:\r\n");

            if (ci.blocks.Count > 0)
            {
                var formattedBlocks = string.Join(", ", ci.blocks.Select(b => $"EX_{b.blockType}"));
                sb.Append("[");
                sb.Append(formattedBlocks.Replace("Block", ""));
                sb.Append("]\r\n");
            }

            sb.Append(ci.opcode);

            var arg = FormatArgument(ci.operand);
            if (arg.Length > 0)
            {
                sb.Append(' ');
                sb.Append(arg);
            }

            return sb.ToString();
        }

        private static string FormatArgument(object argument, string extra = null)
        {
            switch (argument)
            {
                case null:
                    return "";

                case MethodBase member when extra == null:
                    return $"{member.FullDescription()}";

                case MethodBase member:
                    return $"{member.FullDescription()} {extra}";
            }

            var fieldInfo = argument as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.FieldType.FullDescription() + " " + fieldInfo.DeclaringType.FullDescription() + "::" + fieldInfo.Name;

            switch (argument)
            {
                case Label label:
                    return $"L{label.GetHashCode()}";

                case Label[] labels:
                    return string.Join(", ", labels.Select(l => $"L{l.GetHashCode()}").ToArray());

                case LocalBuilder localBuilder:
                    return $"{localBuilder.LocalIndex} ({localBuilder.LocalType})";

                case string s:
                    return s.ToLiteral();

                default:
                    return argument.ToString().Trim();
            }
        }

        public static void RecordOriginalCode(this List<CodeInstruction> il, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
#if DEBUG
            RecordCode(il, callerFilePath, callerMemberName, "original");
#endif
        }

        public static void RecordPatchedCode(this List<CodeInstruction> il, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
#if DEBUG
            RecordCode(il, callerFilePath, callerMemberName, "patched");
#endif
        }

        private static void RecordCode(List<CodeInstruction> il, string callerFilePath, string callerMemberName, string suffix)
        {
            if (!File.Exists(callerFilePath))
                return;

            var text = il.FormatCode();

            Debug.Assert(callerFilePath.Length > 0);
            var dir = Path.GetDirectoryName(callerFilePath);
            Debug.Assert(dir != null);

            var callerFileName = Path.GetFileName(callerFilePath);
            if (callerFileName.ToLower().EndsWith(".cs"))
            {
                callerFileName = callerFileName.Substring(0, callerFileName.Length - 3);
            }
            
            var path = Path.Combine(dir, $"{callerFileName}.{callerMemberName}.Net{DotNetMajorVersion}.{suffix}.il");

            File.WriteAllText(path, text);
        }

        public static CodeInstruction DeepClone(this CodeInstruction ci)
        {
            var clone = ci.Clone();
            clone.labels = ci.labels.ToList();
            clone.blocks = ci.blocks.Select(b => new ExceptionBlock(b.blockType, b.catchType)).ToList();
            return clone;
        }

        public static List<CodeInstruction> DeepClone(this IEnumerable<CodeInstruction> il)
        {
            return il.Select(ci => ci.DeepClone()).ToList();
        }
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CodeInstructionNotFound : Exception
    {
        public CodeInstructionNotFound()
        {
        }

        public CodeInstructionNotFound(string message)
            : base(message)
        {
        }

        public CodeInstructionNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}