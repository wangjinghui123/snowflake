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
	Layer * _menuLayer;		//����ͼ��
	Layer * _chatLayer;		//����ͼ��
	bool _gameFlag;      //��Ϸ��ť�Ƿ�Ϊѡ��״̬
	bool _chatFlag;      //�˵���ť�Ƿ�Ϊѡ��״̬
	bool _gameStart;		//��Ϸ�Ƿ�ʼ��־
};
#endif