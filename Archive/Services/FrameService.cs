using Archive.Dtos;
using Archive.Dtos.Incoming;
using Archive.Logs;
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
        private readonly PointReducer _pointReducer;
        private readonly ArchiveLogger _logger;
        public FrameService(MongoConnection mongoConnection, ZlibCompression zlibCompression,PointReducer pointReducer,ArchiveLogger logger)
        {
            _mongoConnection = mongoConnection;
            _zlibCompression = zlibCompression;
            _pointReducer = pointReducer;
            _logger = logger;
        }

        public async Task<Dictionary<string, List<ParamValueDict>>> GetFrames(GetFramesDto getFramesDto)
        {
            getFramesDto.StartDate = ConvertToUtc(getFramesDto.StartDate);
            getFramesDto.EndDate = ConvertToUtc(getFramesDto.EndDate);
            List<BaseBoxCollection> baseBoxList =  await _mongoConnection.GetDocument(getFramesDto.FrameCount, getFramesDto.StartPoint, getFramesDto.StartDate, getFramesDto.EndDate);

            return MapFramesToDictionary(getFramesDto.StartDate, getFramesDto.EndDate, baseBoxList);
        }

        public async Task<Dictionary<string, List<ParamValueDict>>> GetIcdFrames(GetIcdFramesDto getIcdFramesDto)
        {
            getIcdFramesDto.StartDate = ConvertToUtc(getIcdFramesDto.StartDate);
            getIcdFramesDto.EndDate = ConvertToUtc(getIcdFramesDto.EndDate);
            List<BaseBoxCollection> baseBoxList = await _mongoConnection.GetDocument(getIcdFramesDto.CollectionType,getIcdFramesDto.FrameCount, getIcdFramesDto.StartPoint, getIcdFramesDto.StartDate, getIcdFramesDto.EndDate);
            return MapFramesToDictionary(getIcdFramesDto.StartDate,getIcdFramesDto.EndDate,baseBoxList);
        }
        private DateTime ConvertToUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime,DateTimeKind.Utc);
        }
        private Dictionary<string,List<ParamValueDict>> MapFramesToDictionary(DateTime startDate,DateTime endDate, List<BaseBoxCollection> baseBoxList)
        {
            Dictionary<string, List<ParamValueDict>> retDictionary = new Dictionary<string, List<ParamValueDict>>();

            foreach (BaseBoxCollection frame in baseBoxList)
            {
                Dictionary<string, (int value, bool wasProblemFound)> decodeDictionary = null;
                string decompressedData = _zlibCompression.DecompressData(frame.CompressedData);
                try
                {
                    decodeDictionary = JsonConvert.DeserializeObject<Dictionary<string, (int, bool)>>(decompressedData);
                }catch(Exception e) 
                {
                    _logger.LogError("Tried converting json to packet "+e.Message,LogId.FatalJsonConvert);
                }

                foreach (string key in decodeDictionary.Keys)
                {
                    if (!retDictionary.ContainsKey(key))
                        retDictionary.Add(key,new List<ParamValueDict>());
                    
                    retDictionary[key].Add(new ParamValueDict(decodeDictionary[key].value, decodeDictionary[key].wasProblemFound,frame.RealTime));
                }
            }

            return _pointReducer.ReducePoints(retDictionary);
        }
        public async Task<long> GetFrameCount(GetFrameCount getFrameCount)
        {
            getFrameCount.StartDate = ConvertToUtc(getFrameCount.StartDate);
            getFrameCount.EndDate = ConvertToUtc(getFrameCount.EndDate);
            return await _mongoConnection.GetDocumentCount(getFrameCount.StartDate, getFrameCount.EndDate, getFrameCount.CollectionType);
        }
    }
}
