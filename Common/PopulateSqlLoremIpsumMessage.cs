using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PopulateSqlLoremIpsumMessage : MessageBase
    {
        /// <summary>
        /// This is the size of the field used to fill up database (pump the size, to reduce chances of sql server caching)
        /// </summary>
        public int LoremIpsumBlobSize;

    }
}
