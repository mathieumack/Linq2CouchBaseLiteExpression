using System.Collections.Generic;
using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.LinqFunctions
{
    [TestClass]
    public class LinqContainsUnitTests : BaseUnitTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void CloseConnection()
        {
            base.CloseConnection();
        }

        [TestMethod]
        public void LinqContains_String_Matches()
        {
            var names = new List<string>()
            {
                "name2",
                "name4"
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Name), 2);
        }

        [TestMethod]
        public void LinqContains_String_MatchesOnNameAndAge()
        {
            var names = new List<string>()
            {
                "name2",
                "name4"
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Name) && e.Age > 8, 1);
        }

        [TestMethod]
        public void LinqContains_String_EmptyList()
        {
            var names = new List<string>();
            CheckCount<EntityObject>((e) => names.Contains(e.Name), 0);
        }

        [TestMethod]
        public void LinqContains_String_NoMathch()
        {
            var names = new List<string>()
            {
                "name22",
                "name42"
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Name), 0);
        }

        [TestMethod]
        public void LinqContains_String_MatchesOnAgeNotName()
        {
            var names = new List<string>()
            {
                "name22",
                "name42"
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Name) || e.Age > 8, 2);
        }

        [TestMethod]
        public void LinqContains_Int_Matches()
        {
            var names = new List<int>()
            {
                7,8
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Age), 3);
        }

        [TestMethod]
        public void LinqContains_Int_EmptyList()
        {
            var names = new List<int>();
            CheckCount<EntityObject>((e) => names.Contains(e.Age), 0);
        }

        [TestMethod]
        public void LinqContains_Int_NoMathch()
        {
            var names = new List<int>()
            {
                22,42
            };
            CheckCount<EntityObject>((e) => names.Contains(e.Age), 0);
        }
    }
}
