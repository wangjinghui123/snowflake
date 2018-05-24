#include "FindFriendLayer.h"
#include "Tools/MaskLayer/MaskLayer.h"

enum FindFriendTag
{
	TAG_FIND,
	TAG_NEARBY,
	TAG_INTERESTED
};

enum FindFriendZOrder
{
	FINDFRIEND_MASKLAYER_Z,
	FINDFRIEND_BACKGROUND_Z,
	FINDFRIEND_MENU_Z,
	FINDFRIEND_LAYER_Z
};

FindFriendLayer::FindFriendLayer()
{

}

FindFriendLayer::~FindFriendLayer()
{

}

bool FindFriendLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto maskLayer = MaskLayer::create();
	this->addChild(maskLayer, FINDFRIEND_MASKLAYER_Z);

	_findItem1 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top1_btn0.png",
		"menuScene/findFriendLayer/findFriend_top1_btn0.png",
		CC_CALLBACK_1(FindFriendLayer::menuFindCallback, this));
	_findItem1->setPosition(120, 402);
	_findItem1->setVisible(false);

	_findItem2 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top1_btn1.png",
		"menuScene/findFriendLayer/findFriend_top1_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuFindCallback, this));
	_findItem2->setPosition(120, 402);

	_nearbyItem1 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top2_btn0.png",
		"menuScene/findFriendLayer/findFriend_top2_btn0.png",
		CC_CALLBACK_1(FindFriendLayer::menuNearbyCallback, this));
	_nearbyItem1->setPosition(253, 402);

	_nearbyItem2 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top2_btn1.png",
		"menuScene/findFriendLayer/findFriend_top2_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuNearbyCallback, this));
	_nearbyItem2->setPosition(253, 402);
	_nearbyItem2->setVisible(false);

	_interestedItem1 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top3_btn0.png",
		"menuScene/findFriendLayer/findFriend_top3_btn0.png",
		CC_CALLBACK_1(FindFriendLayer::menuInterestedCallback, this));
	_interestedItem1->setPosition(408, 402);

	_interestedItem2 = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_top3_btn1.png",
		"menuScene/findFriendLayer/findFriend_top3_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuInterestedCallback, this));
	_interestedItem2->setPosition(408, 402);
	_interestedItem2->setVisible(false);

	auto closeItem = MenuItemImage::create(
		"menuScene/findFriendLayer/close.png",
		"menuScene/findFriendLayer/close.png",
		CC_CALLBACK_1(FindFriendLayer::menuCloseCallback, this));
	closeItem->setPosition(718, 402);

	auto menu = Menu::create(_findItem1, _findItem2, _nearbyItem1, _nearbyItem2, _interestedItem1, _interestedItem2, closeItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu,FINDFRIEND_MENU_Z);

	auto background = Sprite::create("menuScene/findFriendLayer/findFriend_background.png");
	background->setPosition(400, 225);
	this->addChild(background, FINDFRIEND_BACKGROUND_Z);

	createFindLayer();
	return true;
}

void FindFriendLayer::createFindLayer()
{
	auto searchItem = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_chazhao.png",
		"menuScene/findFriendLayer/findFriend_chazhao.png",
		CC_CALLBACK_1(FindFriendLayer::menuSearchCallback, this));
	searchItem->setPosition(263,339);

	auto yaoyiyaoItem = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_yaoyiyao_btn0.png",
		"menuScene/findFriendLayer/findFriend_yaoyiyao_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuYaoYiYaoCallback, this));
	yaoyiyaoItem->setPosition(263, 110);

	auto saoyisaoItem = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_saoyisao_btn0.png",
		"menuScene/findFriendLayer/findFriend_saoyisao_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuSaoYiSaoCallback, this));
	saoyisaoItem->setPosition(263, 270);

	auto leidaItem = MenuItemImage::create(
		"menuScene/findFriendLayer/findFriend_leida_btn0.png",
		"menuScene/findFriendLayer/findFriend_leida_btn1.png",
		CC_CALLBACK_1(FindFriendLayer::menuLeiDaCallback, this));
	leidaItem->setPosition(263, 190);

	auto menu = Menu::create(searchItem, yaoyiyaoItem, saoyisaoItem, leidaItem, NULL);
	menu->setPosition(Vec2::ZERO);
	
	auto layer = Layer::create();
	layer->addChild(menu);
	this->addChild(layer, FINDFRIEND_LAYER_Z,TAG_FIND);

	
}

void FindFriendLayer::menuFindCallback(Ref * pSender)
{
	_findItem1->setVisible(false);
	_findItem2->setVisible(true);
	_nearbyItem1->setVisible(true);
	_nearbyItem2->setVisible(false);
	_interestedItem1->setVisible(true);
	_interestedItem2->setVisible(false);

	auto findlayer = this->getChildByTag(TAG_FIND);
	auto nearbyLayer = this->getChildByTag(TAG_NEARBY);
	auto interestedLayer = this->getChildByTag(TAG_INTERESTED);

	if (findlayer != NULL)
	{
		return;
	}

	if (nearbyLayer != NULL)
	{
		this->removeChild(nearbyLayer);
	}

	if (interestedLayer != NULL)
	{
		this->removeChild(interestedLayer);
	}

	createFindLayer();
}

void FindFriendLayer::menuNearbyCallback(Ref * pSender)
{
	_findItem1->setVisible(true);
	_findItem2->setVisible(false);
	_nearbyItem1->setVisible(false);
	_nearbyItem2->setVisible(true);
	_interestedItem1->setVisible(true);
	_interestedItem2->setVisible(false);

	auto findlayer = this->getChildByTag(TAG_FIND);
	auto nearbyLayer = this->getChildByTag(TAG_NEARBY);
	auto interestedLayer = this->getChildByTag(TAG_INTERESTED);

	if (findlayer != NULL)
	{
		this->removeChild(findlayer);
	}

	if (nearbyLayer != NULL)
	{
		return;
	}

	if (interestedLayer != NULL)
	{
		this->removeChild(interestedLayer);
	}
}

void FindFriendLayer::menuInterestedCallback(Ref * pSender)
{
	_findItem1->setVisible(true);
	_findItem2->setVisible(false);
	_nearbyItem1->setVisible(true);
	_nearbyItem2->setVisible(false);
	_interestedItem1->setVisible(false);
	_interestedItem2->setVisible(true);

	auto findlayer = this->getChildByTag(TAG_FIND);
	auto nearbyLayer = this->getChildByTag(TAG_NEARBY);
	auto interestedLayer = this->getChildByTag(TAG_INTERESTED);

	if (findlayer != NULL)
	{
		this->removeChild(findlayer);
	}

	if (nearbyLayer != NULL)
	{
		this->removeChild(nearbyLayer);
	}

	if (interestedLayer != NULL)
	{
		return;
	}
}

void FindFriendLayer::menuCloseCallback(Ref * pSender)
{
	this->removeFromParentAndCleanup(true);
}

bool FindFriendLayer::onTextFieldAttachWithIME(TextFieldTTF*  pSender)
{
	return false;
}

bool FindFriendLayer::onTextFieldDetachWithIME(TextFieldTTF* pSender)
{
	return false;
}

bool FindFriendLayer::onTextFieldInsertText(TextFieldTTF*  pSender, const char * text, size_t nLen)
{
	return false;
}

bool FindFriendLayer::onTextFieldDeleteBackward(TextFieldTTF*  pSender, const char * delText, size_t nLen)
{
	return false;
}

void FindFriendLayer::menuSearchCallback(Ref * pSender)
{

}

void FindFriendLayer::menuSaoYiSaoCallback(Ref * pSender)
{

}

void FindFriendLayer::menuLeiDaCallback(Ref * pSender)
{

}

void FindFriendLayer::menuYaoYiYaoCallback(Ref * pSender)
{

}

void FindFriendLayer::createNearbyLayer()
{
	auto layer = Layer::create();
	this->addChild(layer, FINDFRIEND_LAYER_Z, TAG_NEARBY);
}

void FindFriendLayer::createInterestedLayer()
{
	auto layer = Layer::create();
	this->addChild(layer, FINDFRIEND_LAYER_Z, TAG_INTERESTED);
}