using DownSamplingLibary;

namespace Archive.Services
{
    public class StatisticsPoint : GraphPoint
    {
        public int Sevirity { get; set; }
        public StatisticsPoint(double x, double y,int sevirity):base( x, y)
        {
            Sevirity = sevirity;
        }
        public string ToString()
        {
            return "x- " + X.ToString() + ", y- " + Y.ToString();
        }
    }
}
