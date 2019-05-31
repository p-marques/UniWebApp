using Microsoft.EntityFrameworkCore;
using UniWebApp.Core;

namespace UniWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppEntity> AppEntities { get; set; }
        public DbSet<AppEntityType> AppEntityTypes { get; set; }
        public DbSet<AppEntityDataField> AppEntityFields { get; set; }
        public DbSet<AppEntityDataFieldBoolean> AppEntityDataFieldsBoolean { get; set; }
        public DbSet<AppEntityDataFieldDate> AppEntityDataFieldsDate { get; set; }
        public DbSet<AppEntityDataFieldNumber> AppEntityDataFieldsNumber { get; set; }
        public DbSet<AppEntityDataFieldText> AppEntityDataFieldsText { get; set; }
        public DbSet<AppEntityDataFieldCombobox> AppEntityDataFieldsCombobox { get; set; }
        public DbSet<AppEntityDataFieldComboboxOption> AppEntityDataFieldComboboxOptions { get; set; }
        public DbSet<AppEntityRelation> AppEntityRelations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppEntityDataFieldCombobox>().HasMany(c => c.Options).WithOne(z => z.Combobox);

            builder.Entity<AppEntity>().HasMany(x => x.Relations).WithOne(z => z.Entity).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppEntityType>().HasData(
                new AppEntityType() { Id = 1, Name = "Pessoa"},
                new AppEntityType() { Id = 2, Name = "Pessoa Coletiva" },
                new AppEntityType() { Id = 3, Name = "Empresa" });

            base.OnModelCreating(builder);
        }
    }
}