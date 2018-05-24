#include "SkinLayer.h"
#include "StoreScene.h"

const float MIN_TOUCH_DISTANCE = 10.0;

enum SkinZOrder
{
	SKIN_BACKGROUND_Z,
	SKIN_LABEL_Z,
	SKIN_LIST_Z,
	SKIN_MENU_Z,
	SKIN_SCROLL_LAYER_Z
};

SkinLayer::SkinLayer()
{
	_currentLayer = nullptr;
	_touchMenu = nullptr;
	_touchDistance = 0;
}

SkinLayer::~SkinLayer()
{

}

bool SkinLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto haloItem = CheckBox::create("storeScene/skin/store_halo_btn0.png",
		"storeScene/skin/store_halo_btn1.png");
	haloItem->setPosition(Vec2(65,388));
	haloItem->setZoomScale(0);
	haloItem->addEventListener(CC_CALLBACK_2(SkinLayer::menuHaloCallback, this));
	haloItem->setName("HaloItem");
	haloItem->setSelected(true);
	this->addChild(haloItem, SKIN_MENU_Z);

	auto sporeItem = CheckBox::create("storeScene/skin/store_spore_btn0.png",
		"storeScene/skin/store_spore_btn1.png");
	sporeItem->setPosition(Vec2(169,388));
	sporeItem->setZoomScale(0);
	sporeItem->addEventListener(CC_CALLBACK_2(SkinLayer::menuSporeCallback, this));
	sporeItem->setName("SporeItem");
	this->addChild(sporeItem, SKIN_MENU_Z);

	auto shadowItem = CheckBox::create("storeScene/skin/store_shadow_btn0.png",
		"storeScene/skin/store_shadow_btn1.png");
	shadowItem->setPosition(Vec2(273, 388));
	shadowItem->setZoomScale(0);
	shadowItem->addEventListener(CC_CALLBACK_2(SkinLayer::menuShadowCallback, this));
	shadowItem->setName("ShadowItem");
	this->addChild(shadowItem, SKIN_MENU_Z);

	auto keywordItem = CheckBox::create("storeScene/skin/store_keyword_btn0.png",
		"storeScene/skin/store_keyword_btn1.png");
	keywordItem->setPosition(Vec2(376, 388));
	keywordItem->setZoomScale(0);
	keywordItem->addEventListener(CC_CALLBACK_2(SkinLayer::menuKeywordCallback, this));
	keywordItem->setName("KeywordItem");
	this->addChild(keywordItem, SKIN_MENU_Z);

	auto sortItem = MenuItemImage::create(
		"storeScene/skin/store_skin_drowdownMenu.png",
		"storeScene/skin/store_skin_drowdownMenu.png",
		CC_CALLBACK_1(SkinLayer::menuSortCallback, this));
	sortItem->setPosition(717, 386);

	auto menu = Menu::create(sortItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, SKIN_MENU_Z);

	auto label = Sprite::create("storeScene/skin/store_skin_label0.png");
	label->setPosition(618, 384);
	this->addChild(label, SKIN_LABEL_Z);

	createHaloLayer();

	return true;
}

void SkinLayer::menuHaloCallback(Ref * pSender, CheckBox::EventType type)
{
	auto haloItem = dynamic_cast<CheckBox *>(this->getChildByName("HaloItem"));
	auto sporeItem = dynamic_cast<CheckBox *>(this->getChildByName("SporeItem"));
	auto shadowItem = dynamic_cast<CheckBox *>(this->getChildByName("ShadowItem"));
	auto keywordItem = dynamic_cast<CheckBox *>(this->getChildByName("KeywordItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		sporeItem->setSelected(false);
		shadowItem->setSelected(false);
		keywordItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		haloItem->setSelected(true);
		break;
	}
}

void SkinLayer::menuSporeCallback(Ref * pSender, CheckBox::EventType type)
{
	auto haloItem = dynamic_cast<CheckBox *>(this->getChildByName("HaloItem"));
	auto sporeItem = dynamic_cast<CheckBox *>(this->getChildByName("SporeItem"));
	auto shadowItem = dynamic_cast<CheckBox *>(this->getChildByName("ShadowItem"));
	auto keywordItem = dynamic_cast<CheckBox *>(this->getChildByName("KeywordItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		haloItem->setSelected(false);
		shadowItem->setSelected(false);
		keywordItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		sporeItem->setSelected(true);
		break;
	}
}

void SkinLayer::menuShadowCallback(Ref * pSender, CheckBox::EventType type)
{
	auto haloItem = dynamic_cast<CheckBox *>(this->getChildByName("HaloItem"));
	auto sporeItem = dynamic_cast<CheckBox *>(this->getChildByName("SporeItem"));
	auto shadowItem = dynamic_cast<CheckBox *>(this->getChildByName("ShadowItem"));
	auto keywordItem = dynamic_cast<CheckBox *>(this->getChildByName("KeywordItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		haloItem->setSelected(false);
		sporeItem->setSelected(false);
		keywordItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		shadowItem->setSelected(true);
		break;
	}
}

void SkinLayer::menuKeywordCallback(Ref * pSender, CheckBox::EventType type)
{
	auto haloItem = dynamic_cast<CheckBox *>(this->getChildByName("HaloItem"));
	auto sporeItem = dynamic_cast<CheckBox *>(this->getChildByName("SporeItem"));
	auto shadowItem = dynamic_cast<CheckBox *>(this->getChildByName("ShadowItem"));
	auto keywordItem = dynamic_cast<CheckBox *>(this->getChildByName("KeywordItem"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		haloItem->setSelected(false);
		sporeItem->setSelected(false);
		shadowItem->setSelected(false);
		break;
	case CheckBox::EventType::UNSELECTED:
		keywordItem->setSelected(true);
		break;
	}
}

void SkinLayer::menuSortCallback(Ref * pSender)
{

}

void SkinLayer::menuBuyCallback(Ref * pSender)
{
	log("bug"); 
}

void SkinLayer::menuPriceCallback(Ref * pSender)
{
	log("price");
}

void SkinLayer::createHaloLayer()
{
	int count1 = 10;
	int count2 = 15;
	int row1 = (count1 - 1) / 4 + 1;
	int row2 = (count2 - 1) / 4 + 1;
	int height = 261 * (row1 + row2) + 6 * (row1 + row2 - 2) + 18*2;

	_haloList = ScrollView::create();
	_haloList->setPosition(Vec2(0, 38));
	_haloList->setContentSize(Size(800, 330));
	_haloList->setInnerContainerSize(Size(800, height));
	_haloList->setDirection(ScrollView::Direction::VERTICAL);
	_haloList->setScrollBarEnabled(false);
	_haloList->setBounceEnabled(true);
	
	auto scrollLayer = Layer::create();
	scrollLayer->setContentSize(Size(800, 330));
	scrollLayer->setPosition(0, 38);
	this->addChild(scrollLayer, SKIN_SCROLL_LAYER_Z);

	auto listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(SkinLayer::onTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_2(SkinLayer::onTouchMoved, this);
	listener->onTouchEnded = CC_CALLBACK_2(SkinLayer::onTouchEnded, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, scrollLayer);

	int currentY = height;

	auto label1 = Sprite::create("storeScene/skin/store_skin_label1.png");
	auto size = label1->getContentSize();
	label1->setPosition(400, currentY - 9);
	_haloList->addChild(label1);
	currentY -= 18;

	int count = 0;
	for (int i = 0; i <= row1-1; i++)
	{
		currentY -= 267;
		
		for (int j = 0; j < 4; j++)
		{
			int id = count % 8 + 1;
			Vec2 position = Vec2(110 + 194 * j, currentY + 136);
			createListItem(_haloList, position, id);

			if (++count >= count1)break;
		}
		if (count >= count1)break;
	}

	auto label2 = Sprite::create("storeScene/skin/store_skin_label2.png");
	size = label2->getContentSize();
	label2->setPosition(400, currentY - 9);
	_haloList->addChild(label2);
	currentY -= 18;

	count = 0;
	for (int i = 0; i <= row2-1; i++)
	{
		currentY -= 267;
		

		for (int j = 0; j < 4; j++)
		{
			int id = count % 8 + 1;
			Vec2 position = Vec2(110 + 194 * j, currentY + 136);
			createListItem(_haloList, position, id);

			if (++count >= count2)break;
		}
		if (count >= count2)break;
	}

	_haloLayer = Layer::create();
	_haloLayer->addChild(_haloList, SKIN_LIST_Z);
	this->addChild(_haloLayer, SKIN_LIST_Z);
	_list1Position = _haloList->getInnerContainerPosition();
	//_haloList->scrollToPercentVertical(100, 0, false);
}

void SkinLayer::createSporeLayer()
{

}

void SkinLayer::createShadowLayer()
{

}

void SkinLayer::createKeywordLayer()
{
	
}

void SkinLayer::createListItem(ScrollView * scrollView, Vec2 & position, int id)
{
	auto buyItem = MenuItemImage::create(
		"storeScene/skin/store_skin_model.png",
		"storeScene/skin/store_skin_model.png",
		CC_CALLBACK_1(SkinLayer::menuBuyCallback, this));
	buyItem->setPosition(position);

	auto menu = Menu::create(buyItem, NULL);
	menu->setPosition(Vec2::ZERO);
	menu->setName("BuyItem");
	scrollView->addChild(menu);

	auto priceItem = MenuItemImage::create(
		"storeScene/skin/store_skin_price.png",
		"storeScene/skin/store_skin_price.png",
		CC_CALLBACK_1(SkinLayer::menuPriceCallback, this));
	priceItem->setPosition(position.x+61, position.y+103);

	auto menu2 = Menu::create(priceItem, NULL);
	menu2->setPosition(Vec2::ZERO);
	menu2->setName("PriceItem");
	scrollView->addChild(menu2);


	std::string path = StringUtils::format("public/keyword_%d.png", id);
	auto sprite = Sprite::create(path.c_str());
	sprite->setPosition(position.x, position.y + 35);
	scrollView->addChild(sprite);

	auto label = Label::createWithTTF("200", "fonts/arial.ttf", 22);
	auto lolly = Sprite::create("storeScene/store_lolly.png");
	auto size = label->getContentSize();
	lolly->setPosition(size.width + 11, 12);
	label->addChild(lolly);
	label->setAnchorPoint(Vec2(0.5, 0.5));
	label->setPosition(position.x, position.y - 87);
	label->setColor(Color3B(255, 255, 255));
	label->setContentSize(Size(size.width + 22, 24));
	scrollView->addChild(label);
}

bool SkinLayer::onTouchBegan(Touch * touch, Event * event)
{
	/*cocos2d-x中按钮会吞没触摸事件，导致包含按钮的容器ScrollView无法接收到触摸消息，出现无法滑动
	的现象，这里使用自制算法解决*/
	auto target = event->getCurrentTarget();
	auto point = target->convertToNodeSpace(touch->getLocation());
	Rect rect;
	rect.size = target->getContentSize();
	if (rect.containsPoint(point))
	{
		auto container = _haloList->getInnerContainer();
		auto childList = container->getChildren();
		Vector<Menu * > list;
		for (auto child : childList)
		{
			std::string name = child->getName();
			if (name == "BuyItem" || name == "PriceItem")
			{
				auto menu = dynamic_cast<Menu *>(child);
				auto item = *(menu->getChildren().begin());
				Vec2 p = item->convertToNodeSpace(touch->getLocation());
				Rect rect;
				rect.size = item->getContentSize();
				if (rect.containsPoint(p))
				{
					list.pushBack(menu);
					if (name == "PriceItem")break;
				}
			}
		}

		if (list.size() == 2)
		{
			for (auto item : list)
			{
				if (item->getName() == "PriceItem")
				{
					item->onTouchBegan(touch, event);
					_touchMenu = item;
				}
			}
		}
		else if (list.size() == 1)
		{
			auto item = list.at(0);
			item->onTouchBegan(touch, event);
			_touchMenu = item;
		}

		list.clear();

		_haloList->onTouchBegan(touch, event);
		return true;
	}
	return false;
}

void SkinLayer::onTouchMoved(Touch * touch, Event * event)
{
	if (_touchMenu != nullptr)
	{
		auto delta = touch->getDelta();
		_touchDistance += delta.length();
	}
	
	_haloList->onTouchMoved(touch, event);		//将触摸事件下发给ScrollView
}

void SkinLayer::onTouchEnded(Touch * touch, Event * event)
{
	if (_touchMenu != nullptr)
	{
		if (_touchDistance <= MIN_TOUCH_DISTANCE)
		{
			_touchMenu->onTouchEnded(touch, event);
			
		}

		_touchDistance = 0.0f;
		_touchMenu = nullptr;
	}

	_haloList->onTouchEnded(touch, event);		//将触摸事件下发给ScrollView
}
