#include "MessageLayer.h"

enum MessageZOrder
{
	MESSAGE_BACKGROUND_Z,
	MESSAGE_SPRITE_Z,
	MESSAGE_NODE_Z,
	MESSAGE_MENU
};

MessageLayer::MessageLayer()
{

}

MessageLayer::~MessageLayer()
{

}

bool MessageLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto inputItem = MenuItemImage::create(
		"playerScene/message/player_message_input.png",
		"playerScene/message/player_message_input.png",
		CC_CALLBACK_1(MessageLayer::menuInputCallback, this));
	inputItem->setPosition(415,348);

	auto sendItem = MenuItemImage::create(
		"playerScene/message/player_message_send_btn0.png",
		"playerScene/message/player_message_send_btn1.png",
		CC_CALLBACK_1(MessageLayer::menuSendCallback, this));
	sendItem->setPosition(674,348);

	auto topItem = MenuItemImage::create(
		"playerScene/message/player_message_top.png",
		"playerScene/message/player_message_top.png",
		CC_CALLBACK_1(MessageLayer::menuTopCallback, this));
	topItem->setPosition(739,348);

	auto questionItem = MenuItemImage::create(
		"playerScene/message/player_message_question.png",
		"playerScene/message/player_message_question.png",
		CC_CALLBACK_1(MessageLayer::menuQuestionCallback, this));
	questionItem->setPosition(744,390);

	auto menu = Menu::create(inputItem, sendItem, topItem, questionItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, MESSAGE_MENU);

	auto background = Sprite::create("playerScene/message/player_message_background.png");
	background->setPosition(426, 201);
	this->addChild(background, MESSAGE_BACKGROUND_Z);

	auto top = Sprite::create("playerScene/message/player_message_item1.png");
	top->setPosition(426,390);
	this->addChild(top, MESSAGE_SPRITE_Z);

	createMessageList();

	return true;
}

void MessageLayer::menuInputCallback(Ref * pSender)
{

}

void MessageLayer::menuSendCallback(Ref * pSender)
{

}

void MessageLayer::menuTopCallback(Ref * pSender)
{

}

void MessageLayer::menuQuestionCallback(Ref * pSender)
{

}

void MessageLayer::createMessageList()
{
	_messageList = ScrollView::create();
	_messageList->setPosition(Vec2(86,35));
	_messageList->setDirection(ScrollView::Direction::VERTICAL);
	_messageList->setScrollBarEnabled(false);
	_messageList->setContentSize(Size(678,296));
	_messageList->setInnerContainerSize(Size(678,500));
	this->addChild(_messageList, MESSAGE_NODE_Z);

	auto test = Sprite::create("playerScene/message/picture.jpg");
	test->setPosition(389, 148);
	_messageList->addChild(test);

	
}