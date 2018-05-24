#ifndef _TeamModeLayer_H_
#define _TeamModeLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class TeamModeLayer : public Layer{
public:
	TeamModeLayer();
	~TeamModeLayer();

	virtual bool init();

	CREATE_FUNC(TeamModeLayer);
};

#endif