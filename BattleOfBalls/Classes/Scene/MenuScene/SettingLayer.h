#ifndef _SettingLayer_H_
#define _SettingLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class SettingLayer : public Layer{
public:
	SettingLayer();
	~SettingLayer();

	virtual bool init();

	CREATE_FUNC(SettingLayer);

	void menuAccountCallback(Ref * pSender);
	void menuGameCallback(Ref * pSender);
	void menuCloseCallback(Ref * pSender);
private:
	MenuItemImage * _accountItem1, *_accountItem2;
	MenuItemImage * _gameItem1, *_gameItem2;
	Layer * _accountSettingLayer;
	Layer * _gameSettingLayer;
	Layer * _currentLayer;
};

#endif