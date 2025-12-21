using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quartz;

namespace UnimarFrontend.backend.Service
{
    public class QuartzScheduller
    {
        public record JobConfiguration(string jobName, CronExpression ce);
        public static IServiceCollectionQuartzConfigurator GetConfiguration<T>(
           IServiceCollectionQuartzConfigurator q,
           JobConfiguration jc
       ) where T : IJob
        {
            q.AddJob<T>(opts =>
                opts.WithIdentity(jc.jobName)
            );

            q.AddTrigger(opts => opts
                .ForJob(jc.jobName)
                .WithIdentity($"{jc.jobName}-trigger")
                .StartNow() // ⬅ executa imediatamente ao subir
                .WithCronSchedule(
                    jc.ce.ToString(),
                    cron => cron.WithMisfireHandlingInstructionFireAndProceed()
                )
            );

            return q;
        }
    }
}
