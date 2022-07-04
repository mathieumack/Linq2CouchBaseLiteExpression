using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.SimpleQueries
{
    [TestClass]
    public class BinaryGreaterThanUnitTests : BaseUnitTestToSqlQuery
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
        public void Binary_GreaterThan_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Age > 10, 1);
        }

        [TestMethod]
        public void Binary_GreaterThan_ValueWithVariable_Void()
        {
            int minAge = 10;
            CheckCount<EntityObject>((e) => e.Age > minAge, 1);
        }

        [TestMethod]
        public void Binary_GreaterThan_ValueWithStaticFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age > GetStaticAge(), 1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void Binary_GreaterThan_ValueWithFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age > GetAge(), 1);
        }

        public int GetAge()
        {
            return 10;
        }

        public static int GetStaticAge()
        {
            return 10;
        }
    }
}
