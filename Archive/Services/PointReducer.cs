using Archive.Dtos;
using DownSamplingLibary;
using DownSamplingLibary.LargestTriangle;
using System;
using System.Collections.Generic;

namespace Archive.Services
{
    public class PointReducer
    {
        public PointReducer() { }

        private int ReduceFunction(int pointCount)
        {
            if (pointCount > Consts.MAX_POINTS)
                return Consts.MAX_POINTS;
            return pointCount;
        }
        private List<PointHelper> ConvertToHelper(List<ParamValueDict> paramList)
        {
            List<PointHelper> retList = new List<PointHelper>();
            foreach(ParamValueDict point in paramList)
            {
                double timeValue = point.PacketTime.Subtract(paramList[Consts.FIRST_POINT_REDUCER].PacketTime).TotalMilliseconds;
                retList.Add(new PointHelper(timeValue, point.Value, point.IsFaulty));
            }
            return retList;
        }
        private List<ParamValueDict> ConvertToParam(List<PointHelper> pointsList,DateTime startPoint)
        {
            List<ParamValueDict> retList = new List<ParamValueDict>();
            foreach (PointHelper point in pointsList)
                retList.Add(new ParamValueDict((int)point.Y,point.IsFaulty,startPoint.AddMilliseconds(point.X)));
            return retList;
        }

        public Dictionary<string, List<ParamValueDict>> ReducePoints(Dictionary<string, List<ParamValueDict>> paramsDict)
        {
            LargestTriangle<PointHelper> largestTriangle = new LargestTriangle<PointHelper>();
            Dictionary<string, List<ParamValueDict>> retDict = new Dictionary<string, List<ParamValueDict>>();
            foreach(string dictKey in paramsDict.Keys)
            {
                List<PointHelper> reducedList =  largestTriangle.ReducePoints(ConvertToHelper(paramsDict[dictKey]), ReduceFunction(paramsDict[dictKey].Count));
                retDict.Add(dictKey, ConvertToParam(reducedList, paramsDict[dictKey][0].PacketTime));
            }
            return retDict;
        }
        public Dictionary<string, List<StatisticsPoint>> ReducePoint(Dictionary<string,List<StatisticsPoint>> statisticsDict)
        {
            LargestTriangle<StatisticsPoint> largestTriangle = new LargestTriangle<StatisticsPoint>();
            Dictionary<string, List<StatisticsPoint>> retDict = new Dictionary<string, List<StatisticsPoint>>();
            foreach(string dictKey in statisticsDict.Keys)
                retDict.Add(dictKey, largestTriangle.ReducePoints(statisticsDict[dictKey],ReduceFunction(statisticsDict[dictKey].Count)));
            
            return retDict;
        }
    }
}
