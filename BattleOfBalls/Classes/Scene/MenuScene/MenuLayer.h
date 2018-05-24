#ifndef _MenuLayer_H_
#define _MenuLayer_H_

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

class MenuLayer : public Layer{
public:
	MenuLayer();
	~MenuLayer();

	virtual bool init();

	CREATE_FUNC(MenuLayer);

	virtual void onExit();

	/*大厅界面的按钮使用的是MenuItemImage组件，部分按钮功能未实现*/

	void menuFindFriendCallback(Ref * pSender);  //找朋友按钮回调

	void menuExtendCallback(Ref * pSender);		//加号按钮回调

	void menuSignCallback(Ref * pSender);		//签到按钮回调

	void menuSettingCallback(Ref * pSender);		//设置按钮回调

	void menuWatchGameCallback(Ref * pSender);		//观看比赛按钮回调

	void menuGetLollyCallback(Ref * pSender);		//获取棒棒糖按钮回调

	void menuStoryCallback(Ref * pSender);			//球球故事按钮回调

	void menuStrategyCallback(Ref * pSender);		//攻略按钮回调

	void menuTeamCallback(Ref * pSender);			//战队按钮回调

	void menuRelationCallback(Ref * pSender);		//关系按钮回调

	void menuRankCallback(Ref * pSender);			//排行榜按钮回调

	void menuStoreCallback(Ref * pSender);			//商店按钮回调

	void menuPlayerCallback(Ref * pSender);			//玩家头像按钮回调

	void menuStartCallback(Ref * pSender);			//开始比赛按钮回调

	void menuSurvivalModeCallback(Ref * pSender);	//生存模式按钮回调

	void menuTeamModeCallback(Ref * pSender);		//团战模式按钮回调

	void menuCustomModeCallback(Ref * pSender);		//自定义模式按钮回调

	void menuResetNameCallback(Ref * pSender);		//比赛名字随机按钮


	void setMenuEnable(Ref * pSender);		//设置底下四个按钮是否可触摸

	void menuServerCallback(int index);		//服务器下拉菜单回调

	void gameStartEvent(EventCustom * event);		//处理服务器下发的开始游戏命令

	void gameNameEvent(Ref * pSender, TextField::EventType type);  //输入框控件回调
private:
	MenuItemImage * _extendItem1;		//加号按钮
	MenuItemImage * _extendItem2;		//减号按钮
	Vector<Menu *> _menuList;			//存放大厅中所有按钮

	TextField * _gameName;				//游戏名输入框
};

#endif