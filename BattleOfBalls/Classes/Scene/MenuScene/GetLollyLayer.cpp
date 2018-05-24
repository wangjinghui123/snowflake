#include "GetLollyLayer.h"
#include "Tools/MaskLayer/MaskLayer.h"

enum GetLollyTag
{
	TAG_LOLLY,
	TAG_WEIXIN
};

enum GetLollyZOrder
{
	GETLOLLY_MASKLAYER_Z,
	GETLOLLY_BACKGROUND_Z,
	GETLOLLY_MENU_Z,
	GETLOLLY_LAYER_Z
};

GetLollyLayer::GetLollyLayer()
{

}

GetLollyLayer::~GetLollyLayer()
{

}

bool GetLollyLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto maskLayer = MaskLayer::create();
	this->addChild(maskLayer, GETLOLLY_MASKLAYER_Z);

	_getLollyItem1 = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_lolly_btn0.png",
		"menuScene/getLollyLayer/getLolly_lolly_btn0.png",
		CC_CALLBACK_1(GetLollyLayer::menuGetLollyCallback, this));
	_getLollyItem1->setPosition(125,390);
	_getLollyItem1->setVisible(false);

	_getLollyItem2 = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_lolly_btn1.png",
		"menuScene/getLollyLayer/getLolly_lolly_btn1.png",
		CC_CALLBACK_1(GetLollyLayer::menuGetLollyCallback, this));
	_getLollyItem2->setPosition(125,390);

	_weiXinItem1 = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_weixin_btn0.png",
		"menuScene/getLollyLayer/getLolly_weixin_btn0.png",
		CC_CALLBACK_1(GetLollyLayer::menuWeiXinCallback, this));
	_weiXinItem1->setPosition(295,390);

	_weiXinItem2 = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_weixin_btn1.png",
		"menuScene/getLollyLayer/getLolly_weixin_btn1.png",
		CC_CALLBACK_1(GetLollyLayer::menuWeiXinCallback, this));
	_weiXinItem2->setPosition(295,390);
	_weiXinItem2->setVisible(false);

	auto closeItem = MenuItemImage::create(
		"menuScene/getLollyLayer/close.png",
		"menuScene/getLollyLayer/close.png",
		CC_CALLBACK_1(GetLollyLayer::menuCloseCallback, this));
	closeItem->setPosition(719,389);

	auto menu = Menu::create(_getLollyItem1, _getLollyItem2, _weiXinItem1, _weiXinItem2, closeItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu,GETLOLLY_MENU_Z);

	auto background = Sprite::create("menuScene/getLollyLayer/getLolly_background.png");
	background->setPosition(400, 225);
	this->addChild(background, GETLOLLY_BACKGROUND_Z);

	createLollyLayer();

	return true;
}

void GetLollyLayer::menuGetLollyCallback(Ref * pSender)
{
	_getLollyItem1->setVisible(false);
	_getLollyItem2->setVisible(true);
	_weiXinItem1->setVisible(true);
	_weiXinItem2->setVisible(false);

	auto lollyLayer = this->getChildByTag(TAG_LOLLY);
	auto weixinLayer = this->getChildByTag(TAG_WEIXIN);

	if (lollyLayer != NULL)
	{
		return;
	}

	if (weixinLayer != NULL)
	{
		this->removeChild(weixinLayer);
	}

	createLollyLayer();
}

void GetLollyLayer::menuWeiXinCallback(Ref * pSender)
{
	_getLollyItem1->setVisible(true);
	_getLollyItem2->setVisible(false);
	_weiXinItem1->setVisible(false);
	_weiXinItem2->setVisible(true);

	auto lollyLayer = this->getChildByTag(TAG_LOLLY);
	auto weixinLayer = this->getChildByTag(TAG_WEIXIN);

	if (lollyLayer != NULL)
	{
		this->removeChild(lollyLayer);
	}

	if (weixinLayer != NULL)
	{
		return;
	}

	createWeiXinLayer();
}

void GetLollyLayer::menuMiaoLingCallback(Ref * pSender)
{

}

void GetLollyLayer::menuSaveCallback(Ref * pSender)
{

}

void GetLollyLayer::menuCopyCallback(Ref * pSender)
{

}

void GetLollyLayer::menuLingQuCallback(Ref * pSender)
{

}

void GetLollyLayer::menuCloseCallback(Ref * pSender)
{
	this->removeFromParentAndCleanup(true);
}

void GetLollyLayer::createLollyLayer()
{
	auto miaolingItem = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_miaoling_btn0.png",
		"menuScene/getLollyLayer/getLolly_miaoling_btn1.png",
		CC_CALLBACK_1(GetLollyLayer::menuMiaoLingCallback, this));
	miaolingItem->setPosition(333,110);

	auto copyItem = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_lianjie_btn0.png",
		"menuScene/getLollyLayer/getLolly_lianjie_btn1.png",
		CC_CALLBACK_1(GetLollyLayer::menuCopyCallback, this));
	copyItem->setPosition(591,113);

	auto saveItem = MenuItemImage::create(
		"menuScene/getLollyLayer/getLolly_save_btn0.png",
		"menuScene/getLollyLayer/getLolly_save_btn1.png",
		CC_CALLBACK_1(GetLollyLayer::menuSaveCallback, this));
	saveItem->setPosition(591,67);

	auto menu = Menu::create(miaolingItem, copyItem, saveItem, NULL);
	menu->setPosition(Vec2::ZERO);

	auto layer = Layer::create();
	layer->addChild(menu);

	this->addChild(layer, GETLOLLY_LAYER_Z,TAG_LOLLY);
}

void GetLollyLayer::createWeiXinLayer()
{
	auto layer = Layer::create();
	this->addChild(layer, GETLOLLY_LAYER_Z, TAG_WEIXIN);
}
