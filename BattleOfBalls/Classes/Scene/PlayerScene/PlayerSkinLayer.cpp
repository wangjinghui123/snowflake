#include "PlayerSkinLayer.h"

enum PlayerSkinZOrder
{
	PLAYER_SKIN_BACKGROUND_Z,
	PLAYER_SKIN_SPRITE_Z,
	PLAYER_SKIN_NODE_Z,
	PLAYER_SKIN_MENU_Z
};

PlayerSkinLayer::PlayerSkinLayer()
{

}

PlayerSkinLayer::~PlayerSkinLayer()
{

}

bool PlayerSkinLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_skinItem = CheckBox::create("playerScene/skin/player_skin_pifu_btn0.png",
		"playerScene/skin/player_skin_pifu_btn1.png");
	_skinItem->setPosition(Vec2(141,389));
	_skinItem->setZoomScale(0);
	_skinItem->setSelected(true);
	_skinItem->addEventListener(CC_CALLBACK_2(PlayerSkinLayer::menuSkinCallback, this));
	this->addChild(_skinItem, PLAYER_SKIN_MENU_Z);

	_vestmentItem = CheckBox::create("playerScene/skin/player_skin_shengyi_btn0.png",
		"playerScene/skin/player_skin_shengyi_btn1.png");
	_vestmentItem->setPosition(Vec2(250, 389));
	_vestmentItem->setZoomScale(0);
	_vestmentItem->addEventListener(CC_CALLBACK_2(PlayerSkinLayer::menuVestmentCallback, this));
	this->addChild(_vestmentItem, PLAYER_SKIN_MENU_Z);

	auto background = Sprite::create("playerScene/skin/player_skin_background.png");
	background->setPosition(426,220);
	this->addChild(background, PLAYER_SKIN_SPRITE_Z);

	auto title = Sprite::create("playerScene/skin/player_skin_item0.png");
	title->setPosition(426, 349);
	this->addChild(title, PLAYER_SKIN_SPRITE_Z);

	return true;
}

void PlayerSkinLayer::menuSkinCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_vestmentItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_skinItem->setSelected(true);
		break;
	default:
		break;
	}
}

void PlayerSkinLayer::menuVestmentCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_skinItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_vestmentItem->setSelected(false);
		break;
	default:
		break;
	}
}