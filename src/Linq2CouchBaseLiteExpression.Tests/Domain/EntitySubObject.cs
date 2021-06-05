using System;
using System.Collections.Generic;
using System.Text;

namespace Linq2CouchBaseLiteExpression.Tests.Domain
{
    public class EntitySubObject
    {
        /// <summary>
        /// Sub value used for sorting
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Sub object
        /// </summary>
        public EntitySubObject SubSubObject { get; set; }
    }
}
