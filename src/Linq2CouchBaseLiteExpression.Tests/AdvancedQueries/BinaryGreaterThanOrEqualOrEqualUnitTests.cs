using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.SimpleQueries
{
    [TestClass]
    public class BinaryGreaterThanOrEqualOrEqualUnitTests : BaseUnitTest
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
        public void Binary_GreaterThanOrEqual_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Age >= 9, 2);
        }

        [TestMethod]
        public void Binary_GreaterThanOrEqual_ValueWithVariable_Void()
        {
            int minAge = 9;
            CheckCount<EntityObject>((e) => e.Age >= minAge, 2);
        }

        [TestMethod]
        public void Binary_GreaterThanOrEqual_ValueWithStaticFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age >= GetStaticAge(), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Reflection.TargetException))]
        public void Binary_GreaterThanOrEqual_ValueWithFunction_Void()
        {
            CheckCount<EntityObject>((e) => e.Age >= GetAge(), 2);
        }

        public int GetAge()
        {
            return 9;
        }

        public static int GetStaticAge()
        {
            return 9;
        }
    }
}
