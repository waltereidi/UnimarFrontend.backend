using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;

namespace UnimarFrontend.Infra.Context
{
    public class AppDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=livrosexpo;Username=livrosexpo;Password=livrosexpo"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
