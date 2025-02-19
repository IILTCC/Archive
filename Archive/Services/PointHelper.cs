using DownSamplingLibary;

namespace Archive.Services
{
    public class PointHelper : GraphPoint
    {
        public PointHelper(double x, double y, bool isFaulty) : base(x, y)
        {
            IsFaulty = isFaulty;
        }
        public bool IsFaulty { get; set; }
    }
}
