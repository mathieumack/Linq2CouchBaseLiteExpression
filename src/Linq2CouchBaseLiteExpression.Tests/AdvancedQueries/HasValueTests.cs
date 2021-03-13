using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.AdvancedQueries
{
    [TestClass]
    public class HasValueTests : BaseUnitTest
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

        #region Basic tests

        [TestMethod]
        public void HasValue_True_Exists()
        {
            CheckCount<EntityObject>((e) => e.BirthDay.HasValue, 3);
        }

        [TestMethod]
        public void HasValue_False_Void()
        {
            CheckCount<EntityObject>((e) => !e.BirthDay.HasValue, 2);
        }

        #endregion

        #region Complex queries

        [TestMethod]
        public void HasValue_Mixed_True_Exists()
        {
            CheckCount<EntityObject>((e) => e.BirthDay.HasValue && e.Age > 8, 2);
        }

        [TestMethod]
        public void HasValue_Invert_False_True_Void()
        {
            CheckCount<EntityObject>((e) => !(!e.BirthDay.HasValue), 3);
        }

        #endregion
    }
}
