#ifndef _MaskLayer_H_
#define _MaskLayer_H_

#include "cocos2d.h"

USING_NS_CC;

/*����������һ��Ĵ���*/

class MaskLayer :public Layer{
public:
	MaskLayer();
	~MaskLayer();

	virtual bool init();

	CREATE_FUNC(MaskLayer);

	bool onTouchBegan(Touch * touch, Event * event);
	void onTouchMoved(Touch * touch, Event * event);
	void onTouchEnded(Touch * touch, Event * event);
};

#endif