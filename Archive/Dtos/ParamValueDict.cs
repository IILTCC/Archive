using System;

namespace Archive.Dtos
{
    public class ParamValueDict
    {
        public ParamValueDict(int value,bool isFaulty, DateTime packetTime)
        {
            this.Value = value;
            this.IsFaulty = isFaulty;
            this.PacketTime = packetTime;
        }
        public int Value { get; set; }
        public bool IsFaulty { get; set; }
        public DateTime PacketTime { get; set; }
    }
}
