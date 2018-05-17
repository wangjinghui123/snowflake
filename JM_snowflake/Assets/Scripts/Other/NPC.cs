using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    //LoginInfo _NPC_Jalps = new LoginInfo("Jalps",
    //                                    "NPC_bf3d77719bf642cad731898462d9457d",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_1.jpg",
    //                                    modleType.Laohei_Black);
    //LoginInfo _NPC_Tony = new LoginInfo("Ghost",
    //                                    "NPC_264547cda5166227dee2947f81ea2f39",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_2.jpg",
    //                                    modleType.Laohei_Green);
    //LoginInfo _NPC_QuanTing = new LoginInfo("权亭",
    //                                    "NPC_ca4335d500b9c8d7dabe961edf67c204",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_3.jpg",
    //                                    modleType.Boy_Green);
    //LoginInfo _NPC_Arron = new LoginInfo("Aaron",
    //                                    "NPC__NPC_Aaron",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_4.jpg",
    //                                    modleType.Boy_Orange);
    //LoginInfo _NPC_Leaf = new LoginInfo("得两次",
    //                                    "NPC_ae7f8bc7ffc4a67ebcae9adb31c54677",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_5.jpg",
    //                                    modleType.Nijia_Red);
    //LoginInfo _NPC_Daisy = new LoginInfo("一切简单就好",
    //                                    "NPC_89e8e02dca22e1e08826de834e80d593",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_6.jpg",
    //                                    modleType.Ninja_White);
    //LoginInfo _NPC_Alen = new LoginInfo("燕尾蝶",
    //                                    "NPC_5894535cfccfeb2188d67ebc3a2fe0ce",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_7.jpg",
    //                                    modleType.Boy_Orange);
    //LoginInfo _NPC_Quaye = new LoginInfo("Quaye",
    //                                    "NPC_e9d2cd53604e27804f921a3d331e3d05",
    //                                    "http://cdn.focuscv.cn/lookatme/common/npc/head_8.jpg",
    //                                    modleType.Boy_Orange);

    LoginInfo _NPC_A1 = new LoginInfo("九点心", "NPC_m9zo41ibo4wzs1ldkpfbvz7qf347cv2z", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/p153f4-ux2.jpg", modleType.Laohei_Black);
    LoginInfo _NPC_A2 = new LoginInfo("apas007", "NPC_hzxi6ig47nzef19fircnsgc9hr0clz1z", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/orf55z-23zj.jpg", modleType.Laohei_Green);
    LoginInfo _NPC_A3 = new LoginInfo("qq9ju0ehl3", "NPC_wmq3k7sifefbg60rik1rjugrpk7rqggs", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/onw9xx-bsu.jpg", modleType.Boy_Green);
    LoginInfo _NPC_A4 = new LoginInfo("wingalashe", "NPC_lgdz5vrpek0bvikry00nxuiyjayhpenw", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/oz8sl7-qfp.png", modleType.Nijia_Red);
    LoginInfo _NPC_A5 = new LoginInfo("winggabcd", "NPC_xrexvccntck0epjmgkjnwrzkemo01nvt", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/o0zj85-skc.jpg", modleType.Ninja_White);
    LoginInfo _NPC_A6 = new LoginInfo("wujididi", "NPC_x0vzmp0e5h7brdjyotmkv0qwd8r0eopg", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/oh7aq0-o8a.png", modleType.Boy_Orange);
    LoginInfo _NPC_A7 = new LoginInfo("哈哈哈啦啦啦", "NPC_urvc9dd9e9q53fwl3c5yy2kyy4oag624", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/p085yo-1kcm.jpg", modleType.Boy_Orange);
    LoginInfo _NPC_A8 = new LoginInfo("鹤行鸡群", "NPC_cc2j28h0qaz7iivilz128zverojqbuqf", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/p320aw-uh8.jpg", modleType.Ninja_White);
    LoginInfo _NPC_A9 = new LoginInfo("回收站", "NPC_iiuydz81lk0rq39unwy4qfe34dweo706", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/o0zcbx-1url.jpg", modleType.Boy_Orange);
    LoginInfo _NPC_A10 = new LoginInfo("酱油排", "NPC_1sjs4hv8d7ip2w56h3rhzq87rtsx2mk7", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/p20mng-bft.jpg", modleType.Laohei_Green);
    LoginInfo _NPC_A11 = new LoginInfo("司空摘星", "NPC_ss230yrpk0g2zzeimhmekg5epjbmm9vq", "http://boxact-1252079862.file.myqcloud.com/game/Weima_20180419/npc/o0zhkd-1dix.jpg", modleType.Laohei_Black);



    public PlayerController playerController;
    private List<LoginInfo> _npcInitiaList;
    private List<ScoreInfo> _scoreList;


    void Start()
    {
        //InitiaNPC(20);
    }

    private bool canChangeSocre = false;
    public void InitiaNPC(int NpcCount)
    {
        if (NpcCount == 0)
            return;
        LoginInfo[] Npcs = new LoginInfo[11];

        Npcs[0] = _NPC_A1;
        Npcs[1] = _NPC_A2;
        Npcs[2] = _NPC_A3;
        Npcs[3] = _NPC_A4;
        Npcs[4] = _NPC_A5;
        Npcs[5] = _NPC_A6;
        Npcs[6] = _NPC_A7;
        Npcs[7] = _NPC_A8;
        Npcs[8] = _NPC_A9;
        Npcs[9] = _NPC_A10;
        Npcs[10] = _NPC_A11;

        int idex;
        for (int i = 0; i < NpcCount; i++)
        {
            idex = i % Npcs.Length;

            if (_npcInitiaList == null)
                _npcInitiaList = new List<LoginInfo>();
            //int randomIdex = Random.Range(1, Npcs.Length);
            LoginInfo initiaNpc = new LoginInfo();
            initiaNpc.isNpc = ((int)InitiaItIsNpc.yes).ToString();
            initiaNpc.nickName = Npcs[idex].nickName;
            initiaNpc.userId += Npcs[idex].userId + i.ToString();

            initiaNpc.headimgurl = Npcs[idex].headimgurl;
            initiaNpc.modelType = Npcs[idex].modelType;
            _npcInitiaList.Add(initiaNpc);

            if (_scoreList == null)
                _scoreList = new List<ScoreInfo>();
            ScoreInfo npcScore = new ScoreInfo();
            npcScore.userId = _npcInitiaList[i].userId;
            _scoreList.Add(npcScore);
            playerController.SomeOneLogin(this, _npcInitiaList[i]);
        }
        canChangeSocre = true;
        //NPC_ChangeScore();
    }

    public void NPC_StopScoreCount()
    {
        CancelInvoke("_ChangeRepeatCallBack");
    }
    public void NPC_ChangeScore()
    {
        if (canChangeSocre)
            InvokeRepeating("_ChangeRepeatCallBack", 0f, 0.5f);
    }
    private void _ChangeRepeatCallBack()
    {
        StartCoroutine(_ChangeTheScore());
    }

    private IEnumerator _ChangeTheScore()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < _scoreList.Count; i++)
        {
            _scoreList[i].score = Random.Range(1, 4).ToString();
            playerController.SomeOneScoreChange(this, _scoreList[i]);
        }
    }

}
