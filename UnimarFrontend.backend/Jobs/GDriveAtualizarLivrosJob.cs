using Quartz;
using UnimarFrontend.backend.Service;
namespace UnimarFrontend.backend.Jobs
{
    public class GDriveAtualizarLivrosJob : IJob
    {
        private readonly ILogger<GDriveAtualizarLivrosJob> _logger;
        private readonly BookService _service;

        public GDriveAtualizarLivrosJob(
            ILogger<GDriveAtualizarLivrosJob> logger,
            BookService service
        )
        {
            _logger = logger;
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executando GDriveAtualizarLivrosJob às {time}", DateTime.Now);

            // exemplo de uso
            // await _service.AtualizarLivrosAsync();
        }
    }
}