using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression.SimpleQueries
{
    [TestClass]
    public class BinaryLessThanOrEqualUnitTests : BaseUnitTestToExpression
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
        public void Binary_LessThanOrEqual_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Age <= 8, 3);
        }

        [TestMethod]
        public void Binary_LessThanOrEqual_ValueWithVariable_Void()
        {
            int minAge = 8;
            CheckCount<EntityObject>((e) => e.Age <= minAge, 3);
        }

        [TestMethod]
        public void Binary_LessThanOrEqual_ValueWithStaticFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age <= GetStaticAge(), 3);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void Binary_LessThanOrEqual_ValueWithFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age <= GetAge(), 3);
        }

        public int GetAge()
        {
            return 8;
        }

        public static int GetStaticAge()
        {
            return 8;
        }
    }
}
