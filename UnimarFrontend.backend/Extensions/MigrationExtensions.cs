using Microsoft.EntityFrameworkCore;
using UnimarFrontend.backend.UnimarFrontend.Infra.Context;

namespace UnimarFrontend.backend.Service.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Aplica migrations pendentes automaticamente
            db.Database.Migrate();
        }
    }
}
