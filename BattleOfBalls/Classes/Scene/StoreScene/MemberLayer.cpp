#include "MemberLayer.h"

enum MemberZOrder
{
	MEMBER_BACKGROUND_Z,
	MEMBER_MENU_Z
};

MemberLayer::MemberLayer()
{

}

MemberLayer::~MemberLayer()
{

}

bool MemberLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto monthItem = MenuItemImage::create(
		"storeScene/member/store_member_month_btn0.png",
		"storeScene/member/store_member_month_btn1.png",
		CC_CALLBACK_1(MemberLayer::menuMonthCallback, this));
	monthItem->setPosition(287,63);

	auto yearItem = MenuItemImage::create(
		"storeScene/member/store_member_year_btn0.png",
		"storeScene/member/store_member_year_btn1.png",
		CC_CALLBACK_1(MemberLayer::menuYearCallback, this));
	yearItem->setPosition(496,63);

	auto menu = Menu::create(monthItem, yearItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, MEMBER_MENU_Z);

	auto background = Sprite::create("storeScene/member/store_member_background.png");
	background->setPosition(400, 247);
	this->addChild(background, MEMBER_BACKGROUND_Z);

	return true;
}

void MemberLayer::menuMonthCallback(Ref * pSender)
{

}

void MemberLayer::menuYearCallback(Ref * pSender)
{

}
