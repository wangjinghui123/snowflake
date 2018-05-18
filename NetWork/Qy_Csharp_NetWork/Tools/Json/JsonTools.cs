using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Tools.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    public class JsonData //日后扩展各种类型输出--持续研究中
    {
        public JObject JObject { get{ return m_jobj; } }
        private JObject m_jobj;
        public JToken this[string propertyName]
        {
            get
            {
                try
                {
                    return m_jobj[propertyName];
                }
                catch (Exception ee)
                {
                    Debug.DebugTool.LogWarning(ee.Message);
                    return null;
                }
            }
        }
        public void SetJObject(JObject jobj)
        {
            m_jobj = jobj;
        }
        public T ToObject<T>()
        {
            return m_jobj.ToObject<T>();
        }
    }
    public class JsonTools
    {
        public static string ToJson(object param)
        {
            JsonSerializerSettings _seting = new JsonSerializerSettings();
            string _targetJson = JsonConvert.SerializeObject(param);
            return _targetJson;
        }
        public static JsonData GetJsonData(string jsonStr)
        {
            JObject _jobj = JObject.Parse(jsonStr) as JObject;
            JsonData jdata = new JsonData();
            jdata.SetJObject(_jobj);
            return jdata;
        }

    }
}
