using System.Collections.Generic;

namespace Routine.API.Models
{
    public class Schedule
    {
        public List<Job> Job { get; set; }
    }

    public class Job
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string JobType { get; set; }
        public string Durable { get; set; }
        public string Recover { get; set; }
        public string CronExpression { get; set; }

    }

    public class TokenOption
    {
        public string Role { get; set; }
    }
}