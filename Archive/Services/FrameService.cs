using Archive.Dtos;
using Archive.Dtos.Incoming;
using MongoConsumerLibary.MongoConnection;
using MongoConsumerLibary.MongoConnection.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Services
{
    public class FrameService : IFrameService
    {
        private readonly MongoConnection _mongoConnection;
        public FrameService(MongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<List<BaseBoxCollection>> GetFrames(GetFramesDto getFramesDto)
        {
            getFramesDto.StartDate = ConvertToUtc(getFramesDto.StartDate);
            getFramesDto.EndDate = ConvertToUtc(getFramesDto.EndDate);

            return await _mongoConnection.GetDocument(getFramesDto.FrameCount, getFramesDto.StartPoint, getFramesDto.StartDate, getFramesDto.EndDate);
        }

        public async Task<List<BaseBoxCollection>> GetIcdFrames(GetIcdFramesDto getIcdFramesDto)
        {
            getIcdFramesDto.StartDate = ConvertToUtc(getIcdFramesDto.StartDate);
            getIcdFramesDto.EndDate = ConvertToUtc(getIcdFramesDto.EndDate);

            return await _mongoConnection.GetDocument(getIcdFramesDto.CollectionType,getIcdFramesDto.FrameCount, getIcdFramesDto.StartPoint, getIcdFramesDto.StartDate, getIcdFramesDto.EndDate);
        }
        private DateTime ConvertToUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime,DateTimeKind.Utc);
        }
    }
}
