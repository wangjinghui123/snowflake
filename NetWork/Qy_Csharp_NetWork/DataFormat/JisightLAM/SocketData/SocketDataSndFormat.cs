using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{

    class MsgPushHeartBeat : PostGetMsg
    {
        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }
        private SND_MSG_TYPE m_type = SND_MSG_TYPE.HEART_BEAT;
        private object m_msgData;
    }
    class MsgPostRoomVerification : PostGetMsg
    {
        public MsgPostRoomVerification(string deviceId, string roomId, string gameSign)
        {
            m_type = SND_MSG_TYPE.ROOM_VERIFICATION;
            m_msgData = new MsgData();
            m_msgData.deviceId = deviceId;
            m_msgData.roomId = roomId;
            m_msgData.gameSign = gameSign;
        }
        public override string Tp { get { return ((int)m_type).ToString(); } }
        private SND_MSG_TYPE m_type;
        private MsgData m_msgData;
        public override object Mg { get { return m_msgData; } }
        class MsgData
        {
            public string deviceId = string.Empty;
            public string roomId = string.Empty;
            public string gameSign = string.Empty;
        }
    }
    class MsgPushGameStatus : PushMsg
    {
        public MsgPushGameStatus(string status)
        {
            m_msgData = new MsgData();
            m_msgData.gameStatus = status;
        }
        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }

        private SND_MSG_TYPE m_type = SND_MSG_TYPE.ROOM_STATUS;
        private MsgData m_msgData;

        class MsgData
        {
            public string gameStatus = string.Empty;
        }
    }
    class MsgPushMsgToOther : PushMsg
    {
        public MsgPushMsgToOther(object msg)
        {
            m_msgData = msg;
        }
        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }

        private SND_MSG_TYPE m_type = SND_MSG_TYPE.PC_H5;
        private object m_msgData;
    }
    class MsgPushRankList : PushMsg
    {
        public MsgPushRankList(Dictionary<int, string> rank_userIdDic)
        {
            m_msgData = new List<RankUser>();
            foreach (KeyValuePair<int, string> ky in rank_userIdDic)
            {
                RankUser tempUser = new RankUser();
                tempUser.userId = ky.Value;
                tempUser.rankNum = ky.Key.ToString();
                m_msgData.Add(tempUser);
            }
        }
        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }

        private SND_MSG_TYPE m_type = SND_MSG_TYPE.SUMMARIZE;
        private List<RankUser> m_msgData;

        class RankUser
        {
            public string rankNum = "";
            public string userId = string.Empty;
        }
    }
    class MsgPostAwadeList : PostGetMsg
    {
        public MsgPostAwadeList(Dictionary<string, int> userIdCountAwardList)
        {
            m_msgData.type = 1;
            foreach (KeyValuePair<string, int> ky in userIdCountAwardList)
            {
                Msg.AwardInfo info = new Msg.AwardInfo();
                info.userId = ky.Key;
                info.count = ky.Value;
                m_msgData.data.Add(info);
            }
        }
        public MsgPostAwadeList(List<int> countList)
        {
            Msg.AwardInfo2 info = new Msg.AwardInfo2();
            m_msgData.type = 2;
            for (int idex = 0; idex < countList.Count; ++idex)
            {
                info.count = countList[idex];
                m_msgData.data.Add(info);
            }
        }
        //神秘大奖
        public MsgPostAwadeList(Dictionary<string, int> userIdCountAwardList, string mysterious)
        {
            Msg.AwardInfo3 info = new Msg.AwardInfo3();
            info.mysterious = mysterious;
            m_msgData.type = 3;
            m_msgData.data.Add(info);
            foreach (KeyValuePair<string, int> ky in userIdCountAwardList)
            {
                Msg.AwardInfo user = new Msg.AwardInfo();
                user.userId = ky.Key;
                user.count = ky.Value;
                info.users.Add(user);
            }
        }
        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }

        private SND_MSG_TYPE m_type = SND_MSG_TYPE.AWARDE_REQUEST;
        private Msg m_msgData = new Msg();
        public class Msg
        {
            public int type = 1;

            public List<object> data = new List<object>();
            public class AwardInfo
            {
                public string userId = string.Empty;
                public int count = 0;
            }
            public class AwardInfo2
            {
                public int count = 0;
            }
            public class AwardInfo3
            {
                public List<AwardInfo> users = new List<AwardInfo>();
                public string mysterious = "";
            }
        }
    }


    class MsgPostAwadeList_Leo : PostGetMsg
    {
        public MsgPostAwadeList_Leo(Dictionary<string, int> userIdCountAwardList)
        {
            foreach (KeyValuePair<string, int> ky in userIdCountAwardList)
            {
                Msg info = new Msg();
                info.userId = ky.Key;
                info.is_npc = ky.Value;
                m_msgData.Add(info);
            }
        }

        public override string Tp { get { return ((int)m_type).ToString(); } }
        public override object Mg { get { return m_msgData; } }

        private SND_MSG_TYPE m_type = SND_MSG_TYPE.AWARDE_REQUEST;
        private List<Msg> m_msgData = new List<Msg>();
        public class Msg
        {
            public string userId = string.Empty;
            public int is_npc = 0;
        }
    }

}

