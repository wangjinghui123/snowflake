#ifndef _SignLayer_H_
#define _SignLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class SignLayer : public Layer{
public:
	SignLayer();
	~SignLayer();

	virtual bool init();

	CREATE_FUNC(SignLayer);

	void menuSignCallback(Ref * pSender);
	void menuDayCallback(Ref * pSender);
	void menuCloseCallback(Ref * pSender);
	void menuDayPrizeCallback(Ref * pSender);
	void menuMonthPrizeCallback(Ref * pSender);
	void menuNextCallback(Ref * pSender);
	void menuPreviousCallback(Ref * pSender);

	void createDayItem(Menu * menu);
private:
	int _selectedDay;
	Menu * _menu;
};

#endif