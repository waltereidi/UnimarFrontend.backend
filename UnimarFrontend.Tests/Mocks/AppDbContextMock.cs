using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;

namespace UnimarFrontend.Tests.Mocks
{
    public static class AppDbContextMock
    {
        public static AppDbContext Create(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .Options;

            return new AppDbContext(options);
        }
    }
}
