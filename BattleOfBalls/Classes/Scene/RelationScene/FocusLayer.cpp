#include "FocusLayer.h"

enum FocusZOrder
{
	FOCUS_BACKGROUND_Z,
	FOCUS_SPRITE_Z,
	FOCUS_LABEL_Z,
	FOCUS_LIST_MENU_Z,
	FOCUS_LIST_SPRITE_Z,
	FOCUS_LIST_LABEL_Z,
	FOCUS_LIST_Z,
	FOCUS_MENU_Z,
	FOCUS_LAYER_Z
};

FocusLayer::FocusLayer()
{

}

FocusLayer::~FocusLayer()
{
	_list.clear();
}

bool FocusLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto searchItem = MenuItemImage::create(
		"relationScene/relation_search_btn.png",
		"relationScene/relation_search_btn.png",
		CC_CALLBACK_1(FocusLayer::menuSearchCallback, this));
	searchItem->setPosition(173,374);

	auto sortItem = MenuItemImage::create(
		"relationScene/relation_sort_btn.png",
		"relationScene/relation_sort_btn.png",
		CC_CALLBACK_1(FocusLayer::menuSortCallback, this));
	sortItem->setPosition(696,374);

	auto previousItem = MenuItemImage::create(
		"relationScene/relation_arrow_left.png",
		"relationScene/relation_arrow_left.png",
		CC_CALLBACK_1(FocusLayer::menuPreviousCallback, this));
	previousItem->setPosition(289,22);

	auto nextItem = MenuItemImage::create(
		"relationScene/relation_arrow_right.png",
		"relationScene/relation_arrow_right.png",
		CC_CALLBACK_1(FocusLayer::menuNextCallback, this));
	nextItem->setPosition(481,22);

	auto menu = Menu::create(searchItem, sortItem, previousItem, nextItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, FOCUS_MENU_Z);

	auto focusLimitItem = CheckBox::create("relationScene/relation_huxiang_btn0.png",
		"relationScene/relation_huxiang_btn1.png");
	focusLimitItem->setPosition(Vec2(44,24));
	focusLimitItem->setZoomScale(0);
	focusLimitItem->addEventListener(CC_CALLBACK_2(FocusLayer::menuFocusLimitCallback, this));
	this->addChild(focusLimitItem, FOCUS_MENU_Z);

	auto topSprite = Sprite::create("relationScene/relation_top0.png");
	topSprite->setPosition(400,374);
	this->addChild(topSprite, FOCUS_SPRITE_Z);

	auto title = Sprite::create("relationScene/relation_focus_title.png");
	title->setPosition(400,349);
	this->addChild(title, FOCUS_SPRITE_Z);

	createFriendList();

	return true;
}

void FocusLayer::menuSearchCallback(Ref * pSender)
{

}

void FocusLayer::menuSortCallback(Ref * pSender)
{

}

void FocusLayer::menuPreviousCallback(Ref * pSender)
{
	if (_pageIndex == 1)
	{
		return;
	}
	auto page = _list.at(_pageIndex - 1);
	page->setVisible(false);
	_pageIndex--;

	std::string text = StringUtils::format("%d/%d", _pageIndex, _pageMax);
	_pageLabel->setString(text);

	auto newPage = _list.at(_pageIndex - 1);
	newPage->setVisible(true);
}

void FocusLayer::menuNextCallback(Ref * pSender)
{
	if (_pageIndex == _pageMax)
	{
		return;
	}

	auto page = _list.at(_pageIndex - 1);
	page->setVisible(false);
	_pageIndex++;

	std::string text = StringUtils::format("%d/%d", _pageIndex, _pageMax);
	_pageLabel->setString(text);

	auto newPage = _list.at(_pageIndex - 1);
	newPage->setVisible(true);
}

void FocusLayer::menuFocusLimitCallback(Ref * pSender, CheckBox::EventType type)
{

}

void FocusLayer::menuItemExtendCallback(Ref * pSender)
{

}

void FocusLayer::menuPlayerCallback(Ref * pSender)
{

}

void FocusLayer::createFriendList()
{
	int friendCount = 20;
	int page = (friendCount - 1) / 7 + 1;
	_pageMax = page;
	int count = 0;

	std::string labelText = StringUtils::format("1/%d", page);
	_pageLabel = Label::createWithTTF(labelText.c_str(), "fonts/arial.ttf", 20);
	_pageLabel->setAnchorPoint(Vec2(0.5, 0.5));
	_pageLabel->setPosition(380, 22);
	this->addChild(_pageLabel, FOCUS_LABEL_Z);

	_pageIndex = 1;
	for (int i = 0; i < page; i++)
	{
		auto page = Node::create();
		if (i == 0)
		{
			page->setVisible(true);
		}
		else
		{
			page->setVisible(false);
		}
		page->setPosition(Vec2::ZERO);
		this->addChild(page, FOCUS_LIST_Z);
		_list.pushBack(page);

		

		for (int j = 0; j < 7; j++)
		{
			Map<std::string, Ref *> info;
			
			auto item = createListItem(j,info);
			page->addChild(item);

			if (++count >= friendCount)break;
		}
		if (count >= friendCount)break;
	}
}

Node * FocusLayer::createListItem(int index, Map<std::string, Ref *> & info)
{
	auto playerNode = Node::create();
	
	auto menu = Menu::create();
	menu->setPosition(Vec2::ZERO);
	playerNode->addChild(menu, FOCUS_LIST_MENU_Z);

	if (index % 2 == 0)
	{
		auto btn = MenuItemImage::create(
			"relationScene/relation_item_btn0.png",
			"rerlationScene/relation_item_btn1.png",
			CC_CALLBACK_1(FocusLayer::menuPlayerCallback, this));
		btn->setPosition(400,311-41*index);

		menu->addChild(btn);
	}
	else
	{
		auto btn = MenuItemImage::create(
			"relationScene/relation_item_btn2.png",
			"rerlationScene/relation_item_btn3.png",
			CC_CALLBACK_1(FocusLayer::menuPlayerCallback, this));

		btn->setPosition(400, 311 - 41 * index);

		menu->addChild(btn);
	}

	auto extendItem = MenuItemImage::create(
		"relationScene/relation_itemExtend.png",
		"rerlationScene/relation_itemExtend.png",
		CC_CALLBACK_1(FocusLayer::menuItemExtendCallback, this));
	extendItem->setPosition(741, 311 - 41 * index);

	menu->addChild(extendItem);

	/*auto label = Label::createWithTTF("out", "fonts/arial.ttf", 20);
	label->setColor(Color3B(255, 255, 255));
	label->setPosition(100, 20);
	playerNode->addChild(label, FOCUS_LIST_LABEL_Z);*/

	return playerNode;
}