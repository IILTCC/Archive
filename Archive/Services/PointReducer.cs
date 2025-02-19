using Archive.Dtos;
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
            return Consts.ASYMPTOTA * (Consts.SLOPE/Math.Pow(pointCount + Consts.HORIZONTAL_MOVEMENT,2));
        }
        // x value is n point milisecond subtracted by first point miliseconds
        private List<PointHelper> ConvertToHelper(List<ParamValueDict> paramList)
        {
            List<PointHelper> retList = new List<PointHelper>();
            foreach(ParamValueDict point in paramList)
            {
                double timeValue = point.InsertTime.Subtract(paramList[Consts.FIRST_POINT_REDUCER].InsertTime).TotalMilliseconds;
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
                int desiredPoints = (int)(paramsDict[dictKey].Count * ReduceFunction(paramsDict[dictKey].Count) /Consts.PRECENT );
                List<PointHelper> reducedList =  largestTriangle.ReducePoints(ConvertToHelper(paramsDict[dictKey]), desiredPoints);
                retDict.Add(dictKey, ConvertToParam(reducedList, paramsDict[dictKey][0].InsertTime));
            }
            return retDict;
        }
    }
}
