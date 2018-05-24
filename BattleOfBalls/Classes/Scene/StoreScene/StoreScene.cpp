#include "StoreScene.h"
#include "../SceneManager.h"
#include "TreasureLayer.h"
#include "SkinLayer.h"
#include "VestmentLayer.h"
#include "MemberLayer.h"

enum StoreZOrder
{
	STORE_BACKGROUND_Z,
	STORE_SPRITE_Z,
	STORE_LAYER_Z,
	STORE_MENU_Z
};

Scene * StoreScene::createScene()
{
	auto scene = Scene::create();

	auto layer = StoreScene::create();

	scene->addChild(layer);

	return scene;
}

bool StoreScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_treasureItem1 = MenuItemImage::create(
		"storeScene/store_treasure_btn0.png",
		"storeScene/store_treasure_btn0.png",
		CC_CALLBACK_1(StoreScene::menuTreasureCallback, this));
	_treasureItem1->setPosition(42,431);
	_treasureItem1->setVisible(false);

	_treasureItem2 = MenuItemImage::create(
		"storeScene/store_treasure_btn1.png",
		"storeScene/store_treasure_btn1.png",
		CC_CALLBACK_1(StoreScene::menuTreasureCallback, this));
	_treasureItem2->setPosition(42,431);

	_skinItem1 = MenuItemImage::create(
		"storeScene/store_skin_btn0.png",
		"storeScene/store_skin_btn0.png",
		CC_CALLBACK_1(StoreScene::menuSkinCallback, this));
	_skinItem1->setPosition(122,431);

	_skinItem2 = MenuItemImage::create(
		"storeScene/store_skin_btn1.png",
		"storeScene/store_skin_btn1.png",
		CC_CALLBACK_1(StoreScene::menuSkinCallback, this));
	_skinItem2->setPosition(122,431);
	_skinItem2->setVisible(false);

	_vestmentItem1 = MenuItemImage::create(
		"storeScene/store_vestment_btn0.png",
		"storeScene/store_vestment_btn0.png",
		CC_CALLBACK_1(StoreScene::menuVestmentCallback, this));
	_vestmentItem1->setPosition(202,431);

	_vestmentItem2 = MenuItemImage::create(
		"storeScene/store_vestment_btn1.png",
		"storeScene/store_vestment_btn1.png",
		CC_CALLBACK_1(StoreScene::menuVestmentCallback, this));
	_vestmentItem2->setPosition(202,431);
	_vestmentItem2->setVisible(false);

	_memberItem1 = MenuItemImage::create(
		"storeScene/store_member_btn0.png",
		"storeScene/store_member_btn0.png",
		CC_CALLBACK_1(StoreScene::menuMemberCallback, this));
	_memberItem1->setPosition(278,431);

	_memberItem2 = MenuItemImage::create(
		"storeScene/store_member_btn1.png",
		"storeScene/store_member_btn1.png",
		CC_CALLBACK_1(StoreScene::menuMemberCallback, this));
	_memberItem2->setPosition(278,431);
	_memberItem2->setVisible(false);

	auto returnItem = MenuItemImage::create(
		"storeScene/store_return_btn0.png",
		"storeScene/store_return_btn1.png",
		CC_CALLBACK_1(StoreScene::menuReturnCallback, this));
	returnItem->setPosition(745, 27);

	auto magicItem = MenuItemImage::create(
		"storeScene/store_top.png",
		"storeScene/store_top.png",
		CC_CALLBACK_1(StoreScene::menuMagicCallback, this));
	magicItem->setPosition(400, 429);

	auto beanItem = MenuItemImage::create(
		"storeScene/store_bean_btn0.png",
		"storeScene/store_bean_btn0.png",
		CC_CALLBACK_1(StoreScene::menuBeanCallback, this));
	beanItem->setPosition(506, 427);

	auto lollyItem = MenuItemImage::create(
		"storeScene/store_lolly_btn0.png",
		"storeScene/store_lolly_btn1.png",
		CC_CALLBACK_1(StoreScene::menuLollyCallback, this));
	lollyItem->setPosition(608, 427);

	auto mushroomItem = MenuItemImage::create(
		"storeScene/store_mushroom_btn0.png",
		"storeScene/store_mushroom_btn1.png",
		CC_CALLBACK_1(StoreScene::menuMushroomCallback, this));
	mushroomItem->setPosition(729, 427);

	auto menu = Menu::create(_treasureItem1, _treasureItem2, _skinItem1, _skinItem2, _vestmentItem1, _vestmentItem2,
		_memberItem1, _memberItem2, beanItem, lollyItem, mushroomItem, magicItem, returnItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, STORE_MENU_Z);

	auto background = Sprite::create("public/background.png");
	background->setPosition(400, 225);
	this->addChild(background, STORE_BACKGROUND_Z);

	auto topBackground = Sprite::create("storeScene/store_top.png");
	topBackground->setPosition(400, 429);
	this->addChild(topBackground, STORE_SPRITE_Z);


	_currentLayer = TreasureLayer::create();
	this->addChild(_currentLayer, STORE_LAYER_Z);

	return true;
}

void StoreScene::menuTreasureCallback(Ref * pSender)
{
	_treasureItem1->setVisible(false);
	_treasureItem2->setVisible(true);
	_skinItem1->setVisible(true);
	_skinItem2->setVisible(false);
	_vestmentItem1->setVisible(true);
	_vestmentItem2->setVisible(false);
	_memberItem1->setVisible(true);
	_memberItem2->setVisible(false);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = TreasureLayer::create();
	this->addChild(_currentLayer, STORE_LAYER_Z);
}

void StoreScene::menuSkinCallback(Ref * pSender)
{
	_treasureItem1->setVisible(true);
	_treasureItem2->setVisible(false);
	_skinItem1->setVisible(false);
	_skinItem2->setVisible(true);
	_vestmentItem1->setVisible(true);
	_vestmentItem2->setVisible(false);
	_memberItem1->setVisible(true);
	_memberItem2->setVisible(false);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = SkinLayer::create();
	this->addChild(_currentLayer, STORE_LAYER_Z);
}

void StoreScene::menuVestmentCallback(Ref * pSender)
{
	_treasureItem1->setVisible(true);
	_treasureItem2->setVisible(false);
	_skinItem1->setVisible(true);
	_skinItem2->setVisible(false);
	_vestmentItem1->setVisible(false);
	_vestmentItem2->setVisible(true);
	_memberItem1->setVisible(true);
	_memberItem2->setVisible(false);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = VestmentLayer::create();
	this->addChild(_currentLayer, STORE_LAYER_Z);
}

void StoreScene::menuMemberCallback(Ref * pSender)
{
	_treasureItem1->setVisible(true);
	_treasureItem2->setVisible(false);
	_skinItem1->setVisible(true);
	_skinItem2->setVisible(false);
	_vestmentItem1->setVisible(true);
	_vestmentItem2->setVisible(false);
	_memberItem1->setVisible(false);
	_memberItem2->setVisible(true);

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}

	_currentLayer = MemberLayer::create();
	this->addChild(_currentLayer, STORE_LAYER_Z);
}

void StoreScene::menuBeanCallback(Ref * pSender)
{

}

void StoreScene::menuLollyCallback(Ref * pSender)
{

}

void StoreScene::menuMushroomCallback(Ref * pSender)
{

}

void StoreScene::menuReturnCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_MenuScene);
}

void StoreScene::menuMagicCallback(Ref * pSender)
{

}
