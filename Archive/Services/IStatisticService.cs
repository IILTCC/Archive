using Archive.Dtos;
using System.Threading.Tasks;

namespace Archive.Services
{
    public interface IStatisticService
    {
        public Task<RetStatisticDto> GetStatistics(GetStatisticDto getStatisticDto);
    }
}
