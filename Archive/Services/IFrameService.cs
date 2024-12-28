using Archive.Dtos;
using Archive.Dtos.Incoming;
using MongoConsumerLibary.MongoConnection.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Services
{
    public interface IFrameService
    {
        public Task<List<BaseBoxCollection>> GetFrames(GetFramesDto getFramesDto);
        public Task<List<BaseBoxCollection>> GetIcdFrames(GetIcdFramesDto getFramesDto);
    }
}
