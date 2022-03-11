using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.SimpleQueries
{
    [TestClass]
    public class BinaryNotEqualUnitTest : BaseUnitTestToSqlQuery
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
        public void Binary_NotEqual_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Age != 8, 3);
        }

        [TestMethod]
        public void Binary_NotEqual_ValueWithVariable_Void()
        {
            int minAge = 8;
            CheckCount<EntityObject>((e) => e.Age != minAge, 3);
        }

        [TestMethod]
        public void Binary_NotEqual_ValueWithStaticFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age != GetStaticAge(), 3);
        }

        public static int GetStaticAge()
        {
            return 8;
        }
    }
}
