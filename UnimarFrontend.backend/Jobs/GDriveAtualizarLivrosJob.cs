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
            Console.WriteLine("===========================QUARTZ===============================");
            Console.WriteLine("Execute");
            var lastBook = _service.GetLastBook();
            await _service.AddBookRange(lastBook);
            Console.WriteLine("Execute end");
            // exemplo de uso
            // await _service.AtualizarLivrosAsync();
        }
    }
}