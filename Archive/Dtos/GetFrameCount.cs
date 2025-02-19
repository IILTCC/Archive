using MongoConsumerLibary.MongoConnection.Enums;
using System;

namespace Archive.Dtos
{
    public class GetFrameCount
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IcdType CollectionType { get; set; }
    }
}
