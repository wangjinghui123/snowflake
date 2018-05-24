#ifndef _PlayerDataLayer_H_
#define _PlayerDataLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class PlayerDataLayer : public Layer{
public:
	PlayerDataLayer();
	~PlayerDataLayer();

	virtual bool init();

	CREATE_FUNC(PlayerDataLayer);

	void menuDataCallback(Ref * pSender, CheckBox::EventType type);
	void menuAchievementCallback(Ref * pSender, CheckBox::EventType type);
	void menuHonorCallback(Ref * pSender, CheckBox::EventType type);
	void menuRecordCallback(Ref * pSender, CheckBox::EventType type);

	void createDataNode();
	void createAchievementNode();
	void createHonorNode();
	void createRecordNode();
private:
	CheckBox * _dataItem;
	CheckBox * _achievementItem;
	CheckBox * _honorItem;
	CheckBox * _recordItem;

	Node * _dataNode;
	Node * _achievementNode;
	Node * _honorNode;
	Node * _recordNode;
};

#endif