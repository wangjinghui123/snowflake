#ifndef _LoginLayer_H_
#define _LoginLayer_H_

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

class LoginLayer : public Layer{
public:
	LoginLayer();
	~LoginLayer();

	virtual bool init();

	CREATE_FUNC(LoginLayer);

	bool onTouchBegan(Touch * touch, Event * event);
	void onTouchMoved(Touch * touch, Event * event);
	void onTouchEnded(Touch * touch, Event * event);

	void textFieldNameEvent(Ref * pSender, TextField::EventType type);
	void textFieldPasswordEvent(Ref * pSender, TextField::EventType type);
	void menuLoginCallback(Ref * pSender);
	void menuForgetCallback(Ref * pSender);

	void addMask();

	void loginResultEvent(EventCustom * event);
private:
	TextField * _textFieldName;
	TextField * _textFieldPassword;
	Sprite * _background;
};

#endif