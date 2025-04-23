using Archive.Dtos;
using Archive.Dtos.Incoming;
using MongoConsumerLibary.MongoConnection.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Services
{
    public interface IFrameService
    {
        public Task<Dictionary<string, List<ParamValueDict>>> GetFrames(GetFramesDto getFramesDto);
        public Task<Dictionary<string, List<ParamValueDict>>> GetIcdFrames(GetIcdFramesDto getFramesDto);
        public Task<long> GetFrameCount(GetFrameCount getFrameCount);
        public Task<FrameDatesRo> GetFrameDates(IcdType icdType);
        public Task<Dictionary<string, List<ParamValueDict>>> GetFullIcdFrames(GetFullIcdFramesDto getFullIcdFramesDto);

    }
}
