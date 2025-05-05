using Archive.Services;
using System.Collections.Generic;

namespace Archive.Dtos
{
    public class StatisticsRo
    {
        public Dictionary<string,List<StatisticsPoint>> Graphs { get; set; }
    }
}
