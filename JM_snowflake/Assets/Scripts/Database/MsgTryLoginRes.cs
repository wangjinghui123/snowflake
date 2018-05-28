using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJH
{

    public class MsgTryLoginRes             //用户尝试登陆信息
    {
        public int type
        {
            get { return (int)m_type; }
        }
        public object data
        {
            get { return m_data; }
        }
        private GAME_MESSAGE_TYPE m_type = GAME_MESSAGE_TYPE.TRY_LOGIN_RESP;
        private RankInfo m_data = new RankInfo();
        public void SetMessage(int modelType, int steps, int rankNum)
        {
            m_data.model = modelType;
            m_data.steps = steps;
            m_data.rankNum = rankNum;
        }
        public class RankInfo
        {
            public int model = 0;
            public int steps = 0;
            public int rankNum = 0;
        }
    }

}