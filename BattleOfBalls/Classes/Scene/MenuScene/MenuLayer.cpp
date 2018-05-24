#pragma execution_character_set("utf-8")
#include "MenuLayer.h"
#include "../SceneManager.h"
#include "Header/AppMacros.h"
#include "SignLayer.h"
#include "SettingLayer.h"
#include "GetLollyLayer.h"
#include "ChatLayer.h"
#include "FindFriendLayer.h"
#include "TeamModeLayer.h"
#include "SurvivalModeLayer.h"
#include "CustomModeLayer.h"
#include "../StoryScene/StoryScene.h"
#include "../PlayerScene/PlayerScene.h"
#include "../StoreScene/StoreScene.h"
#include "../WatchGameScene/WatchGameScene.h"
#include "Tools/DropDownList/DropDownList.h"
#include "Tools/PromptBox/PromptBox.h"
#include "Tools/CsvUtils/CsvUtils.h"
#include "Header/Common.h"

enum MenuTag
{
	TAG_MENU_1,
	TAG_MAX
};

enum MenuZOrder
{
	MENU_BACKGROUND_Z,
	MENU_SPRITE_Z,
	MENU_MENU_Z,
	MENU_LABEL_Z,
	MENU_TEXTFIELD_Z,
	MENU_DROPDOWNLIST_Z,
	MENU_LAYER_Z,
	MENU_PROMPT_Z,
	MENU_MAX_Z
};

MenuLayer::MenuLayer()
{

}

MenuLayer::~MenuLayer()
{
	//_menuList.clear();
	PromptBox::getInstance()->clearList();
	_eventDispatcher->removeCustomEventListeners("GameStart");
}

bool MenuLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	_extendItem1 = MenuItemImage::create(
		"menuScene/menu_extend_btn0.png",
		"menuScene/menu_extend_btn0.png",
		CC_CALLBACK_1(MenuLayer::menuExtendCallback, this));
	_extendItem1->setPosition(757, 403);

	_extendItem2 = MenuItemImage::create(
		"menuScene/menu_extend_btn1.png",
		"menuScene/menu_extend_btn1.png",
		CC_CALLBACK_1(MenuLayer::menuExtendCallback, this));
	_extendItem2->setPosition(757, 403);
	_extendItem2->setVisible(false);

	auto signItem = MenuItemImage::create(
		"menuScene/menu_sign_btn.png",
		"menuScene/menu_sign_btn.png",
		CC_CALLBACK_1(MenuLayer::menuSignCallback, this));
	signItem->setPosition(757, 343);

	auto watchGameItem = MenuItemImage::create(
		"menuScene/menu_watchGame_btn.png",
		"menuScene/menu_watchGame_btn.png",
		CC_CALLBACK_1(MenuLayer::menuWatchGameCallback, this));
	watchGameItem->setPosition(757, 295);

	auto settingItem = MenuItemImage::create(
		"menuScene/menu_setting_btn.png",
		"menuScene/menu_setting_btn.png",
		CC_CALLBACK_1(MenuLayer::menuSettingCallback, this));
	settingItem->setPosition(757, 243);

	auto getLollyItem = MenuItemImage::create(
		"menuScene/menu_getLolly_btn.png",
		"menuScene/menu_getLolly_btn.png",
		CC_CALLBACK_1(MenuLayer::menuGetLollyCallback, this));
	getLollyItem->setPosition(757, 189);

	auto storyItem = MenuItemImage::create(
		"menuScene/menu_story_btn.png",
		"menuScene/menu_story_btn.png",
		CC_CALLBACK_1(MenuLayer::menuStoryCallback, this));
	storyItem->setPosition(757, 139);

	auto strategyItem = MenuItemImage::create(
		"menuScene/menu_strategy_btn.png",
		"menuScene/menu_strategy_btn.png",
		CC_CALLBACK_1(MenuLayer::menuStrategyCallback, this));
	strategyItem->setPosition(757, 88);

	auto menu1 = Menu::create(signItem, watchGameItem, settingItem, getLollyItem, storyItem, strategyItem, NULL);
	menu1->setPosition(Vec2::ZERO);
	menu1->setVisible(false);
	this->addChild(menu1, MENU_MENU_Z, TAG_MENU_1);
	_menuList.pushBack(menu1);

	auto teamItem = MenuItemImage::create(
		"menuScene/menu_team_btn0.png",
		"menuScene/menu_team_btn1.png",
		CC_CALLBACK_1(MenuLayer::menuTeamCallback, this));
	teamItem->setPosition(187, 49);

	auto relationItem = MenuItemImage::create(
		"menuScene/menu_relation_btn0.png",
		"menuScene/menu_relation_btn1.png",
		CC_CALLBACK_1(MenuLayer::menuRelationCallback, this));
	relationItem->setPosition(329, 49);

	auto rankItem = MenuItemImage::create(
		"menuScene/menu_rank_btn0.png",
		"menuScene/menu_rank_btn1.png",
		CC_CALLBACK_1(MenuLayer::menuRankCallback, this));
	rankItem->setPosition(460, 49);

	auto storeItem = MenuItemImage::create(
		"menuScene/menu_store_btn0.png",
		"menuScene/menu_store_btn1.png",
		CC_CALLBACK_1(MenuLayer::menuStoreCallback, this));
	storeItem->setPosition(602, 49);

	auto menu2 = Menu::create(teamItem, relationItem, rankItem, storeItem, NULL);
	menu2->setPosition(Vec2::ZERO);
	this->addChild(menu2, MENU_MENU_Z);
	_menuList.pushBack(menu2);

	Node * node = NULL;
	int i = 0;
	auto itemList = menu2->getChildren();
	for (auto node : itemList)
	{
		auto item = dynamic_cast<MenuItemImage *>(node);
		item->setPosition(180 + i * 140, 20);
		item->setOpacity(0);
		item->setEnabled(false);

		auto moveTo = CCMoveTo::create(1, Vec2(180 + i * 140, 49));
		auto fadeIn = CCFadeIn::create(1);
		auto delay = CCDelayTime::create(0.3*i);
		auto callfunc = CallFuncN::create(CC_CALLBACK_0(MenuLayer::setMenuEnable, this, item));
		auto spawn = Spawn::create(moveTo, fadeIn, NULL);
		auto seq = CCSequence::create(delay, spawn, callfunc, NULL);
		item->runAction(seq);
		i++;
	}

	auto playerItem = MenuItemImage::create(
		"menuScene/menu_player_btn.png",
		"menuScene/menu_player_btn.png",
		CC_CALLBACK_1(MenuLayer::menuPlayerCallback, this));
	playerItem->setPosition(82, 412);

	auto startItem = MenuItemImage::create(
		"menuScene/menu_start_btn.png",
		"menuScene/menu_start_btn.png",
		CC_CALLBACK_1(MenuLayer::menuStartCallback, this));
	startItem->setPosition(383, 150);

	auto teamModeItem = MenuItemImage::create(
		"menuScene/menu_teamMode_btn.png",
		"menuScene/menu_teamMode_btn.png",
		CC_CALLBACK_1(MenuLayer::menuTeamModeCallback, this));
	teamModeItem->setPosition(564, 150);

	auto survivalModeItem = MenuItemImage::create(
		"menuScene/menu_survivalMode_btn.png",
		"menuScene/menu_survivalMode_btn.png",
		CC_CALLBACK_1(MenuLayer::menuSurvivalModeCallback, this));
	survivalModeItem->setPosition(648, 150);

	auto customModeItem = MenuItemImage::create(
		"menuScene/menu_customMode_btn.png",
		"menuScene/menu_customMode_btn.png",
		CC_CALLBACK_1(MenuLayer::menuCustomModeCallback, this));
	customModeItem->setPosition(544, 221);

	auto findFriendItem = MenuItemImage::create(
		"menuScene/menu_findFriend_btn.png",
		"menuScene/menu_findFriend_btn.png",
		CC_CALLBACK_1(MenuLayer::menuFindFriendCallback, this));
	findFriendItem->setPosition(694, 403);


	auto resetNameItem = MenuItemImage::create(
		"menuScene/menu_reset.png",
		"menuScene/menu_reset.png",
		CC_CALLBACK_1(MenuLayer::menuResetNameCallback, this));
	resetNameItem->setPosition(498, 287);

	auto menu3 = Menu::create(playerItem, startItem, teamModeItem, survivalModeItem, customModeItem,
		_extendItem1, _extendItem2, resetNameItem, findFriendItem, NULL);
	menu3->setPosition(Vec2::ZERO);
	this->addChild(menu3, MENU_MENU_Z);
	_menuList.pushBack(menu3);

	ValueVector vec;
	for (int i = 1; i <= 4; i++)
	{
		vec.push_back(Value(CsvUtils::getInstance()->getMapData(8 + i, 1, "i18n/public.csv")));
	}


	/*创建下拉列表*/
	auto dropDonwList = DropDownList::create(
		"menuScene/menu_dropdown_top.png",
		"menuScene/menu_dropdown_item.png",
		"menuScene/menu_dropdown_scale9.png",
		vec);
	dropDonwList->setPosition(383, 222);
	dropDonwList->setCallback(CC_CALLBACK_1(MenuLayer::menuServerCallback, this));
	this->addChild(dropDonwList, MENU_DROPDOWNLIST_Z);

	/*添加大厅背景*/
	auto background = Sprite::create("menuScene/menu_background.png");
	background->setPosition(400, 225);
	this->addChild(background, MENU_BACKGROUND_Z);

	auto label1 = Sprite::create("menuScene/menu_label1.png");
	label1->setPosition(288, 318);
	this->addChild(label1, MENU_LABEL_Z);

	auto label2 = Sprite::create("menuScene/menu_label2.png");
	label2->setPosition(278, 252);
	this->addChild(label2, MENU_LABEL_Z);

	auto gameNameSprite = Sprite::create("menuScene/menu_input_btn.png");
	gameNameSprite->setPosition(383, 287);
	this->addChild(gameNameSprite, MENU_SPRITE_Z);

	int id = rand() % CSV_NAME_COUNT + 1;
	std::string name = CsvUtils::getInstance()->getMapData(id, 1, "i18n/name.csv");

	/*创建TextField组件，并设置相关参数*/
	_gameName = TextField::create("", "fonts/STSONG.TTF", 22);
	_gameName->ignoreContentAdaptWithSize(false);
	_gameName->setContentSize(Size(240, 30));
	_gameName->setMaxLength(12);
	_gameName->setMaxLengthEnabled(true);
	_gameName->setString(name);
	_gameName->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_gameName->setTextVerticalAlignment(TextVAlignment::CENTER);
	_gameName->setPosition(Vec2(368, 286));

	_gameName->addEventListener(CC_CALLBACK_2(MenuLayer::gameNameEvent, this));
	this->addChild(_gameName, MENU_TEXTFIELD_Z);

	/*事件监听器*/
	_eventDispatcher->addCustomEventListener("GameStart", CC_CALLBACK_1(MenuLayer::gameStartEvent, this));

	return true;
}

void MenuLayer::menuFindFriendCallback(Ref * pSender)
{
	auto layer = FindFriendLayer::create();
	this->addChild(layer, MENU_LAYER_Z);
}

void MenuLayer::menuExtendCallback(Ref * pSender)
{
	if (_extendItem1->isVisible())
	{
		_extendItem1->setVisible(false);
		_extendItem2->setVisible(true);
		auto menu = this->getChildByTag(TAG_MENU_1);
		menu->setVisible(true);
	}
	else
	{
		_extendItem1->setVisible(true);
		_extendItem2->setVisible(false);
		auto menu = this->getChildByTag(TAG_MENU_1);
		menu->setVisible(false);
	}
}

void MenuLayer::menuSignCallback(Ref * pSender)
{
	auto layer = SignLayer::create();
	this->addChild(layer, MENU_LAYER_Z);
}

void MenuLayer::menuSettingCallback(Ref * pSender)
{
	auto layer = SettingLayer::create();
	this->addChild(layer, MENU_LAYER_Z);
}

void MenuLayer::menuWatchGameCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(4, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
	//SceneManager::getInstance()->changeScene(SceneManager::en_WatchGameScene);
}

void MenuLayer::menuGetLollyCallback(Ref * pSender)
{
	auto layer = GetLollyLayer::create();
	this->addChild(layer, MENU_LAYER_Z);
}

void MenuLayer::menuStoryCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_StoryScene);
}

void MenuLayer::menuStrategyCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(5, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
}

void MenuLayer::menuTeamCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(2, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}

	//SceneManager::getInstance()->changeScene(SceneManager::en_TeamScene);
}

void MenuLayer::menuRelationCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_RelationScene);
}

void MenuLayer::menuRankCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(3, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
	//SceneManager::getInstance()->changeScene(SceneManager::en_RankScene);
}

void MenuLayer::menuStoreCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_StoreScene);
}

void MenuLayer::menuPlayerCallback(Ref * pSender)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_PlayerScene);
}

void MenuLayer::menuStartCallback(Ref * pSender)
{
	bool loginState = UserDefault::getInstance()->getBoolForKey("Login");
	if (!loginState)
	{
		std::string text = CsvUtils::getInstance()->getMapData(22, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, MENU_PROMPT_Z);
		}
		return;
	}

	for (auto menu : _menuList)
	{
		menu->setEnabled(false);
	}

	_eventDispatcher->dispatchCustomEvent("Game Start");


	/*向服务器发送开始游戏请求*/
	std::string accountName = WebSocketManager::getInstance()->getAccountName();
	std::string gameName = _gameName->getString();

	rapidjson::Document doc;
	doc.SetObject();
	rapidjson::Document::AllocatorType & allocator = doc.GetAllocator();
	doc.AddMember("MsgType", MessageType::eMsg_START_GAME_SINGLE, allocator);
	doc.AddMember("AccountName", rapidjson::Value(accountName.c_str(), allocator), allocator);
	doc.AddMember("GameName", rapidjson::Value(gameName.c_str(), allocator), allocator);
	doc.AddMember("VestmentID", 2, allocator);
	doc.AddMember("KeywordID", 1, allocator);

	rapidjson::StringBuffer buffer;
	rapidjson::Writer<rapidjson::StringBuffer>write(buffer);
	doc.Accept(write);

	WebSocketManager::getInstance()->sendMsg(buffer.GetString());
}

void MenuLayer::menuSurvivalModeCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(7, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
	/*auto layer = SurvivalModeLayer::create();
	this->addChild(layer, MENU_LAYER_Z);*/
}

void MenuLayer::menuTeamModeCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(6, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
	/*auto layer = TeamModeLayer::create();
	this->addChild(layer, MENU_LAYER_Z);*/
}

void MenuLayer::menuCustomModeCallback(Ref * pSender)
{
	std::string text = CsvUtils::getInstance()->getMapData(8, 1, "i18n/public.csv");
	auto prompt = PromptBox::getInstance()->createPrompt(text);
	if (prompt)
	{
		this->addChild(prompt, MENU_PROMPT_Z);
	}
	/*auto layer = CustomModeLayer::create();
	this->addChild(layer, MENU_LAYER_Z);*/
}

void MenuLayer::setMenuEnable(Ref * pSender)
{
	auto item = dynamic_cast<MenuItemImage *>(pSender);
	item->setEnabled(true);
}


void MenuLayer::menuResetNameCallback(Ref * pSender)
{
	int id = rand() % CSV_NAME_COUNT + 1;
	std::string name = CsvUtils::getInstance()->getMapData(id, 1, "i18n/name.csv");
	_gameName->setString(name);
}

void MenuLayer::menuServerCallback(int index)
{
	log("%d", index);
}


void MenuLayer::gameStartEvent(EventCustom * event)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_GameScene);
}

void MenuLayer::onExit()
{
	this->unscheduleAllCallbacks();
	Layer::onExit();
}

void MenuLayer::gameNameEvent(Ref * pSender, TextField::EventType type)
{

}