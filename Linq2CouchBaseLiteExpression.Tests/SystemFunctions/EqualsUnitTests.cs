using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.SystemFunctions
{
    [TestClass]
    public class EqualsUnitTests : BaseUnitTest
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
        public void Equals_WithConstants_Exists()
        {
            CheckCount<EntityObject>((e) => e.Name.Equals("name2"), 1);
        }

        [TestMethod]
        public void Equals_WithVariable_Exists()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => e.Name.Equals(nameValue), 1);
        }
    }
}
