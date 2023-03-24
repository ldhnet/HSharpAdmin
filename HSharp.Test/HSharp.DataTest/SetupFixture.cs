using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using HSharp.Util;
using HSharp.Util.Model;

namespace HSharp.DataTest
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            GlobalContext.SystemConfig = new SystemConfig
            {
                DBProvider = "MySql",
                DBConnectionString = "server=localhost;database=HSharpAdmin;user=root;password=123456;port=3306;",
                DBCommandTimeout = 180,
                DBBackup = "DataBase"
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }
    }
}
