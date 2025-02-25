using Archive.Dtos;
using System.Threading.Tasks;

namespace Archive.Services
{
    public interface IStatisticService
    {
        public Task<StatisticsRo> GetStatistics(GetStatisticDto getStatisticDto);
    }
}
