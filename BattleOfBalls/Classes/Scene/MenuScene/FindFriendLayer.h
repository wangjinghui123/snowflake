#ifndef _FindFriendLayer_H_
#define _FindFriendLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class FindFriendLayer : public Layer, public TextFieldDelegate{
public:
	FindFriendLayer();
	~FindFriendLayer();

	virtual bool init();

	CREATE_FUNC(FindFriendLayer);

	void menuFindCallback(Ref * pSender);
	void menuNearbyCallback(Ref * pSender);
	void menuInterestedCallback(Ref * pSender);
	void menuCloseCallback(Ref * pSender);
	void menuSearchCallback(Ref * pSender);
	void menuSaoYiSaoCallback(Ref * pSender);
	void menuLeiDaCallback(Ref * pSender);
	void menuYaoYiYaoCallback(Ref * pSender);

	void createFindLayer();
	void createNearbyLayer();
	void createInterestedLayer();

	virtual bool onTextFieldAttachWithIME(TextFieldTTF*  pSender);
	virtual bool onTextFieldDetachWithIME(TextFieldTTF* pSender);
	virtual bool onTextFieldInsertText(TextFieldTTF*  pSender, const char * text, size_t nLen);
	virtual bool onTextFieldDeleteBackward(TextFieldTTF*  pSender, const char * delText, size_t nLen);
private:
	MenuItemImage * _findItem1;
	MenuItemImage * _findItem2;
	MenuItemImage * _nearbyItem1;
	MenuItemImage * _nearbyItem2;
	MenuItemImage * _interestedItem1;
	MenuItemImage * _interestedItem2;

	TextFieldTTF * _textField;
};

#endif