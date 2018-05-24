#ifndef _WatchGameScene_H_
#define _WatchGameScene_H_

#include "cocos2d.h"

USING_NS_CC;

class WatchGameScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(WatchGameScene);
};
#endif