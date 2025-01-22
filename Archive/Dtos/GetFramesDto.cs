using System;

namespace Archive.Dtos.Incoming
{
    public class GetFramesDto
    {
        public int StartPoint { get; set; } 
        public int FrameCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
