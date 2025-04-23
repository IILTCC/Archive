using Archive.Dtos;
using Archive.Dtos.Incoming;
using Archive.Services;
using Microsoft.AspNetCore.Mvc;
using MongoConsumerLibary.MongoConnection.Collections;
using MongoConsumerLibary.MongoConnection.Enums;
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
        public Task<Dictionary<string, List<ParamValueDict>>> GetFrames([FromBody] GetFramesDto getFramesDto)
        {
            return _frameService.GetFrames(getFramesDto);
        }
        [HttpPost("frameByIcd")]
        public Task<Dictionary<string, List<ParamValueDict>>> GetIcdFrames([FromBody] GetIcdFramesDto getIcdFramesDto)
        {
            return _frameService.GetIcdFrames(getIcdFramesDto);
        }
        [HttpPost("getFrameCount")]
        public Task<long> GetFrameCount([FromBody] GetFrameCount getFrameCount)
        {
            return _frameService.GetFrameCount(getFrameCount);
        }
        [HttpGet("getFrameDates")]
        public Task<FrameDatesRo> GetFrameDates([FromQuery] IcdType icdType)
        {
            return _frameService.GetFrameDates(icdType);
        }        
        [HttpPost("getFullFramesIcd")]
        public Task<Dictionary<string, List<ParamValueDict>>> GetFullIcdFrames([FromBody] GetFullIcdFramesDto getFullIcdFramesDto)
        {
            return _frameService.GetFullIcdFrames(getFullIcdFramesDto);
        }
    }
}
