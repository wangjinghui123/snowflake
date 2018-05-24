#ifndef _MenuScene_H_
#define _MenuScene_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class MenuScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(MenuScene);

	virtual void onExit();
	void menuGameCallback(Ref * pSender);
	void menuChatCallback(Ref * pSender);

	void gameStartEvent(EventCustom * event);
private:
	MenuItemImage * _gameItem;
	MenuItemImage * _chatItem;
	Layer * _menuLayer;		//大厅图层
	Layer * _chatLayer;		//聊天图层
	bool _gameFlag;      //游戏按钮是否为选中状态
	bool _chatFlag;      //菜单按钮是否为选中状态
	bool _gameStart;		//游戏是否开始标志
};
#endif