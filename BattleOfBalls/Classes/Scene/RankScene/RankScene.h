#ifndef _RankScene_H_
#define _RankScene_H_

#include "cocos2d.h"

USING_NS_CC;

class RankScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(RankScene);
};
#endif