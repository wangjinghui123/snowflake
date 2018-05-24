#include "PictureLayer.h"

enum PictureZOrder
{
	PICTURE_BACKGROUND_Z,
	PICTURE_SPRITE_Z,
	PICTURE_MENU_Z
};

PictureLayer::PictureLayer()
{

}

PictureLayer::~PictureLayer()
{

}

bool PictureLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_pictureView = ScrollView::create();
	_pictureView->setContentSize(Size(576, 377));
	_pictureView->setInnerContainerSize(Size(576, 400));
	_pictureView->setDirection(ScrollView::Direction::VERTICAL);
	_pictureView->setScrollBarEnabled(false);
	_pictureView->setBounceEnabled(true);
	_pictureView->setPosition(Vec2(85, 33));
	this->addChild(_pictureView, PICTURE_SPRITE_Z);

	auto picture = Sprite::create("playerScene/picture/picture.jpg");
	picture->setPosition(288, 200);
	_pictureView->addChild(picture);

	auto addItem = MenuItemImage::create(
		"playerScene/picture/player_picture_add.png",
		"playerScene/picture/player_picture_add.png",
		CC_CALLBACK_1(PictureLayer::menuAddCallback, this));
	addItem->setPosition(716,342);

	auto menu = Menu::create(addItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, PICTURE_MENU_Z);

	auto background = Sprite::create("playerScene/picture/player_picture_background.png");
	background->setPosition(373, 221);
	this->addChild(background, PICTURE_BACKGROUND_Z);

	return true;
}

void PictureLayer::menuAddCallback(Ref * pSender)
{

}

void PictureLayer::menuDeleteCallback(Ref * pSender)
{

}