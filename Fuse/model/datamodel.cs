using System.Collections.Generic;
using System.Collections;
using System;

namespace Fuze
{
	/// <summary>
	/// Its the Data Model used for deserialization structures
	/// </summary>
	public class FuzeDataModel
	{
        public class Config
        {
            public string name { get; set; }
            public string appName { get; set; }
            public List<string> domain { get; set; }
            public string image { get; set; }
            public List<Dictionary<string,string>> env { get; set; }
            public List<Dictionary<string,string>> secret { get; set; }
        }
        public class K8SettingObject
        {
            public List<Config> configs { get; set; }
        }          
    }
}