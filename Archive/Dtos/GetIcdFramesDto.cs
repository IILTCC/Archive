using MongoConsumerLibary.MongoConnection.Enums;
using System;

namespace Archive.Dtos
{
    public class GetIcdFramesDto
    {
        public int StartPoint { get; set; }
        public int FrameCount { get; set; }
        public IcdType CollectionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
