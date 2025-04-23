using System;

namespace Archive.Dtos
{
    public class FrameDatesRo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public FrameDatesRo(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
