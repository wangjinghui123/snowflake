#ifndef _PlayerSkinLayer_H_
#define _PlayerSkinLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class PlayerSkinLayer : public Layer{
public:
	PlayerSkinLayer();
	~PlayerSkinLayer();

	virtual bool init();

	CREATE_FUNC(PlayerSkinLayer);

	void menuSkinCallback(Ref * pSender, CheckBox::EventType type);
	void menuVestmentCallback(Ref * pSender, CheckBox::EventType type);
private:
	CheckBox * _skinItem;
	CheckBox * _vestmentItem;
};

#endif