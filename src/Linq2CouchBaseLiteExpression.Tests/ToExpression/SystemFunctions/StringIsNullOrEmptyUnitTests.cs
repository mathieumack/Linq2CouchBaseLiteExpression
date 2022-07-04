using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression.SystemFunctions
{
    [TestClass]
    public class StringIsNullOrEmptyUnitTests : BaseUnitTestToExpression
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
        public void IsNullOrEmpty_Exists()
        {
            CheckCount<EntityObject>((e) => string.IsNullOrEmpty(e.SurName), 2);
        }

        [TestMethod]
        public void IsNullOrEmpty_Void()
        {
            CheckCount<EntityObject>((e) => string.IsNullOrEmpty(e.Name), 0);
        }

        [TestMethod]
        public void Not_IsNullOrEmpty_Void()
        {
            CheckCount<EntityObject>((e) => !string.IsNullOrEmpty(e.Name), 5);
        }
    }
}
