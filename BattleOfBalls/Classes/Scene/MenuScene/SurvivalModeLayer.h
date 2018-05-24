#ifndef _SurvivalModeLayer_H_
#define _SurvivalModeLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class SurvivalModeLayer : public Layer{
public:
	SurvivalModeLayer();
	~SurvivalModeLayer();

	virtual bool init();

	CREATE_FUNC(SurvivalModeLayer);
};

#endif