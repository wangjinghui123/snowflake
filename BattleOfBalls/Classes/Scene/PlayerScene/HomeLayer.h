#ifndef _HomeLayer_H_
#define _HomeLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class HomeLayer : public Layer{
public:
	HomeLayer();
	~HomeLayer();

	virtual bool init();

	CREATE_FUNC(HomeLayer);

	void menuLikeCallback(Ref * pSender);
};

#endif