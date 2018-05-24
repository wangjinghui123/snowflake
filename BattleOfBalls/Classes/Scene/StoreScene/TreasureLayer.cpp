#include "TreasureLayer.h"

enum TreasureZOrder
{
	TREASURE_BACKGROUND_Z,
	TREASURE_SPRITE_Z,
	TREASURE_LABEL_Z,
	TREASURE_MASK_Z,
	TREASURE_MENU_Z
};

TreasureLayer::TreasureLayer()
{

}

TreasureLayer::~TreasureLayer()
{
	_spriteGroup.clear();
}

bool TreasureLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto unlockItem = MenuItemImage::create(
		"storeScene/treasure/store_treasure_unlock_btn0.png",
		"storeScene/treasure/store_treasure_unlock_btn1.png",
		CC_CALLBACK_1(TreasureLayer::menuUnlockCallback, this));
	unlockItem->setPosition(400,69);

	auto menu = Menu::create(unlockItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, TREASURE_MENU_Z);

	auto background = Sprite::create("storeScene/treasure/store_treasure_mask.png");
	background->setPosition(400,235);
	this->addChild(background, TREASURE_BACKGROUND_Z);

	auto label1 = Sprite::create("storeScene/treasure/store_treasure_label1.png");
	label1->setPosition(400,367);
	this->addChild(label1, TREASURE_LABEL_Z);

	auto label2 = Sprite::create("storeScene/treasure/store_treasure_label2.png");
	label2->setPosition(400,301);
	this->addChild(label2, TREASURE_LABEL_Z);

	auto maskLeft = Sprite::create("storeScene/treasure/store_treasure_mask_left.png");
	maskLeft->setPosition(148,235);
	this->addChild(maskLeft, TREASURE_MASK_Z);

	auto maskRight = Sprite::create("storeScene/treasure/store_treasure_mask_right.png");
	maskRight->setPosition(651, 235);
	this->addChild(maskRight, TREASURE_MASK_Z);

	createSpriteGroup();

	this->scheduleUpdate();

	return true;
}

void TreasureLayer::menuUnlockCallback(Ref * pSender)
{

}

void TreasureLayer::update(float dt)
{
	for (auto sprite : _spriteGroup)
	{
		auto position = sprite->getPosition();
		auto newPosition = Vec2(position.x - 1, position.y);
		if (newPosition.x <= -100)
		{
			newPosition.x = 1180;
		}
		sprite->setPosition(newPosition);
	}
}

void TreasureLayer::createSpriteGroup()
{
	for (int i = 1; i <= 8; i++)
	{
		std::string path = StringUtils::format("public/keyword_%d.png", i);
		auto sprite = Sprite::create(path.c_str());
		sprite->setPosition(Vec2(-200 + i * 160, 211));
		this->addChild(sprite, TREASURE_SPRITE_Z);
		_spriteGroup.pushBack(sprite);
	}
}

void TreasureLayer::onExit()
{
	this->unscheduleAllCallbacks();
	Layer::onExit();
}