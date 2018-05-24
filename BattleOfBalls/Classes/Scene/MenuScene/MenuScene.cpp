#include "MenuScene.h"
#include "MenuLayer.h"
#include "ChatLayer.h"

enum MenuZOrder
{
	MENU_SCENE_BACKGROUND_Z,
	MENU_SCENE_LAYER_Z,
	MENU_SCENE_MENU_Z
};

Scene * MenuScene::createScene()
{
	auto scene = Scene::create();

	auto layer = MenuScene::create();

	scene->addChild(layer);
	
	return scene;
}

bool MenuScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_gameItem = MenuItemImage::create(
		"menuScene/menu_game_btn2.png",
		"menuScene/menu_game_btn3.png",
		CC_CALLBACK_1(MenuScene::menuGameCallback, this));
	_gameItem->setPosition(Vec2(17, 338));
	_gameFlag = true;

	_chatItem = MenuItemImage::create(
		"menuScene/menu_chat_btn0.png",
		"menuScene/menu_chat_btn1.png",
		CC_CALLBACK_1(MenuScene::menuChatCallback, this));
	_chatItem->setPosition(Vec2(17, 112));
	_chatFlag = false;

	auto menu = Menu::create(_gameItem, _chatItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, MENU_SCENE_MENU_Z);

	auto background = Sprite::create("public/background.png");
	background->setPosition(400, 225);
	this->addChild(background, MENU_SCENE_BACKGROUND_Z);

	_menuLayer = MenuLayer::create();
	this->addChild(_menuLayer, MENU_SCENE_LAYER_Z);


	_eventDispatcher->addCustomEventListener("Game Start", CC_CALLBACK_1(MenuScene::gameStartEvent, this));
	_gameStart = false;

	return true;
}

void MenuScene::menuGameCallback(Ref * pSender)
{
	if (_gameStart)
	{
		return;
	}
	
	log("press");

	if (!_gameFlag)
	{
		_menuLayer = MenuLayer::create();
		this->addChild(_menuLayer, MENU_SCENE_LAYER_Z);

		_chatLayer->removeFromParentAndCleanup(true);
		_chatLayer = NULL;

		_gameFlag = true;
		_gameItem->setNormalImage(Sprite::create("menuScene/menu_game_btn2.png"));
		_gameItem->setSelectedImage(Sprite::create("menuScene/menu_game_btn3.png"));
		_chatFlag = false;
		_chatItem->setNormalImage(Sprite::create("menuScene/menu_chat_btn0.png"));
		_chatItem->setSelectedImage(Sprite::create("menuScene/menu_chat_btn1.png"));
	
	}
	
}

void MenuScene::menuChatCallback(Ref * pSender)
{
	if (_gameStart)
	{
		return;
	}

	if (!_chatFlag)
	{
		_chatLayer = ChatLayer::create();
		this->addChild(_chatLayer, MENU_SCENE_LAYER_Z);

		_menuLayer->removeFromParentAndCleanup(true);
		_menuLayer = NULL;

		_chatFlag = true;
		_chatItem->setNormalImage(Sprite::create("menuScene/menu_chat_btn2.png"));
		_chatItem->setSelectedImage(Sprite::create("menuScene/menu_chat_btn3.png"));
		_gameFlag = false;
		_gameItem->setNormalImage(Sprite::create("menuScene/menu_game_btn0.png"));
		_gameItem->setSelectedImage(Sprite::create("menuScene/menu_game_btn1.png"));
	}
	
}

void MenuScene::gameStartEvent(EventCustom * event)
{
	_gameStart = true;
}

void MenuScene::onExit()
{
	_eventDispatcher->removeCustomEventListeners("Game Start");
	Layer::onExit();
	
}