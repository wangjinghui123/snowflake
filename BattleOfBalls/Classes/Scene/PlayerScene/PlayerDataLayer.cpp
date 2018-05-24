#include "PlayerDataLayer.h"

enum PlayerDATAZOrder
{
	PLAYER_DATA_BACKGROUND_Z,
	PLAYER_DATA_SPRITE_Z,
	PLAYER_DATA_NODE_Z,
	PLAYER_DATA_MENU_Z
};

PlayerDataLayer::PlayerDataLayer()
{

}

PlayerDataLayer::~PlayerDataLayer()
{

}

bool PlayerDataLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_dataItem = CheckBox::create("playerScene/game/player_game_data_btn0.png",
		"playerScene/game/player_game_data_btn1.png");
	_dataItem->setPosition(Vec2(426,394));
	_dataItem->setZoomScale(0);
	_dataItem->setSelected(true);
	_dataItem->addEventListener(CC_CALLBACK_2(PlayerDataLayer::menuDataCallback, this));
	this->addChild(_dataItem, PLAYER_DATA_MENU_Z);

	_achievementItem = CheckBox::create("playerScene/game/player_game_achievement_btn0.png",
		"playerScene/game/player_game_achievement_btn1.png");
	_achievementItem->setPosition(Vec2(426, 117));
	_achievementItem->setZoomScale(0);
	_achievementItem->addEventListener(CC_CALLBACK_2(PlayerDataLayer::menuAchievementCallback, this));
	this->addChild(_achievementItem, PLAYER_DATA_MENU_Z);

	_honorItem = CheckBox::create("playerScene/game/player_game_honor_btn0.png",
		"playerScene/game/player_game_honor_btn1.png");
	_honorItem->setPosition(Vec2(426, 83));
	_honorItem->setZoomScale(0);
	_honorItem->addEventListener(CC_CALLBACK_2(PlayerDataLayer::menuHonorCallback, this));
	this->addChild(_honorItem, PLAYER_DATA_MENU_Z);

	_recordItem = CheckBox::create("playerScene/game/player_game_record_btn0.png",
		"playerScene/game/player_game_record_btn1.png");
	_recordItem->setPosition(Vec2(426, 49));
	_recordItem->setZoomScale(0);
	_recordItem->addEventListener(CC_CALLBACK_2(PlayerDataLayer::menuRecordCallback, this));
	this->addChild(_recordItem, PLAYER_DATA_MENU_Z);

	createDataNode();
	createAchievementNode();
	createHonorNode();
	createRecordNode();

	return true;
}

void PlayerDataLayer::menuDataCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:		
		_achievementItem->setSelected(false);
		_honorItem->setSelected(false);
		_recordItem->setSelected(false);

		_achievementItem->setPosition(Vec2(426, 117));
		_honorItem->setPosition(Vec2(426, 83));
		_recordItem->setPosition(Vec2(426, 49));

		_dataNode->setVisible(true);
		_achievementNode->setVisible(false);
		_honorNode->setVisible(false);
		_recordNode->setVisible(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_dataItem->setSelected(true);
		break;
	default:
		break;
	}
}

void PlayerDataLayer::menuAchievementCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_dataItem->setSelected(false);		
		_honorItem->setSelected(false);
		_recordItem->setSelected(false);

		_achievementItem->setPosition(Vec2(426, 360));
		_honorItem->setPosition(Vec2(426, 83));
		_recordItem->setPosition(Vec2(426, 49));

		_dataNode->setVisible(false);
		_achievementNode->setVisible(true);
		_honorNode->setVisible(false);
		_recordNode->setVisible(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_achievementItem->setSelected(true);
		break;
	default:
		break;
	}
}

void PlayerDataLayer::menuHonorCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_dataItem->setSelected(false);
		_achievementItem->setSelected(false);		
		_recordItem->setSelected(false);

		_achievementItem->setPosition(Vec2(426, 360));
		_honorItem->setPosition(Vec2(426, 326));
		_recordItem->setPosition(Vec2(426, 49));

		_dataNode->setVisible(false);
		_achievementNode->setVisible(false);
		_honorNode->setVisible(true);
		_recordNode->setVisible(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_honorItem->setSelected(true);
		break;
	default:
		break;
	}
}

void PlayerDataLayer::menuRecordCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_dataItem->setSelected(false);
		_achievementItem->setSelected(false);
		_honorItem->setSelected(false);	

		_achievementItem->setPosition(Vec2(426, 360));
		_honorItem->setPosition(Vec2(426, 326));
		_recordItem->setPosition(Vec2(426, 292));

		_dataNode->setVisible(false);
		_achievementNode->setVisible(false);
		_honorNode->setVisible(false);
		_recordNode->setVisible(true);
		break;
	case CheckBox::EventType::UNSELECTED:
		_recordItem->setSelected(true);
		break;
	default:
		break;
	}
}

void PlayerDataLayer::createDataNode()
{
	_dataNode = Node::create();
	this->addChild(_dataNode, PLAYER_DATA_NODE_Z);

	auto background = Sprite::create("playerScene/game/player_game_dataBackground.png");
	background->setPosition(425, 256);
	_dataNode->addChild(background);

	for (int i = 0; i < 9; i++)
	{
		std::string path;
		if (i % 2 == 0)
		{
			path = "playerScene/game/player_game_item0.png";
		}
		else
		{
			path = "playerScene/game/player_game_item1.png";		
		}

		auto item = Sprite::create(path.c_str());
		item->setPosition(586, 360 - i * 26);
		_dataNode->addChild(item);
	}
}

void PlayerDataLayer::createAchievementNode()
{
	_achievementNode = Node::create();
	this->addChild(_achievementNode, PLAYER_DATA_NODE_Z);
}

void PlayerDataLayer::createHonorNode()
{
	_honorNode = Node::create();
	this->addChild(_honorNode, PLAYER_DATA_NODE_Z);
}

void PlayerDataLayer::createRecordNode()
{
	_recordNode = Node::create();
	this->addChild(_recordNode, PLAYER_DATA_NODE_Z);
}