using System.Linq;
using Couchbase.Lite.Query;
using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryNotUnitTests : BaseUnitTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public void Binary_Not_Expression()
        {
            CheckCount<EntityObject>((e) => !(e.IsHuman), 2);
        }
    }
}
