using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    using Tools.Debug;
    enum DEVICED_TYPD
    {
        MACS = 0,
        DEVICE_ONE = 1,
        DEVICE_TWO = 2,
        DEVICE_THREE = 3,

    }
    enum SOCKET_TYPE
    {
        SOCKET = 0,
        WEB_SOCKET = 1,
    }
    class SndToMasterData
    {
        public SndToMasterData(string token_param, string gameId_param, string[] macOrDevice_param, DEVICED_TYPD deviceType)
        {
            try
            {
                m_token = token_param;
                m_gameId = gameId_param;
                m_socketType = ((int)SOCKET_TYPE.SOCKET).ToString();
                m_deviceType = ((int)deviceType).ToString();
                m_deviceId = ChosseDevice(deviceType, macOrDevice_param);
            }
            catch (Exception ee)
            {
                DebugTool.LogWarning("SndToMasterData():There is a wrong operation :" + ee.Message);
            }
        }
        public string token { get { return m_token; } }
        public string gameId { get { return m_gameId; } }
        public string socketType { get { return m_socketType; } }
        public string deviceType { get { return m_deviceType; } }
        public object deviceId { get { return m_deviceId; } }

        private string m_token = string.Empty;
        private string m_gameId = string.Empty;
        private string m_socketType = string.Empty;
        private string m_deviceType = string.Empty;
        private object m_deviceId = null;

        private object ChosseDevice(DEVICED_TYPD deviceType, string[] macOrDevice)
        {
            object _target = null;
            switch (deviceType)
            {
                case DEVICED_TYPD.MACS:
                    _target = new Macs(macOrDevice);
                    break;
                case DEVICED_TYPD.DEVICE_ONE:
                    _target = new DeviceId_1(macOrDevice[0]);
                    break;
                case DEVICED_TYPD.DEVICE_TWO:
                    _target = new DeviceId_2(macOrDevice[0], macOrDevice[1]);
                    break;
                case DEVICED_TYPD.DEVICE_THREE:
                    _target = new DeviceId_3(macOrDevice[0], macOrDevice[1], macOrDevice[2]);
                    break;
            }
            return _target;
        }
        class Macs
        {
            public Macs(string[] macsId)
            {
                macs = macsId;
            }
            public string[] macs;
        }
        class DeviceId_1
        {
            public DeviceId_1(string id0_param)
            {
                id0 = id0_param;
            }
            public string id0 = string.Empty;
        }
        class DeviceId_2
        {
            public DeviceId_2(string id0_param, string id1_param)
            {
                id0 = id0_param;
                id1 = id1_param;
            }
            public string id0 = string.Empty;
            public string id1 = string.Empty;
        }
        class DeviceId_3
        {
            public DeviceId_3(string id0_param, string id1_param, string id2_param)
            {
                id0 = id0_param;
                id1 = id1_param;
                id2 = id2_param;
            }
            public string id0 = string.Empty;
            public string id1 = string.Empty;
            public string id2 = string.Empty;
        }

    }

}
