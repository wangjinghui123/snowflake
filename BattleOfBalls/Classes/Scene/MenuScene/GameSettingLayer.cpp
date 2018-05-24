#include "GameSettingLayer.h"

const float MIN_TOUCH_DISTANCE = 10.0;

enum GameSettingZOrder
{
	GAME_BACKGROUND_Z,
	GAME_SPRITE_Z,
	GAME_LAYER_Z,
	GAME_MENU_Z,
	GAME_SCROLL_LAYER_Z
};

GameSettingLayer::GameSettingLayer()
{
	_touchDistance = 0;
	_currentLayer = nullptr;
	_touchBox = nullptr;
}

GameSettingLayer::~GameSettingLayer()
{

}

bool GameSettingLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_gameItem1 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_gameSetting_btn0.png",
		"menuScene/settingLayer/setting_game_gameSetting_btn0.png",
		CC_CALLBACK_1(GameSettingLayer::menuGameCallback, this));
	_gameItem1->setPosition(118, 358);
	_gameItem1->setVisible(false);

	_gameItem2 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_gameSetting_btn1.png",
		"menuScene/settingLayer/setting_game_gameSetting_btn1.png",
		CC_CALLBACK_1(GameSettingLayer::menuGameCallback, this));
	_gameItem2->setPosition(118, 358);

	_operationItem1 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_operationSetting_btn0.png",
		"menuScene/settingLayer/setting_game_operationSetting_btn0.png",
		CC_CALLBACK_1(GameSettingLayer::menuOperationCallback, this));
	_operationItem1->setPosition(236, 358);

	_operationItem2 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_operationSetting_btn1.png",
		"menuScene/settingLayer/setting_game_operationSetting_btn1.png",
		CC_CALLBACK_1(GameSettingLayer::menuOperationCallback, this));
	_operationItem2->setPosition(236, 358);
	_operationItem2->setVisible(false);

	_matchItem1 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_matchSetting_btn0.png",
		"menuScene/settingLayer/setting_game_matchSetting_btn0.png",
		CC_CALLBACK_1(GameSettingLayer::menuMatchCallback, this));
	_matchItem1->setPosition(354, 358);

	_matchItem2 = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_matchSetting_btn1.png",
		"menuScene/settingLayer/setting_game_matchSetting_btn1.png",
		CC_CALLBACK_1(GameSettingLayer::menuMatchCallback, this));
	_matchItem2->setPosition(354, 358);
	_matchItem2->setVisible(false);

	auto menu = Menu::create(_gameItem1, _gameItem2, _operationItem1, _operationItem2, _matchItem1, _matchItem2, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, GAME_MENU_Z);

	auto background = Sprite::create("menuScene/settingLayer/setting_game_background.png");
	background->setPosition(400, 203);
	this->addChild(background, GAME_BACKGROUND_Z);

	createGameSettingLayer();

	return true;
}

void GameSettingLayer::menuGameCallback(Ref * pSender)
{
	_gameItem1->setVisible(false);
	_gameItem2->setVisible(true);
	_operationItem1->setVisible(true);
	_operationItem2->setVisible(false);
	_matchItem1->setVisible(true);
	_matchItem2->setVisible(false);

	createGameSettingLayer();
}

void GameSettingLayer::menuOperationCallback(Ref * pSender)
{
	_gameItem1->setVisible(true);
	_gameItem2->setVisible(false);
	_operationItem1->setVisible(false);
	_operationItem2->setVisible(true);
	_matchItem1->setVisible(true);
	_matchItem2->setVisible(false);

	createOperationSettingLayer();
}

void GameSettingLayer::menuMatchCallback(Ref * pSender)
{
	_gameItem1->setVisible(true);
	_gameItem2->setVisible(false);
	_operationItem1->setVisible(true);
	_operationItem2->setVisible(false);
	_matchItem1->setVisible(false);
	_matchItem2->setVisible(true);

	createMatchSettingLayer();
}

void GameSettingLayer::menuCheckCallback(Ref * pSender, CheckBox::EventType type)
{
	auto target = dynamic_cast<CheckBox *>(pSender);
	std::string name = target->getName();

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		if (name == "QualityItem1" || name == "QualityItem2" || name == "QualityItem3")
		{
			auto item1 = dynamic_cast<CheckBox *>(_scrollView->getChildByName("QualityItem1"));
			auto item2 = dynamic_cast<CheckBox *>(_scrollView->getChildByName("QualityItem2"));
			auto item3 = dynamic_cast<CheckBox *>(_scrollView->getChildByName("QualityItem3"));
			item1->setSelected(false);
			item2->setSelected(false);
			item3->setSelected(false);
			target->setSelected(true);
		}
		else if (name == "ChatItem1")
		{
			auto item = dynamic_cast<CheckBox *>(_scrollView->getChildByName("ChatItem2"));
			item->setSelected(false);
		}
		else if (name == "ChatItem2")
		{
			auto item = dynamic_cast<CheckBox *>(_scrollView->getChildByName("ChatItem1"));
			item->setSelected(false);
		}
		break;
	case CheckBox::EventType::UNSELECTED:
		if (name == "QualityItem1" || name == "QualityItem2" || name == "QualityItem3")
		{
			target->setSelected(true);
		}
		else if (name == "ChatItem1")
		{
			target->setSelected(true);
		}
		else if (name == "ChatItem2")
		{
			target->setSelected(true);
		}
		break;
	default:
		break;
	}


}

void GameSettingLayer::menuVoiceCallback(Ref * pSender)
{

}

void GameSettingLayer::createGameSettingLayer()
{
	float p[30] = { 185, 409, 426, 409, 185, 370, 296, 370, 426, 370, 185, 331, 185, 292, 426, 292,
		185, 253, 185, 175, 185, 136, 426, 136, 185, 97, 185, 58, 185, 19 };
	const char * name[15] = { "SoundItem1", "SoundItem2", "QualityItem1", "QualityItem2", "QualityItem3", "VestmentItem1", "InvitationItem1",
		"InvitationItem2", "ButtonItem1", "BulletScreenItem1", "ChatItem1", "ChatItem2", "QuickChatItem2", "PushItem1", "PrivacyItem1" };

	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}
	_currentLayer = Layer::create();
	this->addChild(_currentLayer, GAME_LAYER_Z);

	_scrollView = ScrollView::create();
	_scrollView->setPosition(Vec2(62, 32));
	_scrollView->setContentSize(Size(676, 302));
	_scrollView->setInnerContainerSize(Size(676, 428));
	_scrollView->setDirection(ScrollView::Direction::VERTICAL);
	_scrollView->setScrollBarEnabled(false);
	_scrollView->setBounceEnabled(true);


	auto background = Sprite::create("menuScene/settingLayer/setting_game_background0.png");
	background->setPosition(338, 214);
	_scrollView->addChild(background);

	for (int i = 0; i < 15; i++)
	{
		auto item = CheckBox::create(
			"menuScene/settingLayer/setting_game_check_btn0.png",
			"menuScene/settingLayer/setting_game_check_btn1.png");
		item->setZoomScale(0);
		item->setPosition(Vec2(p[i * 2], p[i * 2 + 1]));
		item->addEventListener(CC_CALLBACK_2(GameSettingLayer::menuCheckCallback, this));
		item->setName(StringUtils::format("%s",name[i]));
		_scrollView->addChild(item);
	}

	auto scrollLayer = Layer::create();
	scrollLayer->setContentSize(Size(676, 302));
	scrollLayer->setPosition(Vec2(62, 32));
	//this->addChild(scrollLayer, GAME_SCROLL_LAYER_Z);

	
	_currentLayer->addChild(_scrollView);
	_currentLayer->addChild(scrollLayer);

	auto listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(GameSettingLayer::onTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_2(GameSettingLayer::onTouchMoved, this);
	listener->onTouchEnded = CC_CALLBACK_2(GameSettingLayer::onTouchEnded, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, scrollLayer);
}

void GameSettingLayer::createOperationSettingLayer()
{
	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}
	_currentLayer = Layer::create();
	this->addChild(_currentLayer, GAME_LAYER_Z);

	auto background = Sprite::create("menuScene/settingLayer/setting_game_operationBackground.png");
	background->setPosition(400, 255);
	_currentLayer->addChild(background, GAME_BACKGROUND_Z);

	auto label1 = Sprite::create("menuScene/settingLayer/setting_game_operation_label1.png");
	label1->setPosition(160, 149);
	_currentLayer->addChild(label1, GAME_SPRITE_Z);

	auto label2 = Sprite::create("menuScene/settingLayer/setting_game_operation_label2.png");
	label2->setPosition(160, 100);
	_currentLayer->addChild(label2, GAME_SPRITE_Z);

	auto label3 = Sprite::create("menuScene/settingLayer/setting_game_operation_label3.png");
	label3->setPosition(150, 51);
	_currentLayer->addChild(label3, GAME_SPRITE_Z);

	_spitBtn = Sprite::create("public/spit_btn.png");
	_spitBtn->setPosition(619, 232);
	_currentLayer->addChild(_spitBtn, GAME_SPRITE_Z);

	_divideBtn = Sprite::create("public/divide_btn.png");
	_divideBtn->setPosition(684, 232);
	_currentLayer->addChild(_divideBtn, GAME_SPRITE_Z);

	auto resetItem = MenuItemImage::create(
		"menuScene/settingLayer/setting_game_operation_reset.png",
		"menuScene/settingLayer/setting_game_operation_reset.png",
		CC_CALLBACK_1(GameSettingLayer::menuResetCallback, this));
	resetItem->setPosition(715, 355);

	auto menu = Menu::create(resetItem, NULL);
	menu->setPosition(Vec2::ZERO);
	_currentLayer->addChild(menu, GAME_MENU_Z);

	auto sliderSize1 = Slider::create();
	sliderSize1->loadBarTexture("menuScene/settingLayer/setting_game_sliderTrack.png");
	sliderSize1->loadSlidBallTextures("menuScene/settingLayer/setting_game_sliderThumb.png", "menuScene/settingLayer/setting_game_sliderThumb.png", "");
	sliderSize1->setPosition(Vec2(475, 149));
	sliderSize1->addEventListener(CC_CALLBACK_2(GameSettingLayer::sporeSizeEvent, this));
	sliderSize1->setPercent(50);
	_currentLayer->addChild(sliderSize1, GAME_MENU_Z);

	auto sliderSize2 = (Slider *)sliderSize1->clone();
	sliderSize2->setPosition(Vec2(475, 100));
	sliderSize2->addEventListener(CC_CALLBACK_2(GameSettingLayer::divideSizeEvent, this));
	sliderSize2->setPercent(50);
	_currentLayer->addChild(sliderSize2, GAME_MENU_Z);

	auto sliderOpacity = (Slider *)sliderSize1->clone();
	sliderOpacity->setPosition(Vec2(475, 51));
	sliderOpacity->addEventListener(CC_CALLBACK_2(GameSettingLayer::opacityEvent, this));
	sliderOpacity->setPercent(0);
	_currentLayer->addChild(sliderOpacity, GAME_MENU_Z);

}

void GameSettingLayer::createMatchSettingLayer()
{
	if (_currentLayer != nullptr)
	{
		_currentLayer->removeFromParentAndCleanup(true);
		_currentLayer = nullptr;
	}
	_currentLayer = Layer::create();
	this->addChild(_currentLayer, GAME_LAYER_Z);

	auto background = Sprite::create("menuScene/settingLayer/setting_game_match_background.png");
	background->setPosition(345, 196);
	_currentLayer->addChild(background, GAME_BACKGROUND_Z);

	auto checkItem1 = CheckBox::create("menuScene/settingLayer/setting_game_check_btn0.png",
		"menuScene/settingLayer/setting_game_check_btn1.png");
	checkItem1->addEventListener(CC_CALLBACK_2(GameSettingLayer::menuMatchCheckCallback, this));
	checkItem1->setPosition(Vec2(274, 238));
	checkItem1->setName("CheckItem1");
	checkItem1->setZoomScale(0);
	_currentLayer->addChild(checkItem1, GAME_MENU_Z);

	auto checkItem2 = CheckBox::create("menuScene/settingLayer/setting_game_check_btn0.png",
		"menuScene/settingLayer/setting_game_check_btn1.png");
	checkItem2->addEventListener(CC_CALLBACK_2(GameSettingLayer::menuMatchCheckCallback, this));
	checkItem2->setPosition(Vec2(274, 153));
	checkItem2->setName("CheckItem2");
	checkItem2->setZoomScale(0);
	_currentLayer->addChild(checkItem2, GAME_MENU_Z);

	auto checkItem3 = CheckBox::create("menuScene/settingLayer/setting_game_smallCheck_btn0.png",
		"menuScene/settingLayer/setting_game_smallCheck_btn1.png");
	checkItem3->addEventListener(CC_CALLBACK_2(GameSettingLayer::menuMatchCheckCallback, this));
	checkItem3->setPosition(Vec2(317, 87));
	checkItem3->setName("CheckItem3");
	checkItem3->setZoomScale(0);
	_currentLayer->addChild(checkItem3, GAME_MENU_Z);
}

void GameSettingLayer::menuResetCallback(Ref * pSender)
{

}

void GameSettingLayer::sporeSizeEvent(Ref * pSender, Slider::EventType type)
{
	if (type == Slider::EventType::ON_PERCENTAGE_CHANGED)
	{
		Slider* slider = dynamic_cast<Slider*>(pSender);
		int percent = slider->getPercent();

		_spitBtn->setScale((float)percent / 50);
	}
}

void GameSettingLayer::divideSizeEvent(Ref * pSender, Slider::EventType type)
{
	if (type == Slider::EventType::ON_PERCENTAGE_CHANGED)
	{
		Slider* slider = dynamic_cast<Slider*>(pSender);
		int percent = slider->getPercent();
		_divideBtn->setScale((float)percent / 50);
	}
}

void GameSettingLayer::opacityEvent(Ref * pSender, Slider::EventType type)
{
	if (type == Slider::EventType::ON_PERCENTAGE_CHANGED)
	{
		Slider* slider = dynamic_cast<Slider*>(pSender);
		int percent = slider->getPercent();

		_spitBtn->setOpacity(255 * (float)(100 - percent) / 100);
		_divideBtn->setOpacity(255 * (float)(100 - percent) / 100);
	}
}

void GameSettingLayer::menuMatchCheckCallback(Ref * pSender, CheckBox::EventType type)
{
	auto item = dynamic_cast<CheckBox *>(pSender);
	std::string name = item->getName();
	auto item1 = dynamic_cast<CheckBox *>(_currentLayer->getChildByName("CheckItem1"));
	auto item2 = dynamic_cast<CheckBox *>(_currentLayer->getChildByName("CheckItem2"));
	auto item3 = dynamic_cast<CheckBox *>(_currentLayer->getChildByName("CheckItem3"));

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		if (name == "CheckItem1")
		{
			item2->setSelected(false);
			item3->setSelected(false);
		}
		else if (name == "CheckItem2")
		{
			item1->setSelected(false);
		}
		else if (name == "CheckItem3")
		{
			item1->setSelected(false);
			item2->setSelected(true);
		}
		break;

	case CheckBox::EventType::UNSELECTED:
		if (name == "CheckItem1")
		{
			item1->setSelected(true);
		}
		else if (name == "CheckItem2")
		{
			item1->setSelected(true);
			item3->setSelected(false);
		}
		break;
	default:
		break;
	}

}

bool GameSettingLayer::onTouchBegan(Touch * touch, Event * event)
{
	/*cocos2d-x中按钮会吞没触摸事件，导致包含按钮的容器ScrollView无法接收到触摸消息，出现无法滑动
	的现象，这里使用自制算法解决*/
	auto target = event->getCurrentTarget();
	auto point = target->convertToNodeSpace(touch->getLocation());
	Rect rect;
	rect.size = target->getContentSize();
	if (rect.containsPoint(point))
	{
		_touchDistance = 0;
		auto container = _scrollView->getInnerContainer();
		auto childList = container->getChildren();

		for (auto child : childList)
		{
			std::string name = child->getName();
			if (name != "")
			{

				point = child->convertToNodeSpace(touch->getLocation());
				rect.size = child->getContentSize();
				if (rect.containsPoint(point))
				{
					_touchBox = dynamic_cast<CheckBox *>(child);
					//_touchBox->onTouchBegan(touch, event);
					break;
				}
			}
		}

		_scrollView->onTouchBegan(touch, event);

		return true;
	}

	return false;
}

void GameSettingLayer::onTouchMoved(Touch * touch, Event * event)
{
	if (_touchBox != nullptr)
	{
		_touchDistance += touch->getDelta().length();
	}

	_scrollView->onTouchMoved(touch, event);		//将触摸事件下发给ScrollView
}

void GameSettingLayer::onTouchEnded(Touch * touch, Event * event)
{

	if (_touchBox != nullptr)
	{
		if (_touchDistance <= MIN_TOUCH_DISTANCE)
		{
			_touchBox->onTouchEnded(touch, event);
		}

		_touchDistance = 0;
		_touchBox = nullptr;
	}
	_scrollView->onTouchEnded(touch, event);		//将触摸事件下发给ScrollView
}
