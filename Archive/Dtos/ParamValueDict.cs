using System;

namespace Archive.Dtos
{
    public class ParamValueDict
    {
        public ParamValueDict(int value,bool isFaulty, DateTime insertTime)
        {
            this.Value = value;
            this.IsFaulty = isFaulty;
            this.InsertTime = insertTime;
        }
        public int Value { get; set; }
        public bool IsFaulty { get; set; }
        public DateTime InsertTime { get; set; }
    }
}
