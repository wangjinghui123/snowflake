#ifndef __MESSAGETYPE_H__
#define __MESSAGETYPE_H__

/*协议号，与服务器同步*/

namespace MessageType
{
	enum eMsgType
	{
		eMsg_LOGIN = 1,					//登录
		eMsg_LOGIN_RESULT,				//服务器返回结果
		eMsg_REGISTER,					//注册
		eMsg_REGISTER_RESULT,			//服务器返回结果
		eMsg_ACCOUNT_INFO,				//获取账号信息
		eMsg_ACCOUNT_INFO_RESULT,		//服务器返回结果
		eMsg_PLAYER_INFO,				//获取玩家角色数据
		eMsg_PLAYER_INFO_RESULT,		//服务器返回结果
		eMsg_START_GAME_SINGLE,			//开始自由模式
		eMsg_START_GAME_SINGLE_RESULT,	//服务器返回结果
		eMsg_START_GAME_TEAM,			//开始团战模式
		eMsg_START_GAME_SURVIVAL,		//开始生存模式
		eMsg_CHAT_SEND,					//聊天信息发送
		eMsg_CHAT_RECEIVE,				//聊天信息接收
		eMsg_MOVE,						//玩家移动
		eMsg_UPDATE_POSITION,			//更新玩家位置
		eMsg_DIVIDE,					//玩家分身操作
		eMsg_SPIT_SPORE,				//玩家吐孢子操作
		eMsg_SPIT_SPORE_RESULT,			//服务器返回结果
		eMsg_EAT_SPORE,					//玩家吃掉孢子
		eMsg_ADD_PRICK,					//添加刺
		eMsg_EAT_PRICK,					//玩家碰到刺
		eMsg_EAT_BEAN,					//玩家吃豆子
		eMsg_UPDATE_SPORE,				//更新孢子信息
		eMsg_ENTER_PLAYER,				//新玩家进入房间
		eMsg_PLAYER_CONCENTRATE,		//玩家中和操作
		eMsg_UPDATE_TIME,				//更新游戏时间

		eMsg_MAX = 9999
	};
}


#endif