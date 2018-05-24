#ifndef _MemberLayer_H_
#define _MemberLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class MemberLayer : public Layer{
public:
	MemberLayer();
	~MemberLayer();

	virtual bool init();

	CREATE_FUNC(MemberLayer);

	void menuMonthCallback(Ref * pSender);
	void menuYearCallback(Ref * pSender);
};

#endif