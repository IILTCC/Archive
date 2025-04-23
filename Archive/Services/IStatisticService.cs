using Archive.Dtos;
using System.Threading.Tasks;

namespace Archive.Services
{
    public interface IStatisticService
    {
        public Task<StatisticsRo> GetStatistics(GetStatisticDto getStatisticDto);
        public Task<StatisticsRo> GetFullStatistics(GetFullStatistics getFullStatistics);
        public Task<long> GetStatisticsCount(GetStatisticsCount getStatisticsCount);
        public Task<StatisticsDateRo> GetStatisticsDates();

    }
}
