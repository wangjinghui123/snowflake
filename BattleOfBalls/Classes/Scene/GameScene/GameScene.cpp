#include "GameScene.h"
#include "GameLayer.h"
#include "DataLayer.h"
#include "../MenuScene/SettingLayer.h"

enum GameZOrder
{
	GAME_BACKGROUND_Z,
	GAME_LAYER_Z,
	GAME_DATA_Z,
	GAME_MENU_Z,
	GAME_SETTING_LAYER_Z
};

Scene * GameScene::createScene()
{
	auto scene = Scene::create();

	auto layer = GameScene::create();

	scene->addChild(layer);

	return scene;
}

bool GameScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto spitItem = CheckBox::create(
		"gameScene/spit_btn.png",
		"gameScene/spit_btn.png");
	spitItem->setPosition(Vec2(650,60));
	spitItem->setZoomScale(-0.1f);
	spitItem->addEventListener(CC_CALLBACK_2(GameScene::menuSpitCallback, this));
	this->addChild(spitItem, GAME_MENU_Z);

	auto divideItem = CheckBox::create(
		"gameScene/divide_btn.png",
		"gameScene/divide_btn.png");
	divideItem->setPosition(Vec2(720, 60));
	divideItem->setZoomScale(-0.1f);
	divideItem->addEventListener(CC_CALLBACK_2(GameScene::menuDivideCallback, this));
	this->addChild(divideItem, GAME_MENU_Z);

	auto settingItem = MenuItemImage::create(
		"gameScene/game_setting_btn.png",
		"gameScene/game_setting_btn.png",
		CC_CALLBACK_1(GameScene::menuSettingCallback, this));
	settingItem->setPosition(776, 433);

	auto menu = Menu::create(settingItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, GAME_MENU_Z);

	auto dataLayer = DataLayer::create();
	this->addChild(dataLayer, GAME_DATA_Z);

	auto gameLayer = GameLayer::create();
	this->addChild(gameLayer, GAME_LAYER_Z);

	return true;
}

void GameScene::menuSpitCallback(Ref * pSender, CheckBox::EventType type)
{
	_eventDispatcher->dispatchCustomEvent("Spit");
}

void GameScene::menuDivideCallback(Ref * pSender, CheckBox::EventType type)
{
	_eventDispatcher->dispatchCustomEvent("Divide");
}

void GameScene::menuSettingCallback(Ref * pSender)
{
	auto layer = SettingLayer::create();
	this->addChild(layer, GAME_SETTING_LAYER_Z);
}