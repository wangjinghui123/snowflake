#ifndef _DataLayer_H_
#define _DataLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

/*数据图层，负责更新和显示玩家游戏时的数据*/

class DataLayer : public Layer{
public:
	DataLayer();
	~DataLayer();

	virtual bool init();

	CREATE_FUNC(DataLayer);

	virtual void onExit();

	void updateTime(EventCustom * event);
	void updateScore(EventCustom * event);
	void updateRank(EventCustom * event);

	void menuRankCallback(Ref * pSender);
private:
	Label * _timeLabel;
	Label * _scoreLabel;
	Vector<Label *> _rankList;
	Sprite * _rankSprite;
	Scale9Sprite * _rankBk1;
	Scale9Sprite * _rankBk2;
	Scale9Sprite * _rankBk3;
	int _timeTotal;
};

#endif