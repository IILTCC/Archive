﻿using System;

namespace Archive.Dtos
{
    public class GetStatisticDto
    {
        public int StartPoint { get; set; }
        public int StatisticsCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
