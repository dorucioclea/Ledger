﻿using Ledger.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Shared.Locations.Context
{
    public class LedgerLocationDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }

        public LedgerLocationDbContext(DbContextOptions<LedgerLocationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
