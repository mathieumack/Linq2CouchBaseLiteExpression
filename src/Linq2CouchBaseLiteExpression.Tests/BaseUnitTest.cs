using System;
using Couchbase.Lite;

namespace Linq2CouchBaseLiteExpression.Tests
{
    public abstract class BaseUnitTest
    {
        protected Database db;

        /// <summary>
        /// Create a local database and insert some elements.
        /// The database is the same for all unit tests as we just tru queries :
        /// </summary>
        public virtual void TestInitialize()
        {
            db = new Database(Guid.NewGuid().ToString());

            // Empty database, so we will create 6 sample documents :
            CreateDocument("name1", "firstName1", 8, true, null, DateTimeOffset.UtcNow);
            CreateDocument("name2", null, 8, true, null, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name3", "firstName3", 12, true, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
            CreateDocument("name4", "", 9, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name5", "sur \"Name\"", 7, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        }

        public virtual void CloseConnection()
        {
            db.Delete();
            db.Dispose();
        }

        /// <summary>
        /// Create a new document in the local storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surName"></param>
        /// <param name="age"></param>
        /// <param name="isHuman"></param>
        /// <param name="birthday"></param>
        /// <param name="createdAt"></param>
        private void CreateDocument(string name, string surName, int age, bool isHuman, DateTimeOffset? birthday, DateTimeOffset createdAt)
        {
            using (var newDocument = new MutableDocument())
            {
                newDocument.SetString("Name", name)
                            .SetString("SurName", surName)
                            .SetInt("Age", age)
                            .SetInt("SubObject.Value", age)
                            .SetInt("SubObject.SubSubObject.Value", age)
                            .SetBoolean("IsHuman", isHuman)
                            .SetDate("CreatedAt", createdAt);

                if (birthday.HasValue)
                    newDocument.SetDate("BirthDay", birthday.Value);
                else
                    newDocument.SetValue("BirthDay", null);

                db.Save(newDocument);
            }
        }
    }
}
