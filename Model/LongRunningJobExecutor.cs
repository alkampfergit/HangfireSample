using System;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    public static class LongRunningJobExecutor
    {
        public static async Task Execute(string jobId)
        {
            Console.WriteLine($"Loading job {jobId}");
            var job = LongRunningJobStorage.LoadJob(jobId);
            Console.WriteLine($"Executing job {jobId} - {job.Type}");
            await Task.Delay(1000 * job.Duration);

            Console.WriteLine($"Executed job {jobId} - {job.Type}");
        }
    }
}
