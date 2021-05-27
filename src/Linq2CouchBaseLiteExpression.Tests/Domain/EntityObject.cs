using System;

namespace Linq2CouchBaseLiteExpression.Tests.Domain
{
    public class EntityObject
    {
        /// <summary>
        /// Unique indentifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Creation date of the object in the Viewer repository
        /// </summary>
        public DateTimeOffset SystemCreationDate { get; set; }

        /// <summary>
        /// Last update date of the object in the Viewer repository
        /// </summary>
        public DateTimeOffset SystemLastUpdateDate { get; set; }

        /// <summary>
        /// Logical delete status
        /// </summary>
        public bool Deleted { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Enumeration value
        /// </summary>
        public EntityType TypeOfObject { get; set; }

        /// <summary>
        /// Birthday date
        /// </summary>
        /// <remarks>Can be null</remarks>
        public DateTimeOffset? BirthDay { get; set; }

        public int Age { get; set; }

        public string SurName { get; set; }

        public bool IsHuman { get; set; }
    }
}
