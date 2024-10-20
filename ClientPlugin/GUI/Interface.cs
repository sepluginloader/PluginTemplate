using ClientPlugin.GUI.Builders;
using ClientPlugin.GUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlugin.GUI
{
    internal class Interface
    {
        public static string FriendlyName = "TestName";

        public static List<Base> Elements = new List<Base>()
        {
            new Checkbox("checkbox1", "Alpha Checkbox", "This is a test"),
            new Checkbox("checkbox2", "Beta Checkbox", "This is a test"),
            new Checkbox("checkbox3", "Gamma Checkbox", "This is a test"),
        };


    }
}
