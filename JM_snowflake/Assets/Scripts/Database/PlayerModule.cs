using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJH
{



    public enum modleType
    {
        None = -1,
        Module1 = 0,
    }

    /// <summary>
    /// 玩家数据
    /// </summary>
    public class PlayerData : EventArgs
    {
        /// <summary>
        /// 玩家信息
        /// </summary>
        /// <param name="nickName">昵称</param>
        /// <param name="userId">玩家id</param>
        /// <param name="headPortraitUrl">头像的url</param>
        /// <param name="modleType">模型类型</param>
        public PlayerData(string nickName, string userId, string headPortraitUrl, modleType modleType)
        {
            _nickName = nickName;
            _userId = userId;
            _headPortraitUrl = headPortraitUrl;
            _modleType = modleType;
        }

        public string nickName
        {
            get
            {
                return _nickName;
            }
            set
            {
                _nickName = value;
            }
        }
        private string _nickName = string.Empty;

        public string userId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }
        private string _userId = string.Empty;

        public string headimgurl
        {
            get
            {
                return _headPortraitUrl;
            }
            set
            {
                _headPortraitUrl = value;
            }
        }
        private string _headPortraitUrl;

        public int modelType
        {
            get
            {
                return ((int)_modleType);
            }
            set
            {
                _modleType = (modleType)int.Parse(value.ToString());
            }
        }
        private modleType _modleType;
    }


    public class PlayerModule
    {
        static PlayerModule instance;
        Dictionary<string, PlayerData> playerdata;
        public static PlayerModule Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerModule();
                }
                return instance;
            }
        }
        private Dictionary<string, LoginInfo> tryLoginInfo;

        private Dictionary<string, PlayerData> PlayerdataDic
        {
            get
            {
                if (playerdata == null)
                {
                    playerdata = new Dictionary<string, PlayerData>();
                }
                return playerdata;
            }
        }

        public Dictionary<string, LoginInfo> TryLoginInfo
        {
            get
            {
                if (tryLoginInfo == null)
                {
                    tryLoginInfo = new Dictionary<string, LoginInfo>();
                }
                return tryLoginInfo;
            }
        }
        public void Add(string userId)
        {
            if (TryLoginInfo.ContainsKey(userId))
            {
                Debug.LogError("包含此尝试登陆的玩家信息");

            }
        }
        /// <summary>
        /// 是否包含尝试登陆的userID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ContainsTryUser(string userId)
        {
            if (TryLoginInfo.ContainsKey(userId))
            {
                return true;
            }
            return false;
        }

        public LoginInfo GetTryLogininfo(string userId)
        {
            if (TryLoginInfo.ContainsKey(userId))
            {
                return TryLoginInfo[userId];
            }
            else
            {
                return null;
            }
        }


        private PlayerModule() { }

        public PlayerData GetPlayerDataByID(string userId)
        {
            if (PlayerdataDic.ContainsKey(userId))
            {
                return PlayerdataDic[userId];
            }
            else
            {
                return null;
            }
        }

        public List<PlayerData> GetAllPlayer()
        {
            if (PlayerdataDic.Count != 0)
            {
                List<PlayerData> list = new List<PlayerData>();

                foreach (var item in PlayerdataDic)
                {
                    list.Add(item.Value);
                }
                return list;
            }
            return null;
        }


        public void Add(string userID, PlayerData data)
        {

            if (PlayerdataDic.ContainsKey(userID))
            {
                Debug.LogException(new System.Exception("已存在该玩家:" + userID));
            }
            else
            {
                UpdateInfo(userID, data);
            }
        }

        public void UpdateInfo(string userID, PlayerData data)
        {
            PlayerdataDic.Add(userID, data);
            ///玩家登陆成功
        }

        public void ClearData()
        {
            PlayerdataDic.Clear();
        }

        public void RemovePlayer(string userId)
        {
            if (PlayerdataDic.ContainsKey(userId))
            {
                PlayerdataDic.Remove(userId);
            }
            else
            {
                Debug.LogError("该玩家不存在");
            }
        }

    }

}