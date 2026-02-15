using ENOYAEntegrasyonV2.Business;
using ENOYAEntegrasyonV2.Models.Entities;
using System;
using System.Data.Entity;

namespace ENOYAEntegrasyonV2.DbContxt
{
    public class TesisContext : DbContext, IDisposable
    {
        
        public TesisContext() : base(AppGlobals.appSettings.Database.GetConnectionString())
        {
            //Database.SetInitializer<eofAppsEntities>(new CreateDatabaseIfNotExists<eofAppsEntities>());
            //Database.SetInitializer<TesisContext>("");
        }

        public virtual DbSet<MALZEME> MALZEMEs { get; set; }
        public virtual DbSet<SEVKIYAT> SEVKIYATs { get; set; }
        public virtual DbSet<IFSPLAN> IFSPLANs { get; set; }
        public virtual DbSet<PERDETAY> PERDETAYs { get; set; }
        public virtual DbSet<SILO_AD> SILO_ADs { get; set; }
        public virtual DbSet<CONFIG> CONFIGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //.Entity<Department>().ToTable("t_Department");
            modelBuilder.Entity<MALZEME>().ToTable(tableName: new MALZEME().GetType().Name, schemaName: "dbo");
            modelBuilder.Entity<SEVKIYAT>().ToTable(tableName: new SEVKIYAT().GetType().Name, schemaName: "dbo");
            modelBuilder.Entity<IFSPLAN>().ToTable(tableName: new IFSPLAN().GetType().Name, schemaName: "dbo");
            modelBuilder.Entity<PERDETAY>().ToTable(tableName: new PERDETAY().GetType().Name, schemaName: "dbo");
            modelBuilder.Entity<SILO_AD>().ToTable(tableName: new SILO_AD().GetType().Name, schemaName: "dbo");
            modelBuilder.Entity<CONFIG>().ToTable(tableName: new CONFIG().GetType().Name, schemaName: "dbo");
        }

        async void IDisposable()
        {
            this.Dispose();
        }

    }
}
