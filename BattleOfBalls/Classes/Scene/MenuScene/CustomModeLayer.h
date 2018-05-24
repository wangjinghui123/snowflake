#ifndef _CustomModeLayer_H_
#define _CustomModeLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class CustomModeLayer : public Layer{
public:
	CustomModeLayer();
	~CustomModeLayer();

	virtual bool init();

	CREATE_FUNC(CustomModeLayer);
};

#endif