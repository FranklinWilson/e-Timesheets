namespace eTimesheet.Models
{
    public class JobList
    {
        public List<Job> Jobs { get; set; }
        public Job NewJob { get; set; } = new Job();
    }
}
