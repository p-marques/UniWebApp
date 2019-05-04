using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UniWebApp.Core;

namespace UniWebApp.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppEntity> AppEntities { get; set; }
        public DbSet<AppEntityType> AppEntityTypes { get; set; }
        public DbSet<AppEntityDataField> AppEntityFiels { get; set; }
        public DbSet<AppEntityDataFieldComboboxOption> AppEntityDataFieldComboboxOptions { get; set; }
    }
}
