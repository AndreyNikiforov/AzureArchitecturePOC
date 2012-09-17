using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// This message directs worker to populate sql storage with a some data
    /// </summary>
    public class PopulateSqlMessage : MessageBase
    {
        /// <summary>
        /// Number that this batch should start from
        /// </summary>
        public int StartFrom;

        /// <summary>
        /// This is the size of the field used to fill up database (pump the size, to reduce chances of sql server caching)
        /// </summary>
        public int LoremIpsumBlobSize;
    }
}
