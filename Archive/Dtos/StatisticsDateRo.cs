using System;

namespace Archive.Dtos
{
    public class StatisticsDateRo
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public StatisticsDateRo(DateTime startDate , DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
