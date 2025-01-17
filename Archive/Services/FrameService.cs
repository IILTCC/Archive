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
        private readonly PointReducer _pointReducer;
        public FrameService(MongoConnection mongoConnection, ZlibCompression zlibCompression,PointReducer pointReducer)
        {
            _mongoConnection = mongoConnection;
            _zlibCompression = zlibCompression;
            _pointReducer = pointReducer;
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
                }catch(Exception e) {}

                foreach (string key in decodeDictionary.Keys)
                {
                    if (!retDictionary.ContainsKey(key))
                        retDictionary.Add(key,new List<ParamValueDict>());
                    
                    retDictionary[key].Add(new ParamValueDict(decodeDictionary[key].value, decodeDictionary[key].wasProblemFound,frame.InsertTime));
                }
            }

            return _pointReducer.ReducePoints(retDictionary);
        }

    }
}
