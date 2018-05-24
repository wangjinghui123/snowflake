#ifndef _GameSettingLayer_H_
#define _GameSettingLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"
USING_NS_CC;
using namespace ui;

class GameSettingLayer : public Layer{
public:
	GameSettingLayer();
	~GameSettingLayer();

	virtual bool init();

	CREATE_FUNC(GameSettingLayer);

	bool onTouchBegan(Touch * touch, Event * event);
	void onTouchMoved(Touch * touch, Event * event);
	void onTouchEnded(Touch * touch, Event * event);

	/*菜单按钮的回调函数*/
	void menuGameCallback(Ref * pSender);
	void menuOperationCallback(Ref * pSender);
	void menuMatchCallback(Ref * pSender);

	void menuCheckCallback(Ref * pSender, CheckBox::EventType type);
	void menuVoiceCallback(Ref * pSender);
	void menuResetCallback(Ref * pSender);
	void menuMatchCheckCallback(Ref * pSender, CheckBox::EventType type);

	void sporeSizeEvent(Ref * pSender, Slider::EventType type);
	void divideSizeEvent(Ref * pSender, Slider::EventType type);
	void opacityEvent(Ref * pSender, Slider::EventType type);

	void createGameSettingLayer();
	void createOperationSettingLayer();
	void createMatchSettingLayer();
private:
	MenuItemImage * _gameItem1, *_gameItem2;
	MenuItemImage * _operationItem1, *_operationItem2;
	MenuItemImage * _matchItem1, *_matchItem2;

	Layer * _currentLayer;
	Sprite * _spitBtn;
	Sprite * _divideBtn;
	ScrollView * _scrollView;
	CheckBox * _touchBox;
	float _touchDistance;
};

#endif