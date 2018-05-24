#include "HomeLayer.h"

enum HomeZOrder
{
	HOME_BACKGROUND_Z,
	HOME_SPRITE_Z,
	HOME_MENU_Z,
	HOME_LAYER_Z
};

HomeLayer::HomeLayer()
{

}

HomeLayer::~HomeLayer()
{

}

bool HomeLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto likeItem = MenuItemImage::create(
		"playerScene/home/player_home_xihuan_btn0.png",
		"playerScene/home/player_home_xihuan_btn1.png",
		CC_CALLBACK_1(HomeLayer::menuLikeCallback, this));
	likeItem->setPosition(424,59);

	auto menu = Menu::create(likeItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, HOME_MENU_Z);

	auto background = Sprite::create("playerScene/home/player_home_background.png");
	background->setPosition(426, 221);
	this->addChild(background);

	return true;
}

void HomeLayer::menuLikeCallback(Ref * pSender)
{
	
}