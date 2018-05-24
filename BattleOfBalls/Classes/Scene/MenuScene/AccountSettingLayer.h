#ifndef _AccountSettingLayer_H_
#define _AccountSettingLayer_H_

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

class AccountSettingLayer : public Layer{
public:
	AccountSettingLayer();
	~AccountSettingLayer();

	virtual bool init();

	CREATE_FUNC(AccountSettingLayer);

	/*菜单按钮的回调函数*/
	void menuModifyNameCallback(Ref * pSender);
	void menuModifyPasswordCallback(Ref * pSender);
	void menuChangePhoneCallback(Ref * pSender);
	
	void sexChangeCallback(Ref * pSender,CheckBox::EventType type);
	void menuSwitchAccountCallback(int index);

	void textFieldNameEvent(Ref * pSender, TextField::EventType type);
	void textFieldPasswordEvent(Ref * pSender, TextField::EventType type);
	void textFieldAgeEvent(Ref * pSender, TextField::EventType type);


	void loginSuccessEvent(EventCustom * event);		//登录成功后请求账号信息

	void accountInfoResult(EventCustom * event);		//账号信息回包
private:
	MenuItemImage * _maleItem1, *_maleItem2;
	MenuItemImage * _femaleItem1, *_femaleItem2;
	CheckBox * _maleBox;
	CheckBox * _femaleBox;

	TextField * _textFieldName;
	TextField * _textFieldPassword;
	TextField * _textFieldAge;
};

#endif