using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression.AdvancedQueries
{
    [TestClass]
    public class ConditionalIfUnitTests : BaseUnitTestToExpression
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
        public void ConditionalIf_Exists()
        {
            var test = 10;
            CheckCount<EntityObject>((e) => (test > 0 ? e.Age > 0 : true), 5);
        }


        [TestMethod]
        public void ConditionalIf_NoResults()
        {
            var test = -1;
            CheckCount<EntityObject>((e) => (test > 0 ? e.Age > 0 : false), 0);
        }
    }
}
