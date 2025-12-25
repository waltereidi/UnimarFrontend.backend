using Quartz;
using UnimarFrontend.backend.Service;

namespace UnimarFrontend.backend.Jobs
{
    public class GenerateThumbNailJob : IJob
    {

        private readonly ILogger<GenerateThumbNailJob> _logger;
        private readonly BookService _service;

        public GenerateThumbNailJob(
            ILogger<GenerateThumbNailJob> logger,
            BookService service
        )
        {
            _logger = logger;
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Console.WriteLine("===========================QUARTZ===============================");
            //Console.WriteLine("Execute");
            var result = _service.GetBookWithouthThumbNail();
            _service.GenerateThumbNail(result);

            //var lastBook = _service.GetBookWithouthThumbNail();
            //await _service.AddBookRange(lastBook);
            //Console.WriteLine("Execute end");
            //// exemplo de uso
            //// await _service.AtualizarLivrosAsync();
            
        }
    }
}
