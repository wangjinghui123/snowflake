#ifndef __GAMEMODETYPE_H__
#define __GAMEMODETYPE_H__


/*游戏类型定义*/

namespace GameMode
{
	enum eMode
	{
		eMode_SINGLE = 1,		//单人自由模式
		eMode_TEAM = 4,			//团战模式
		eMode_SURVIVAL = 8,		//生存模式
		eMode_CUSTOM = 16		//自定义模式
	};
}




#endif