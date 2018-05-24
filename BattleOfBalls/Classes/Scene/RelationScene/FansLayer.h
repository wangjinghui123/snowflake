#ifndef _FansLayer_H_
#define _FansLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class FansLayer : public Layer{
public:
	FansLayer();
	~FansLayer();

	virtual bool init();

	CREATE_FUNC(FansLayer);
};

#endif