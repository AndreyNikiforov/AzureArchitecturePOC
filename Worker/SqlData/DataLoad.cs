using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Worker.SqlData
{
    /// <summary>
    /// This is EF code first for DataLoad table
    /// </summary>
    public class DataLoad
    {
        [Key]
        public int Id { get; set; }

        public string LoremIpsum { get; set; }
    }
}
