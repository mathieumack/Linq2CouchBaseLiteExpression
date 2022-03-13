using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.SystemFunctions
{
    [TestClass]
    public class EqualsUnitTests : BaseUnitTestToSqlQuery
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
        public void Equals_Boolen_WithConstants_Exists()
        {
            CheckCount<EntityObject>((e) => e.IsHuman.Equals(true), 3);
        }

        [TestMethod]
        public void Equals_String_WithConstants_Exists()
        {
            CheckCount<EntityObject>((e) => e.Name.Equals("name2"), 1);
        }

        [TestMethod]
        public void Equals_String_WithVariable_Exists()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => e.Name.Equals(nameValue), 1);
        }
    }
}
