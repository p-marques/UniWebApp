using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using UniWebApp.Core;

namespace UniWebApp.Data
{
    public class AppDbContext : IdentityDbContext
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppEntityDataFieldCombobox>().HasMany(c => c.Options).WithOne(z => z.Combobox);

            base.OnModelCreating(builder);
        }
    }
}
