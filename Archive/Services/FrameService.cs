using Archive.Dtos;
using Archive.Dtos.Incoming;
using MongoConsumerLibary.MongoConnection;
using MongoConsumerLibary.MongoConnection.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Services
{
    public class FrameService : IFrameService
    {
        private readonly MongoConnection _mongoConnection;
        private readonly ZlibCompression _zlibCompression;
        public FrameService(MongoConnection mongoConnection, ZlibCompression zlibCompression)
        {
            _mongoConnection = mongoConnection;
            _zlibCompression = zlibCompression;
        }

        public async Task<List<RetFrameDto>> GetFrames(GetFramesDto getFramesDto)
        {
            getFramesDto.StartDate = ConvertToUtc(getFramesDto.StartDate);
            getFramesDto.EndDate = ConvertToUtc(getFramesDto.EndDate);
            List<BaseBoxCollection> baseBoxList =  await _mongoConnection.GetDocument(getFramesDto.FrameCount, getFramesDto.StartPoint, getFramesDto.StartDate, getFramesDto.EndDate);

            return ConvertToRetDto(getFramesDto.StartDate, getFramesDto.EndDate, baseBoxList);
        }

        public async Task<List<RetFrameDto>> GetIcdFrames(GetIcdFramesDto getIcdFramesDto)
        {
            getIcdFramesDto.StartDate = ConvertToUtc(getIcdFramesDto.StartDate);
            getIcdFramesDto.EndDate = ConvertToUtc(getIcdFramesDto.EndDate);

            List<BaseBoxCollection> baseBoxList = await _mongoConnection.GetDocument(getIcdFramesDto.CollectionType,getIcdFramesDto.FrameCount, getIcdFramesDto.StartPoint, getIcdFramesDto.StartDate, getIcdFramesDto.EndDate);
            return ConvertToRetDto(getIcdFramesDto.StartDate,getIcdFramesDto.EndDate,baseBoxList);
        }
        private DateTime ConvertToUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime,DateTimeKind.Utc);
        }
        private List<RetFrameDto> ConvertToRetDto(DateTime startDate,DateTime endDate, List<BaseBoxCollection> baseBoxList)
        {
            List<RetFrameDto> retList = new List<RetFrameDto>();
            foreach (var frame in baseBoxList)
            {
                Dictionary<string, (int value, bool wasProblemFound)> decryptedDictionary = null;
                string decompressedData = _zlibCompression.DecompressData(frame.CompressedData);
                try
                {
                    decryptedDictionary = JsonConvert.DeserializeObject<Dictionary<string, (int, bool)>>(decompressedData);
                }catch(Exception e)
                {
                }

                Dictionary<string, ParamValueDict> retDictionay = new Dictionary<string, ParamValueDict>();
                foreach (string key in decryptedDictionary.Keys)
                    retDictionay[key] = new ParamValueDict(decryptedDictionary[key].value, decryptedDictionary[key].wasProblemFound);

                RetFrameDto retFrameDto = new RetFrameDto(frame.IcdType,frame.InsertTime,frame.ExpirationTime,retDictionay);
                retList.Add(retFrameDto);
            }
            return retList;
        }
    }
}
