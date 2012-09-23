using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Worker.SqlData
{
    public class SqlStorageContext : DbContext
    {
        public DbSet<DataLoad> LoremIpsum { get; set; }

        public SqlStorageContext(string connectionString) : base(connectionString) {}

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
