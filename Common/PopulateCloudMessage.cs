﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class PopulateCloudMessage : MessageBase
    {
        public int Partition { get; set; }
        public int Batch { get; set; }
    }
}
