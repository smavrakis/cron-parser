namespace CronParser.Models
{
    public class CronFieldRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Step { get; set; } = 1;
    }
}
