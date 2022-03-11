using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.AdvancedQueries
{
    [TestClass]
    public class NullUnitTests : BaseUnitTestToSqlQuery
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
        public void EqualsNull_Exists()
        {
            CheckCount<EntityObject>((e) => e.SurName == null, 1);
        }

        [TestMethod]
        public void NullEquals_Exists()
        {
            CheckCount<EntityObject>((e) => null == e.SurName, 1);
        }

        [TestMethod]
        public void IsNotNull_Exists()
        {
            CheckCount<EntityObject>((e) => e.SurName != null, 4);
        }

        [TestMethod]
        public void NotNullIs_Exists()
        {
            CheckCount<EntityObject>((e) => null != e.SurName, 4);
        }

        [TestMethod]
        public void NullableIsNull_Exists()
        {
            CheckCount<EntityObject>((e) => e.BirthDay == null, 2);
        }
    }
}
