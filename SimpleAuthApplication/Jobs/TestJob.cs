using FluentScheduler;

namespace SimpleAuthApplication.Jobs
{
    public class TestJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine($"TestJob executed at: {DateTime.Now}");
        }
    }
}
