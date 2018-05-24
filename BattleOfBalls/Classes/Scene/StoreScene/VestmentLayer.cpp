#include "VestmentLayer.h"

const int PAGEVIEW_THRESHOLD = 50;

enum VestmentZOrder
{
	VESTMENT_BACKGROUND_Z,
	VESTMENT_SPRITE_Z,
	VESTMENT_PAGEVIEW_Z,
	VESTMENT_MENU_Z,
	VESTMENT_LAYER_Z
	
};

VestmentLayer::VestmentLayer()
{

}

VestmentLayer::~VestmentLayer()
{

}

bool VestmentLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto yuanGuItem = CheckBox::create("storeScene/vestment/store_yuangu_btn0.png",
		"storeScene/vestment/store_yuangu_btn1.png");
	yuanGuItem->setPosition(Vec2(64, 388));
	yuanGuItem->setZoomScale(0);
	yuanGuItem->addEventListener(CC_CALLBACK_2(VestmentLayer::menuYuanGuCallback, this));
	yuanGuItem->setName("YuanGuItem");
	yuanGuItem->setSelected(true);
	this->addChild(yuanGuItem, VESTMENT_MENU_Z);

	auto chaoFanItem = CheckBox::create("storeScene/vestment/store_chaofan_btn0.png",
		"storeScene/vestment/store_chaofan_btn1.png");
	chaoFanItem->setPosition(Vec2(169, 388));
	chaoFanItem->setZoomScale(0);
	chaoFanItem->addEventListener(CC_CALLBACK_2(VestmentLayer::menuChaoFanCallback, this));
	chaoFanItem->setName("ChaoFanItem");
	this->addChild(chaoFanItem, VESTMENT_MENU_Z);

	auto magicBoxItem = MenuItemImage::create(
		"storeScene/vestment/store_magicBox.png",
		"storeScene/vestment/store_magicBox.png",
		CC_CALLBACK_1(VestmentLayer::menuMagicBoxCallback, this));
	magicBoxItem->setPosition(50,110);

	auto materialBoxItem = MenuItemImage::create(
		"storeScene/vestment/store_materialBox.png",
		"storeScene/vestment/store_materialBox.png",
		CC_CALLBACK_1(VestmentLayer::menuMaterialBoxCallback, this));
	materialBoxItem->setPosition(50,45);

	auto introductionItem = MenuItemImage::create(
		"storeScene/vestment/store_introduction.png",
		"storeScene/vestment/store_introduction.png",
		CC_CALLBACK_1(VestmentLayer::menuIntroductionCallback, this));
	introductionItem->setPosition(758, 377);

	auto menu = Menu::create(magicBoxItem, materialBoxItem, introductionItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, VESTMENT_MENU_Z);
	
	_pageView1 = PageView::create();
	_pageView1->setContentSize(Size(800, 450));
	_pageView1->setPosition(Vec2::ZERO);
	_pageView1->setCustomScrollThreshold(PAGEVIEW_THRESHOLD);
	
	_pageCount = 5;

	for (int i = 0; i < _pageCount; i++)
	{
		auto layout = Layout::create();
		layout->setContentSize(Size(800, 450));

		auto sprite = Sprite::create("storeScene/vestment/store_vestment_background.png");
		sprite->setPosition(400, 225);
		layout->addChild(sprite);

		auto vestment = Sprite::create(StringUtils::format("public/keyword_%d.png",i+1));
		vestment->setPosition(400, 225);
		float scale = 220 / vestment->getContentSize().width;
		vestment->setScale(scale);
		layout->addChild(vestment);

		auto item = MenuItemImage::create(
			"storeScene/vestment/store_combine_btn0.png",
			"storeScene/vestment/store_combine_btn1.png",
			CC_CALLBACK_1(VestmentLayer::menuCombineCallback, this));
		item->setPosition(400, 60);

		auto menu = Menu::create(item, NULL);
		menu->setPosition(Vec2::ZERO);
		layout->addChild(menu);

		_pageView1->addPage(layout);
	}

	for (int i = 0; i < _pageCount; i++)
	{
		auto indexSprite = Sprite::create("storeScene/vestment/store_point_0.png");
		indexSprite->setPosition((i - float(_pageCount) / 2) * 20 + 400, 16);
		this->addChild(indexSprite, VESTMENT_SPRITE_Z);
		_indexSprite.pushBack(indexSprite);
	}

	_pageIndex = 0;
	_indexSprite.at(_pageIndex)->setTexture("storeScene/vestment/store_point_1.png");
	_pageView1->addEventListener(CC_CALLBACK_2(VestmentLayer::pageViewEvent, this));

	this->addChild(_pageView1, VESTMENT_PAGEVIEW_Z);

	
	
	return true;
}

void VestmentLayer::menuYuanGuCallback(Ref * pSender, CheckBox::EventType type)
{
	auto yuanGuItem = dynamic_cast<CheckBox *>(this->getChildByName("YuanGuItem"));
	auto chaoFanItem = dynamic_cast<CheckBox *>(this->getChildByName("ChaoFanItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		chaoFanItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		yuanGuItem->setSelected(true);
		break;
	default:
		break;
	}
}

void VestmentLayer::menuChaoFanCallback(Ref * pSender, CheckBox::EventType type)
{
	auto yuanGuItem = dynamic_cast<CheckBox *>(this->getChildByName("YuanGuItem"));
	auto chaoFanItem = dynamic_cast<CheckBox *>(this->getChildByName("ChaoFanItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		yuanGuItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		chaoFanItem->setSelected(true);
		break;
	default:
		break;
	}
}

void VestmentLayer::menuMagicBoxCallback(Ref * pSender)
{

}

void VestmentLayer::menuMaterialBoxCallback(Ref * pSender)
{

}

void VestmentLayer::menuIntroductionCallback(Ref * pSender)
{

}

void VestmentLayer::menuCombineCallback(Ref * pSender)
{

}

void VestmentLayer::pageViewEvent(Ref * pSender, PageView::EventType type)
{
	int index = _pageView1->getCurPageIndex();
	switch (type)
	{
	case PageView::EventType::TURNING:
		

		if (index != _pageIndex)
		{
			_indexSprite.at(_pageIndex)->setTexture("storeScene/vestment/store_point_0.png");
			_pageIndex = index;
			_indexSprite.at(_pageIndex)->setTexture("storeScene/vestment/store_point_1.png");
		}
		break;
	default:
		break;
	}
}

