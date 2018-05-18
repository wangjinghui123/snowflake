using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    using Tools.Debug;
    using Tools.Json;
    class JsonDataHandle
    {
        /// <summary>
        /// 主控服务器返回值的数据解析 
        /// </summary>
        /// <param name="message">返回数据</param>
        /// <param name="msgFromMaster">外部应用数据</param>
        /// <returns></returns>
        public bool WebDataHandle(string message, out MsgFromMaster msgFromMaster)
        {
            DebugTool.LogTag("WebDataHandle()：--- in ---", message);
            msgFromMaster = new MsgFromMaster();
            JsonData _jsonData = JsonTools.GetJsonData(message);
            if (_jsonData["status"] != null)
            {
                if (_jsonData["status"].ToString() != "0")
                {
                    DebugTool.LogTag("status:", _jsonData["status"] + "   WebNet status is wrong.");
                    return false;
                }
                else
                {
                    //待扩展其他错误(非“0”)状态码
                }
            }
            else
            {
                DebugTool.LogTag("status:", _jsonData["status"] + "   WebNet status is not finger .");
                return false;
            }
            DebugTool.LogTag("status:", _jsonData["status"] + "   WebNet status is right.");
            if (_jsonData["message"]["WMIp"] != null)
                msgFromMaster.WMIp = _jsonData["message"]["WMIp"].ToString();
            else
                DebugTool.LogWarning("WebDataHandle(): Receive[message][WMIp] is null . So it's use default value.");
            if (_jsonData["message"]["WMPort"] != null)
                msgFromMaster.WMPort = _jsonData["message"]["WMPort"].ToString();
            else
                DebugTool.LogWarning("WebDataHandle(): Receive[message][WMPort] is null . So it's use default value.");
            if (_jsonData["message"]["roomId"] != null)
                msgFromMaster.roomId = _jsonData["message"]["roomId"].ToString();
            else
                DebugTool.LogWarning("WebDataHandle(): Receive[message][roomId] is null . So it's use default value.");
            if (_jsonData["message"]["gameSign"] != null)
                msgFromMaster.gameSign = _jsonData["message"]["gameSign"].ToString();
            else
                DebugTool.LogWarning("WebDataHandle(): Receive[message][gameSign] is null . So it's use default value.");
            if (_jsonData["message"]["deviceId"] != null)
                msgFromMaster.deviceId = _jsonData["message"]["deviceId"].ToString();
            else
                DebugTool.LogWarning("WebDataHandle(): Receive[message][deviceId] is null . So it's use default value.");

            DebugTool.LogTag("WebDataHandle()：--- out ---", message);
            return true;
        }


        public bool SocketStrDataHandle(string message, out ClearDataFormat clearData)
        {
            JsonData _jsonData = JsonTools.GetJsonData(message);
            string _typeStr = string.Empty;
            clearData = new ClearDataFormat();
            if (_jsonData["Tp"] != null)
            {
                _typeStr = _jsonData["Tp"].ToString();
                int _typeInt;
                bool isSucc = int.TryParse(_typeStr, out _typeInt);
                if (isSucc)
                {
                    REV_MSG_TYPE _typeEnum = (REV_MSG_TYPE)_typeInt;
                    clearData.SetType(_typeEnum);
                    if (_jsonData["Tm"] != null)
                    {
                        string _time = _jsonData["Tm"].ToString();
                        clearData.SetTime(_time);
                    }
                    if (_jsonData["Mg"] != null)
                    {
                        string _msg = _jsonData["Mg"].ToString();
                        clearData.SetMsg(_msg);
                    }
                    return true;
                }
                else
                {
                    //type不是定义的数字类型
                    return false;
                }
            }
            else
            {
                //找不到相应Type
                return false;
            }
        }
    }

}
