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

        // fucntion to remove more points the bigger the input points non lineary
        private double ReduceFunction(int pointCount)
        {
            return (Consts.UPPER_ASYMPTOTA_LIMIT / (1 + Consts.SLOPE / pointCount));
        }
        // x value is n point milisecond subtracted by first point miliseconds
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
                int desiredPoints = (int)(paramsDict[dictKey].Count * (Consts.PRECENT- ReduceFunction(paramsDict[dictKey].Count)) /Consts.PRECENT );
                List<PointHelper> reducedList =  largestTriangle.ReducePoints(ConvertToHelper(paramsDict[dictKey]), desiredPoints);
                retDict.Add(dictKey, ConvertToParam(reducedList, paramsDict[dictKey][0].PacketTime));
            }
            return retDict;
        }
        public Dictionary<string, List<GraphPoint>> ReducePoint(Dictionary<string,List<GraphPoint>> statisticsDict)
        {
            LargestTriangle<GraphPoint> largestTriangle = new LargestTriangle<GraphPoint>();
            Dictionary<string, List<GraphPoint>> retDict = new Dictionary<string, List<GraphPoint>>();
            foreach(string dictKey in statisticsDict.Keys)
            {
                int desiredPoints = (int)(statisticsDict[dictKey].Count * (Consts.PRECENT - ReduceFunction(statisticsDict[dictKey].Count)) / Consts.PRECENT);
                retDict.Add(dictKey, largestTriangle.ReducePoints(statisticsDict[dictKey],desiredPoints));
            }
            return retDict;
        }
    }
}
