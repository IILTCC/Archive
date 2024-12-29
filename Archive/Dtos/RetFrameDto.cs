using System;
using System.Collections.Generic;

namespace Archive.Dtos
{
    public class RetFrameDto
    {
        public RetFrameDto(string icdType , DateTime insertTime, DateTime expirationDate, Dictionary<string,ParamValueDict> paramDictionary)
        {
            IcdType = icdType;
            InsertTime = insertTime;
            ExpirationTime = expirationDate;
            ParamDictionary = paramDictionary;
        }
        public string IcdType { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public Dictionary<string, ParamValueDict> ParamDictionary { get; set; }
    }
}
