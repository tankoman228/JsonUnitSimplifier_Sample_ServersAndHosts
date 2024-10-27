using JsonUnitSimplifier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForServersAndHostWithJSONUnitSimplifier
{
    [TestClass]
    public class Unit_Auto
    {
        private const string PATH = "C:\\Users\\Admin\\source\\repos\\ServersAndHosts\\Tests\\TestForServersAndHostWithJSONUnitSimplifier\\JSON\\Auto\\";

        [TestMethod]
        public void Automatic()
        {
            TestByJSON.AutoTestByJSONs(PATH);
        }
    }
}
