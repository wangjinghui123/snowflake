#include "RelationScene.h"
#include "FocusLayer.h"
#include "FansLayer.h"
#include "RivalLayer.h"
#include "../MenuScene/FindFriendLayer.h"
#include "../SceneManager.h"

enum RelationZOrder
{
	RELATION_BACKGROUND_Z,
	RELATION_MENU_Z,
	RELATION_LAYER_Z
};

Scene * RelationScene::createScene()
{
	auto scene = Scene::create();

	auto layer = RelationScene::create();

	scene->addChild(layer);

	return scene;
}

bool RelationScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_focusItem = CheckBox::create("relationScene/relation_focus_btn0.png",
		"relationScene/relation_focus_btn1.png");
	_focusItem->setPosition(Vec2(214,416));
	_focusItem->addEventListener(CC_CALLBACK_2(RelationScene::menuFocusCallback, this));
	_focusItem->setZoomScale(0);
	_focusItem->setSelected(true);
	_focusItem->setName("FocusItem");
	this->addChild(_focusItem, RELATION_MENU_Z);

	_fansItem = CheckBox::create("relationScene/relation_fans_btn0.png",
		"relationScene/relation_fans_btn1.png");
	_fansItem->setPosition(Vec2(385, 416));
	_fansItem->addEventListener(CC_CALLBACK_2(RelationScene::menuFansCallback, this));
	_fansItem->setZoomScale(0);
	_fansItem->setName("FansItem");
	this->addChild(_fansItem, RELATION_MENU_Z);

	_rivalItem = CheckBox::create("relationScene/relation_rival_btn0.png",
		"relationScene/relation_rival_btn1.png");
	_rivalItem->setPosition(Vec2(535, 416));
	_rivalItem->addEventListener(CC_CALLBACK_2(RelationScene::menuRivalCallback, this));
	_rivalItem->setZoomScale(0);
	_rivalItem->setName("RivalItem");
	this->addChild(_rivalItem, RELATION_MENU_Z);

	auto findFriendItem = MenuItemImage::create(
		"relationScene/relation_findFriend_btn0.png",
		"relationScene/relation_findFriend_btn1.png",
		CC_CALLBACK_1(RelationScene::menuFindFriendCallback, this));
	findFriendItem->setPosition(723,412);

	auto returnItem = MenuItemImage::create(
		"public/return_btn0.png",
		"public/return_btn1.png",
		CC_CALLBACK_1(RelationScene::menuReturnCallback, this));
	returnItem->setPosition(751,22);

	auto menu = Menu::create(findFriendItem, returnItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, RELATION_MENU_Z);

	auto background = Sprite::create("public/background.png");
	background->setPosition(400, 225);
	this->addChild(background, RELATION_BACKGROUND_Z);

	auto line1 = Sprite::create("relationScene/relation_line.png");
	line1->setPosition(303,416);
	this->addChild(line1, RELATION_BACKGROUND_Z);

	auto line2 = Sprite::create("relationScene/relation_line.png");
	line2->setPosition(466,416);
	this->addChild(line2, RELATION_BACKGROUND_Z);

	_focusLayer = FocusLayer::create();
	this->addChild(_focusLayer, RELATION_LAYER_Z);

	_fansLayer = FansLayer::create();
	_fansLayer->setVisible(false);
	this->addChild(_fansLayer, RELATION_LAYER_Z);

	_rivalLayer = RivalLayer::create();
	_rivalLayer->setVisible(false);
	this->addChild(_rivalLayer, RELATION_LAYER_Z);

	return true;
}

void RelationScene::menuFocusCallback(Ref * pSender, CheckBox::EventType type)
{

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_fansItem->setSelected(false);
		_rivalItem->setSelected(false);

		_focusLayer->setVisible(true);
		_fansLayer->setVisible(false);
		_rivalLayer->setVisible(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_focusItem->setSelected(true);
		break;
	default:
		break;
	}
}

void RelationScene::menuFansCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_focusItem->setSelected(false);
		_rivalItem->setSelected(false);

		_focusLayer->setVisible(false);
		_fansLayer->setVisible(true);
		_rivalLayer->setVisible(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		_fansItem->setSelected(true);
		break;
	default:
		break;
	}
}

void RelationScene::menuRivalCallback(Ref * pSender, CheckBox::EventType type)
{
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		_focusItem->setSelected(false);
		_fansItem->setSelected(false);

		_focusLayer->setVisible(false);
		_fansLayer->setVisible(false);
		_rivalLayer->setVisible(true);
		break;
	case CheckBox::EventType::UNSELECTED:
		_rivalItem->setSelected(true);
		break;
	default:
		break;
	}
}

void RelationScene::menuFindFriendCallback(Ref * pSender)
{
	auto layer = FindFriendLayer::create();
	this->addChild(layer, RELATION_LAYER_Z);
}

void RelationScene::menuReturnCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_MenuScene);
}