using MongoConsumerLibary.MongoConnection.Enums;
using System;

namespace Archive.Dtos
{
    public class GetFullIcdFramesDto
    {
        public IcdType CollectionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
