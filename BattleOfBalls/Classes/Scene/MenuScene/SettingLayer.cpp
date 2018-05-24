#include "SettingLayer.h"
#include "Tools/MaskLayer/MaskLayer.h"
#include "GameSettingLayer.h"
#include "AccountSettingLayer.h"

enum SettingZOrder
{
	SETTING_MASKLAYER_Z,
	SETTING_BACKGROUND_Z,
	SETTING_MENU_Z,
	SETTING_LAYER_Z
};

SettingLayer::SettingLayer()
{
	_currentLayer = nullptr;
}

SettingLayer::~SettingLayer()
{

}

bool SettingLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto maskLayer = MaskLayer::create();
	this->addChild(maskLayer, SETTING_MASKLAYER_Z);

	_accountItem1 = MenuItemImage::create(
		"menuScene/settingLayer/setting_account_btn0.png",
		"menuScene/settingLayer/setting_account_btn0.png",
		CC_CALLBACK_1(SettingLayer::menuAccountCallback, this));
	_accountItem1->setPosition(121,400);
	_accountItem1->setVisible(false);

	_accountItem2 = MenuItemImage::create(
		"menuScene/settingLayer/setting_account_btn1.png",
		"menuScene/settingLayer/setting_account_btn1.png",
		CC_CALLBACK_1(SettingLayer::menuAccountCallback, this));
	_accountItem2->setPosition(121,400);

	_gameItem1 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_btn0.png",
		"menuScene/settingLayer/setting_game_btn0.png",
		CC_CALLBACK_1(SettingLayer::menuGameCallback, this));
	_gameItem1->setPosition(248,400);

	_gameItem2 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_btn1.png",
		"menuScene/settingLayer/setting_game_btn1.png",
		CC_CALLBACK_1(SettingLayer::menuGameCallback, this));
	_gameItem2->setPosition(248,400);
	_gameItem2->setVisible(false);

	auto closeItem = MenuItemImage::create(
		"menuScene/settingLayer/setting_close.png",
		"menuScene/settingLayer/setting_close.png",
		CC_CALLBACK_1(SettingLayer::menuCloseCallback, this));
	closeItem->setPosition(721,400);

	auto menu = Menu::create(_accountItem1, _accountItem2, _gameItem1, _gameItem2, closeItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu,SETTING_MENU_Z);

	auto background = Sprite::create("menuScene/settingLayer/setting_background.png");
	background->setPosition(400, 225);
	this->addChild(background, SETTING_BACKGROUND_Z);

	_currentLayer = AccountSettingLayer::create();
	this->addChild(_currentLayer, SETTING_LAYER_Z);

	return true;
}

void SettingLayer::menuAccountCallback(Ref * pSender)
{
	_accountItem1->setVisible(false);
	_accountItem2->setVisible(true);
	_gameItem1->setVisible(true);
	_gameItem2->setVisible(false);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = AccountSettingLayer::create();
	this->addChild(_currentLayer, SETTING_LAYER_Z);
}

void SettingLayer::menuGameCallback(Ref * pSender)
{
	_accountItem1->setVisible(true);
	_accountItem2->setVisible(false);
	_gameItem1->setVisible(false);
	_gameItem2->setVisible(true);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = GameSettingLayer::create();
	this->addChild(_currentLayer, SETTING_LAYER_Z);
}

void SettingLayer::menuCloseCallback(Ref * pSender)
{
	this->removeFromParentAndCleanup(true);
}

