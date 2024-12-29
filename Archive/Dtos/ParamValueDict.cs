namespace Archive.Dtos
{
    public class ParamValueDict
    {
        public ParamValueDict(int value,bool isFaulty)
        {
            this.Value = value;
            this.IsFaulty = isFaulty;
        }
        public int Value { get; set; }
        public bool IsFaulty { get; set; }
    }
}
