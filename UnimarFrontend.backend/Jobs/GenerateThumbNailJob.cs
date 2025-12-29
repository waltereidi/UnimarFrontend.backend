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
            try
            {
                Console.WriteLine("===========================QUARTZ===============================");
                Console.WriteLine("Execute Thumbnail");
                var result = _service.GetBookWithouthThumbNail();
                if (result == null)
                    return;
                Console.WriteLine("Thumbnail 29");
                _service.GenerateThumbNail(result);
                
                //var lastBook = _service.GetBookWithouthThumbNail();
                //await _service.AddBookRange(lastBook);
                //Console.WriteLine("Execute end");
                //// exemplo de uso
                //// await _service.AtualizarLivrosAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                _logger.LogError(ex.Message);
            }
            
            
        }
    }
}
