using System;
using NUnit.Framework;
using HSharp.Util;

namespace HSharp.UtilTest
{
    public class ComputerHelperTest
    {
        [Test]
        public void TestGetComputerInfo()
        {
            ComputerInfo computerInfo = ComputerHelper.GetComputerInfo();

            Assert.Warn(computerInfo.CPURate);
            Assert.Warn(computerInfo.RAMRate);
            Assert.Warn(computerInfo.TotalRAM);
            Assert.Warn(computerInfo.RunTime);
        }
    }
}
