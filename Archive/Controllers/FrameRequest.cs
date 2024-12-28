using Archive.Dtos;
using Archive.Dtos.Incoming;
using Archive.Services;
using Microsoft.AspNetCore.Mvc;
using MongoConsumerLibary.MongoConnection.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameRequest : Controller
    {
        private readonly IFrameService _frameService;
        public FrameRequest(IFrameService frameService)
        {
            _frameService = frameService;
        }
       [HttpPost("getFrames")]
        public Task<List<BaseBoxCollection>> GetFrames([FromBody] GetFramesDto getFramesDto)
        {
            return _frameService.GetFrames(getFramesDto);
        }
        [HttpPost("getIcdFrames")]
        public Task<List<BaseBoxCollection>> GetIcdFrames([FromBody] GetIcdFramesDto getIcdFramesDto)
        {
            return _frameService.GetIcdFrames(getIcdFramesDto);
        }
    }
}
