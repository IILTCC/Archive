using Archive.Dtos;
using DownSamplingLibary;
using MongoConsumerLibary.MongoConnection;
using MongoConsumerLibary.MongoConnection.Collections.PropetyClass;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly MongoConnection _mongoConnection;
        private readonly PointReducer _pointReducer;
        public StatisticService(MongoConnection mongoConnection, ZlibCompression zlibCompression, PointReducer pointReducer)
        {
            _mongoConnection = mongoConnection;
            _pointReducer = pointReducer;
        }
        public async Task<StatisticsRo> GetStatistics(GetStatisticDto getStatisticDto)
        {
            getStatisticDto.StartDate = ConvertToUtc(getStatisticDto.StartDate);
            getStatisticDto.EndDate = ConvertToUtc(getStatisticDto.EndDate);
            List<StatisticCollection> statisticsList = await _mongoConnection.GetStatisticDocument(getStatisticDto.StatisticsCount, getStatisticDto.StartPoint,getStatisticDto.StartDate, getStatisticDto.EndDate);

            StatisticsRo retDto = new StatisticsRo();
            retDto.Graphs = MapStatisticsGraph(statisticsList);
            (retDto.SevirityValues,retDto.Values) = MapStatisticsLastValues(statisticsList);
            return retDto;
        }
        private DateTime ConvertToUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
        public Dictionary<string,List<GraphPoint>> MapStatisticsGraph(List<StatisticCollection> statisticList)
        {
            Dictionary<string, List<GraphPoint>> retDict = new Dictionary<string, List<GraphPoint>>();
            Dictionary<string, DateTime> startingPointDict = new Dictionary<string, DateTime>();
            InitWorkingDicts(ref retDict, ref startingPointDict, statisticList);

            retDict = _pointReducer.ReducePoint(retDict);
            // adding back the time to miliseconds
            foreach(string dictKey in retDict.Keys)
                for(int pointIndex = 0; pointIndex<retDict[dictKey].Count;pointIndex++)
                    retDict[dictKey][pointIndex].X = new DateTimeOffset(startingPointDict[dictKey].AddMilliseconds(retDict[dictKey][pointIndex].X)).ToUnixTimeMilliseconds();

            return retDict;
        }
        public void InitWorkingDicts(ref Dictionary<string,List<GraphPoint>> retDict, ref Dictionary<string,DateTime> startingPointDict, List<StatisticCollection> statisticList)
        {
            foreach (StatisticCollection collection in statisticList)
            {
                foreach (string dictionaryKey in collection.StatisticValues.Keys)
                {
                    if (!retDict.ContainsKey(dictionaryKey))
                    {
                        retDict.Add(dictionaryKey, new List<GraphPoint>());
                        startingPointDict.Add(dictionaryKey, collection.RealTime);
                    }

                    // converting all points time to start from 0 relative to first point
                    retDict[dictionaryKey].Add(new GraphPoint(startingPointDict[dictionaryKey].Subtract(collection.RealTime).TotalMilliseconds, collection.StatisticValues[dictionaryKey].Value));
                }
            }
        }

        public (Dictionary<string,int> sevirity, Dictionary<string,float> value) MapStatisticsLastValues(List<StatisticCollection> statisticsList)
        {

            Dictionary<string, int> retSevirityDict = new Dictionary<string, int>();
            Dictionary<string, float> retValueDict = new Dictionary<string, float>();
            if (statisticsList.Count == 0)
                return (retSevirityDict, retValueDict);
            Dictionary<string, StatisticDictValue> lastValue = statisticsList[statisticsList.Count - 1].StatisticValues;
            foreach (string parameterName in lastValue.Keys)
            {
                retSevirityDict.Add(parameterName,lastValue[parameterName].Sevirity);
                retValueDict.Add(parameterName, lastValue[parameterName].Value);
            }
            return (retSevirityDict, retValueDict);
        }

        public async Task<long> GetStatisticsCount(GetStatisticsCount getStatisticsCount)
        {
            getStatisticsCount.StartDate = ConvertToUtc(getStatisticsCount.StartDate);
            getStatisticsCount.EndDate = ConvertToUtc(getStatisticsCount.EndDate);
            return await _mongoConnection.GetStatisticsCount(getStatisticsCount.StartDate, getStatisticsCount.EndDate);

        }
    }
}
