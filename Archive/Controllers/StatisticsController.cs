using Archive.Dtos;
using Archive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [HttpPost("getStatistics")]
        public Task<StatisticsRo> GetStatistics(GetStatisticDto getStatisticDto)
        {
            return _statisticService.GetStatistics(getStatisticDto);
        }
    }
}
