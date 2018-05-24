#ifndef _RelationScene_H_
#define _RelationScene_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class RelationScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(RelationScene);

	void menuFocusCallback(Ref * pSender, CheckBox::EventType type);
	void menuFansCallback(Ref * pSender, CheckBox::EventType type);
	void menuRivalCallback(Ref * pSender, CheckBox::EventType type);
	void menuFindFriendCallback(Ref * pSender);
	void menuReturnCallback(Ref * pSender);
private:
	CheckBox * _focusItem;
	CheckBox * _fansItem;
	CheckBox * _rivalItem;
	Layer * _focusLayer;
	Layer * _fansLayer;
	Layer * _rivalLayer;
};
#endif