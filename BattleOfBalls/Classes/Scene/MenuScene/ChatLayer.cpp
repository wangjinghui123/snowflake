#include "ChatLayer.h"
#include "Tools/CsvUtils/CsvUtils.h"
#include "Tools/PromptBox/PromptBox.h"
#include "Header/Common.h"

const float MIN_TOUCH_DISTANCE = 10.0;

enum ChatZOrder
{
	CHAT_BACKGROUND_Z,
	CHAT_SCROLLVIEW_Z,
	CHAT_MENU_Z,
	CHAT_SPRITE_Z,
	CHAT_TEXTFIELD_Z,
	CHAT_LABEL_Z,
	CHAT_SCROLL_LAYER_Z,
	CHAT_PROMPT_Z,
	CHAT_MESSAGE_SPRITE_Z,
	CHAT_MESSAGE_LABEL_Z
};

ChatLayer::ChatLayer()
{
	_touchDistance = 0;
	_touchFriendItem = nullptr;
	_selectedFriendItem = nullptr;
	_msgCount = 0;
}

ChatLayer::~ChatLayer()
{
	_eventDispatcher->removeCustomEventListeners("ChatMsgReceive");
}

bool ChatLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto sendItem = MenuItemImage::create(
		"menuScene/chatLayer/chat_send_btn0.png",
		"menuScene/chatLayer/chat_send_btn1.png",
		CC_CALLBACK_1(ChatLayer::menuSendCallback, this));
	sendItem->setPosition(747,33);

	auto talkModeItem = MenuItemImage::create(
		"menuScene/chatLayer/chat_talk_mode_btn.png",
		"menuScene/chatLayer/chat_talk_mode_btn.png",
		CC_CALLBACK_1(ChatLayer::menuTalkModeCallback, this));
	talkModeItem->setPosition(373,33);

	auto expressionItem = MenuItemImage::create(
		"menuScene/chatLayer/chat_expression_btn.png",
		"menuScene/chatLayer/chat_expression_btn.png",
		CC_CALLBACK_1(ChatLayer::menuExpressionCallback, this));
	expressionItem->setPosition(684,33);

	auto searchItem = MenuItemImage::create(
		"menuScene/chatLayer/chat_search_btn.png",
		"menuScene/chatLayer/chat_search_btn.png",
		CC_CALLBACK_1(ChatLayer::menuSearchCallback, this));
	searchItem->setPosition(192,428);

	auto menu = Menu::create(sendItem, talkModeItem, expressionItem, searchItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu,CHAT_MENU_Z);

	std::string text = CsvUtils::getInstance()->getMapData(20, 1, "i18n/public.csv");

	_textFieldInput = TextField::create(text, "fonts/STSONG.TTF", 18);
	_textFieldInput->ignoreContentAdaptWithSize(false);
	_textFieldInput->setContentSize(Size(255, 33));
	_textFieldInput->setMaxLength(12);
	_textFieldInput->setMaxLengthEnabled(true);
	_textFieldInput->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldInput->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldInput->setColor(Color3B(0, 0, 0));
	_textFieldInput->setPosition(Vec2(531, 33));
	_textFieldInput->addEventListener(CC_CALLBACK_2(ChatLayer::textFieldInputEvent, this));
	this->addChild(_textFieldInput, CHAT_TEXTFIELD_Z);

	auto background = Sprite::create("menuScene/chatLayer/chat_background.png");;
	background->setPosition(418,227);
	this->addChild(background, CHAT_BACKGROUND_Z);

	auto inputSprite = Sprite::create("menuScene/chatLayer/chat_input_btn.png");
	inputSprite->setPosition(531, 33);
	this->addChild(inputSprite, CHAT_SPRITE_Z);

	_searchSprite = Sprite::create("menuScene/chatLayer/chat_search_sprite.png");
	_searchSprite->setPosition(182,427);
	this->addChild(_searchSprite, CHAT_SPRITE_Z);

	_playerName = Label::createWithTTF("", "fonts/STSONG.TTF", 16);
	_playerName->setPosition(353, 429);
	_playerName->setAnchorPoint(Vec2(0, 0.5));
	_playerName->setTextColor(Color4B(0, 0, 0, 255));
	this->addChild(_playerName, CHAT_LABEL_Z);

	createFriendList();
	createMessageList();

	_eventDispatcher->addCustomEventListener("ChatMsgReceive", CC_CALLBACK_1(ChatLayer::receiveMessageEvent, this));

	return true;
}

void ChatLayer::menuSendCallback(Ref * pSender)
{
	if (_selectedFriendItem == nullptr)
	{
		std::string text = CsvUtils::getInstance()->getMapData(21, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, CHAT_PROMPT_Z);
		}
		return;
	}

	std::string accountName = UserDefault::getInstance()->getStringForKey("AccountName");
	if (accountName == "")
	{
		std::string text = CsvUtils::getInstance()->getMapData(22, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, CHAT_PROMPT_Z);
		}
		return;
	}

	std::string msg = _textFieldInput->getString();
	if (msg == "")
	{
		std::string text = CsvUtils::getInstance()->getMapData(20, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, CHAT_PROMPT_Z);
		}
	}
	else
	{
		std::string senderName = UserDefault::getInstance()->getStringForKey("AccountName");
		std::string receiverName = _selectedFriendItem->getParent()->getName();
		rapidjson::Document doc;
		doc.SetObject();
		rapidjson::Document::AllocatorType & allocator = doc.GetAllocator();
		doc.AddMember("MsgType", MessageType::eMsg_CHAT_SEND,allocator);
		doc.AddMember("Sender", rapidjson::Value(senderName.c_str(), allocator), allocator);
		//doc.AddMember("Receiver", rapidjson::Value(receiverName.c_str(), allocator), allocator);
		doc.AddMember("Receiver", rapidjson::Value(senderName.c_str(), allocator), allocator);
		doc.AddMember("ChatMsg", rapidjson::Value(msg.c_str(), allocator), allocator);

		rapidjson::StringBuffer buffer;
		rapidjson::Writer<rapidjson::StringBuffer> write(buffer);
		doc.Accept(write);
		WebSocketManager::getInstance()->sendMsg(buffer.GetString());
	}
}

void ChatLayer::menuTalkModeCallback(Ref * pSender)
{

}

void ChatLayer::menuExpressionCallback(Ref * pSender)
{

}

void ChatLayer::menuSearchCallback(Ref * pSender)
{

}

void ChatLayer::textFieldInputEvent(Ref * pSender, TextField::EventType type)
{
	switch (type)
	{
	case TextField::EventType::ATTACH_WITH_IME:
		break;
	case TextField::EventType::DETACH_WITH_IME:
		break;
	case TextField::EventType::INSERT_TEXT:
		break;
	case TextField::EventType::DELETE_BACKWARD:
		break;
	default:
		break;
	}
}

void ChatLayer::createFriendList()
{
	_friendList = ScrollView::create();
	_friendList->setPosition(Vec2(43, 7));
	_friendList->setContentSize(Size(296, 406));
	_friendList->setDirection(ScrollView::Direction::VERTICAL);
	_friendList->setScrollBarEnabled(false);
	_friendList->setBounceEnabled(true);
	this->addChild(_friendList, CHAT_SCROLLVIEW_Z);

	auto scrollLayer = Layer::create();
	scrollLayer->setContentSize(Size(296, 406));
	scrollLayer->setPosition(Vec2(43, 7));
	this->addChild(scrollLayer, CHAT_SCROLL_LAYER_Z);

	auto listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(ChatLayer::friendListTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_2(ChatLayer::friendListTouchMoved, this);
	listener->onTouchEnded = CC_CALLBACK_2(ChatLayer::friendListTouchEnded, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, scrollLayer);

	int itemCount = 15;
	float height = 49 * itemCount + itemCount - 1;
	height = height > 406 ? height : 406;
	_friendList->setInnerContainerSize(Size(296, height));

	float yPosition = height;
	for (int i = 0; i < itemCount;i++)
	{
		yPosition -= 49;
		//std::string name = StringUtils::format("%s", "Test");
		createFriendItem(_friendList, Vec2(0, yPosition), StringUtils::format("%s", "Test"));
		yPosition -= 1;
	}
}

void ChatLayer::createFriendItem(ScrollView * list,const Vec2 &position,const std::string &name)
{
	auto itemNode = Node::create();
	itemNode->setPosition(position);
	itemNode->setContentSize(Size(292, 49));
	itemNode->setAnchorPoint(Vec2(0, 0));
	itemNode->setName(name);
	list->addChild(itemNode);

	auto checkBoxItem = CheckBox::create(
		"menuScene/chatLayer/chat_item_btn0.png",
		"menuScene/chatLayer/chat_item_btn1.png");
	checkBoxItem->setPosition(Vec2(148, 25));
	checkBoxItem->setZoomScale(0);
	checkBoxItem->setName("CheckBox");
	checkBoxItem->addEventListener(CC_CALLBACK_2(ChatLayer::friendItemEvent, this));
	itemNode->addChild(checkBoxItem);

	auto playerItem = MenuItemImage::create(
		"menuScene/chatLayer/chat_player_btn.png",
		"menuScene/chatLayer/chat_player_btn.png",
		CC_CALLBACK_1(ChatLayer::menuPlayerCallback, this));
	playerItem->setPosition(36, 25);
	playerItem->setName("MenuItem");

	auto menu = Menu::create(playerItem, NULL);
	menu->setPosition(Vec2::ZERO);
	menu->setName("Menu");
	itemNode->addChild(menu);

	auto playerName = Label::createWithTTF(name, "fonts/STSONG.TTF", 20);
	playerName->setAnchorPoint(Vec2(0, 0.5));
	playerName->setPosition(68, 25);
	playerName->setColor(Color3B(0, 0, 0));
	itemNode->addChild(playerName);
}

void ChatLayer::createMessageList()
{
	_messageList = ScrollView::create();
	_messageList->setPosition(Vec2(348, 57));
	_messageList->setContentSize(Size(440, 358));
	_messageList->setInnerContainerSize(Size(440, 358));
	_messageList->setDirection(ScrollView::Direction::VERTICAL);
	_messageList->setScrollBarEnabled(false);
	_messageList->setBounceEnabled(true);
	this->addChild(_messageList, CHAT_SCROLLVIEW_Z);

	for (int i = 1; i < 5;i++)
	{
		addMessage(StringUtils::format("%s", "hello"), StringUtils::format("%s", "world"));
	}
}

void ChatLayer::addMessage(const std::string & senderName, const std::string & msg)
{
	_msgCount++;

	auto msgNode = Node::create();
	msgNode->setAnchorPoint(Vec2(0, 0));
	Size innerSize = _messageList->getInnerContainerSize();
	Vec2 nodePosition = Vec2(0, innerSize.height - (_msgCount * 44 + (_msgCount - 1) * 10));
	msgNode->setPosition(nodePosition);
	_messageList->addChild(msgNode);

	auto msgLabel = Label::createWithTTF(msg, "fonts/STSONG.TTF", 18);
	msgLabel->setAnchorPoint(Vec2(0, 0));
	msgLabel->setColor(Color3B(0, 0, 0));
	msgNode->addChild(msgLabel,CHAT_MESSAGE_LABEL_Z);
	Size size = msgLabel->getContentSize();

	

	auto headSprite = Sprite::create("menuScene/chatLayer/chat_player_btn.png");
	msgNode->addChild(headSprite, CHAT_MESSAGE_SPRITE_Z);

	std::string accountName = UserDefault::getInstance()->getStringForKey("AccountName");
	if (accountName != senderName)
	{	
		auto scale9Sprite = Scale9Sprite::create("menuScene/chatLayer/chat_message_sprite0.png");
		scale9Sprite->setAnchorPoint(Vec2(0, 0));
		Rect rect;
		rect.size = Size(167, 22);
		rect.origin = Vec2(17, 2);
		scale9Sprite->setCapInsets(rect);
		scale9Sprite->setContentSize(Size(size.width + 19, size.height + 6));
		msgNode->addChild(scale9Sprite, CHAT_MESSAGE_SPRITE_Z);

		scale9Sprite->setPosition(Vec2(63, 9));
		msgLabel->setPosition(80, 13);
		headSprite->setPosition(37, 21);
	}
	else
	{
		auto scale9Sprite = Scale9Sprite::create("menuScene/chatLayer/chat_message_sprite1.png");
		scale9Sprite->setAnchorPoint(Vec2(0, 0));
		Rect rect;
		rect.size = Size(167, 22);
		rect.origin = Vec2(17, 2);
		scale9Sprite->setCapInsets(rect);
		scale9Sprite->setContentSize(Size(size.width + 19, size.height + 6));
		msgNode->addChild(scale9Sprite, CHAT_MESSAGE_SPRITE_Z);

		float x = 442 - (size.width + 19 + headSprite->getContentSize().width + 17);
		scale9Sprite->setPosition(Vec2(x, 9));
		msgLabel->setPosition(x+7, 13);
		headSprite->setPosition(405, 21);
	}

	Size newSize = Size(innerSize.width, _msgCount * 44 + (_msgCount - 1) * 10);
	newSize.height = newSize.height > 358 ? newSize.height : 358;
	_messageList->setInnerContainerSize(newSize);

	auto childVec = _messageList->getInnerContainer()->getChildren();
	for (int i = 1; i <= _msgCount; i++)
	{
		Vec2 position = Vec2(0, newSize.height - (i * 44 + (i - 1) * 10));
		childVec.at(i-1)->setPosition(position);
	}
}

void ChatLayer::friendItemEvent(Ref * pSender, CheckBox::EventType type)
{
	auto checkBox = dynamic_cast<CheckBox *>(pSender);

	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		if (_selectedFriendItem == nullptr)
		{
			_selectedFriendItem = checkBox;
			Node * parent = _selectedFriendItem->getParent();
			_playerName->setString(parent->getName());
		}
		else if (checkBox != _selectedFriendItem)
		{
			_selectedFriendItem->setSelected(false);
			_selectedFriendItem = checkBox;
			Node * parent = _selectedFriendItem->getParent();
			_playerName->setString(parent->getName());
		}
		break;
	case CheckBox::EventType::UNSELECTED:
		checkBox->setSelected(true);
		break;
	default:
		break;
	}
}

void ChatLayer::menuPlayerCallback(Ref * pSender)
{

}

bool ChatLayer::friendListTouchBegan(Touch * touch, Event * event)
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
		auto container = _friendList->getInnerContainer();
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
					_touchFriendItem = dynamic_cast<CheckBox *>(child->getChildByName("CheckBox"));
					break;
				}
			}
		}

		_friendList->onTouchBegan(touch, event);

		return true;
	}

	return false;
}

void ChatLayer::friendListTouchMoved(Touch * touch, Event * event)
{
	if (_touchFriendItem != nullptr)
	{
		_touchDistance += touch->getDelta().length();
	}

	_friendList->onTouchMoved(touch, event);		//将触摸事件下发给ScrollView
}

void ChatLayer::friendListTouchEnded(Touch * touch, Event * event)
{

	if (_touchFriendItem != nullptr)
	{
		if (_touchDistance <= MIN_TOUCH_DISTANCE)
		{
			_touchFriendItem->onTouchEnded(touch, event);
		}

		_touchDistance = 0;
		_touchFriendItem = nullptr;
	}

	_friendList->onTouchEnded(touch, event);//将触摸事件下发给ScrollView
}

void ChatLayer::receiveMessageEvent(EventCustom * event)
{
	char * msg = (char *)event->getUserData();
	rapidjson::Document doc;
	doc.Parse<0>(msg);
	if (doc.HasParseError())
	{
		log("GetParseError %d\n", doc.GetParseError());
		return;
	}

	if (doc.IsObject())
	{
		std::string senderName = doc["Sender"].GetString();
		std::string msg = doc["ChatMsg"].GetString();
		addMessage(senderName, msg);
	}
}