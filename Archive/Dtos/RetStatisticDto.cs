using DownSamplingLibary;
using System.Collections.Generic;

namespace Archive.Dtos
{
    public class RetStatisticDto
    {
        public Dictionary<string,List<GraphPoint>> Graphs { get; set; }
        public Dictionary<string, int> SevirityValues { get; set; }
        public Dictionary<string,float> Values { get; set; }
    }
}
