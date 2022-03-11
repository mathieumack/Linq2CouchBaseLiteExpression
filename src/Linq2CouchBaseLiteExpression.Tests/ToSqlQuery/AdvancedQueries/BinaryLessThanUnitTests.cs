using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.SimpleQueries
{
    [TestClass]
    public class BinaryLessThanUnitTests : BaseUnitTestToSqlQuery
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
        public void Binary_LessThan_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Age < 10, 4);
        }

        [TestMethod]
        public void Binary_LessThan_ValueWithVariable_Void()
        {
            int minAge = 10;
            CheckCount<EntityObject>((e) => e.Age < minAge, 4);
        }

        [TestMethod]
        public void Binary_LessThan_ValueWithStaticFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age < GetStaticAge(), 4);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void Binary_LessThan_ValueWithFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age < GetAge(), 4);
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
