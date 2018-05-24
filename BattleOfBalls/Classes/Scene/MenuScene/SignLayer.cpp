#include "SignLayer.h"
#include "Tools/MaskLayer/MaskLayer.h"

enum SignTag
{
	TAG_7DAY,
	TAG_14DAY,
	TAG_28DAY,
	TAG_DAY
};
enum SignZOrder
{
	SIGN_MASKLAYER_Z,
	SIGN_BAKCGROUND_Z,
	SIGN_MENU_Z,
	SIGN_MAX_Z
};

SignLayer::SignLayer()
{
	_selectedDay = -1;
}

SignLayer::~SignLayer()
{

}

bool SignLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto maskLayer = MaskLayer::create();
	this->addChild(maskLayer, SIGN_MASKLAYER_Z);

	auto background = Sprite::create("menuScene/signLayer/sign_background.png");
	background->setPosition(400, 225);
	this->addChild(background,SIGN_BAKCGROUND_Z);

	auto closeItem = MenuItemImage::create(
		"menuScene/signLayer/sign_close.png",
		"menuScene/signLayer/sign_close.png",
		CC_CALLBACK_1(SignLayer::menuCloseCallback, this));
	closeItem->setPosition(684, 424);

	auto dayPrizeItem1 = MenuItemImage::create(
		"menuScene/signLayer/sign_dayPrize_btn.png",
		"menuScene/signLayer/sign_dayPrize_btn.png",
		CC_CALLBACK_1(SignLayer::menuDayPrizeCallback, this));
	dayPrizeItem1->setPosition(549, 122);

	auto dayPrizeItem2 = MenuItemImage::create(
		"menuScene/signLayer/sign_dayPrize_btn.png",
		"menuScene/signLayer/sign_dayPrize_btn.png",
		CC_CALLBACK_1(SignLayer::menuDayPrizeCallback, this));
	dayPrizeItem2->setPosition(626, 122);

	auto signItem = MenuItemImage::create(
		"menuScene/signLayer/sign_btn0.png",
		"menuScene/signLayer/sign_btn0.png",
		CC_CALLBACK_1(SignLayer::menuSignCallback, this));
	signItem->setPosition(400, 40);

	_menu = Menu::create(closeItem, dayPrizeItem1, dayPrizeItem2, signItem, NULL);
	_menu->setPosition(Vec2::ZERO);
	this->addChild(_menu,SIGN_MENU_Z);

	createDayItem(_menu);

	return true;
}

void SignLayer::menuSignCallback(Ref * pSender)
{

}

void SignLayer::menuDayCallback(Ref * pSender)
{
	auto item = (MenuItemImage *)pSender;
	int tag = item->getTag();
	if (_selectedDay == tag)
	{
		return;
	}
	else
	{
		if (_selectedDay != -1)
		{
			auto selectedItem = (MenuItemImage *)_menu->getChildByTag(_selectedDay);
			selectedItem->setNormalImage(Sprite::create("menuScene/signLayer/sign_day_btn0.png"));
			selectedItem->setSelectedImage(Sprite::create("menuScene/signLayer/sign_day_btn0.png"));
		}		

		item->setNormalImage(Sprite::create("menuScene/signLayer/sign_day_btn1.png"));
		item->setSelectedImage(Sprite::create("menuScene/signLayer/sign_day_btn1.png"));
		_selectedDay = tag;
	}
}

void SignLayer::menuCloseCallback(Ref * pSender)
{
	this->removeFromParentAndCleanup(true);
}

void SignLayer::menuDayPrizeCallback(Ref * pSender)
{

}

void SignLayer::menuMonthPrizeCallback(Ref * pSender)
{

}

void SignLayer::menuNextCallback(Ref * pSender)
{

}

void SignLayer::menuPreviousCallback(Ref * pSender)
{

}

void SignLayer::createDayItem(Menu * menu)
{
	int dayCount = 30;
	int count = 0;

	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 7; j++)
		{
			if (++count > dayCount)
			{
				return;
			}

			auto item = MenuItemImage::create(
				"menuScene/signLayer/sign_day_btn0.png",
				"menuScene/signLayer/sign_day_btn0.png",
				CC_CALLBACK_1(SignLayer::menuDayCallback, this));
			item->setPosition(140 + j * 53, 343 - i * 53);
			std::string str = StringUtils::format("%d", count);
			auto label = Label::createWithTTF(str.c_str(), "fonts/arial.ttf", 14);
			label->setPosition(5, 40);
			label->setColor(Color3B(133, 156, 114));
			label->setAnchorPoint(Vec2(0, 0.5));
			item->addChild(label,1);
			item->setTag(TAG_DAY + count);
			menu->addChild(item);
			
		}
	}
}
