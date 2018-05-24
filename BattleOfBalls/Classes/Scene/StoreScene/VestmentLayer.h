#ifndef _VestmentLayer_H_
#define _VestmentLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class VestmentLayer : public Layer{
public:
	VestmentLayer();
	~VestmentLayer();

	virtual bool init();

	CREATE_FUNC(VestmentLayer);

	void menuYuanGuCallback(Ref * pSender, CheckBox::EventType type);
	void menuChaoFanCallback(Ref * pSender, CheckBox::EventType type);
	void menuMagicBoxCallback(Ref * pSender);
	void menuMaterialBoxCallback(Ref * pSender);
	void menuIntroductionCallback(Ref * pSender);
	void menuCombineCallback(Ref * pSender);

	void pageViewEvent(Ref * pSender, PageView::EventType type);
private:
	Vector<Sprite *> _indexSprite;
	Vec2 _touchBeganPosition;
	PageView * _pageView1;
	int _pageCount;
	int _pageIndex;
};

#endif