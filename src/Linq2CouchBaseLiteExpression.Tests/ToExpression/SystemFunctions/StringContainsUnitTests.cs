using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression.SystemFunctions
{
    [TestClass]
    public class StringContainsUnitTests : BaseUnitTestToExpression
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
        public void Contains_String_WithNull_Exists()
        {
            CheckCount<EntityObject>((e) => e.Name.Contains(null), 0);
        }

        [TestMethod]
        public void Contains_String_WithConstants_Exists()
        {
            CheckCount<EntityObject>((e) => e.Name.Contains("ame2"), 1);
        }

        [TestMethod]
        public void Contains_String_WithVariable_Exists()
        {
            var nameValue = "ame2";
            CheckCount<EntityObject>((e) => e.Name.Contains(nameValue), 1);
        }
    }
}
