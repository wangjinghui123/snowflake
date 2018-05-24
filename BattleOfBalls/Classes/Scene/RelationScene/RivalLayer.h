#ifndef _RivalLayer_H_
#define _RivalLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class RivalLayer : public Layer{
public:
	RivalLayer();
	~RivalLayer();

	virtual bool init();

	CREATE_FUNC(RivalLayer);
};

#endif