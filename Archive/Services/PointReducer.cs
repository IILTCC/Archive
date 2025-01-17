using Archive.Dtos;
using DownSamplingLibary.LargestTriangle;
using System;
using System.Collections.Generic;

namespace Archive.Services
{
    public class PointReducer
    {
        public PointReducer() { }

        private double ReduceFunction(int pointCount)
        {
            const int asymptota= 98;
            const int slope = 20000;
            const double horizontalMovement = 141.4213563;
            return asymptota * (slope/Math.Pow(pointCount + horizontalMovement,2));
        }
        private List<PointHelper> ConvertToHelper(List<ParamValueDict> paramList)
        {
            List<PointHelper> retList = new List<PointHelper>();
            foreach(ParamValueDict point in paramList)
                retList.Add(new PointHelper(point.InsertTime.Subtract(paramList[0].InsertTime).TotalMilliseconds, point.Value, point.IsFaulty));
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
                int desiredPoints = (int)(paramsDict[dictKey].Count * ReduceFunction(paramsDict[dictKey].Count) / 100);
                List<PointHelper> reducedList =  largestTriangle.ReducePoints(ConvertToHelper(paramsDict[dictKey]), desiredPoints);
                retDict.Add(dictKey, ConvertToParam(reducedList, paramsDict[dictKey][0].InsertTime));
            }
            return retDict;
        }
    }
}
