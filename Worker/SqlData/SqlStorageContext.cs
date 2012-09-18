using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Worker.SqlData
{
    public class SqlStorageContext : DbContext
    {
        public DbSet<DataLoad> DataLoads { get; set; }

        public SqlStorageContext(string connextionString) : base(connextionString) {}

        //required for migrations
        public SqlStorageContext() :base()
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
