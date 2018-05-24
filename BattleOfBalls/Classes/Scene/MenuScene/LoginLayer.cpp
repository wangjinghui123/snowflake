#include "LoginLayer.h"
#include "Tools/CsvUtils/CsvUtils.h"
#include "Tools/PromptBox/PromptBox.h"
#include "Tools/WebSocketManager/WebSocketManager.h"
#include "json/document.h"
#include "json/stringbuffer.h"
#include "json/writer.h"
#include "Header/MessageType.h"

using namespace rapidjson;
enum LoginZOrder
{
	LOGIN_MASKLAYER_Z,
	LOGIN_BACKGROUND_Z,
	LOGIN_SPRITE_Z,
	LOGIN_MENU_Z,
	LOGIN_TEXTFIELD_Z,
	LOGIN_LABEL_Z,
	LOGIN_PROMPT_Z
};

LoginLayer::LoginLayer()
{

}

LoginLayer::~LoginLayer()
{
	PromptBox::getInstance()->clearList();
	_eventDispatcher->removeCustomEventListeners("LoginResult");
}

bool LoginLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);

	listener->onTouchBegan = CC_CALLBACK_2(LoginLayer::onTouchBegan, this);
	listener->onTouchMoved = CC_CALLBACK_2(LoginLayer::onTouchMoved, this);
	listener->onTouchEnded = CC_CALLBACK_2(LoginLayer::onTouchEnded, this);

	_eventDispatcher->addEventListenerWithSceneGraphPriority(listener, this);


	_textFieldName = TextField::create("", "fonts/STSONG.TTF", 18);
	_textFieldName->ignoreContentAdaptWithSize(false);
	_textFieldName->setContentSize(Size(250, 33));
	_textFieldName->setMaxLength(12);
	_textFieldName->setMaxLengthEnabled(true);
	_textFieldName->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldName->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldName->setPosition(Vec2(400, 301));
	
	_textFieldName->addEventListener(CC_CALLBACK_2(LoginLayer::textFieldNameEvent, this));
	this->addChild(_textFieldName, LOGIN_TEXTFIELD_Z);

	
	_textFieldPassword = TextField::create("", "fonts/STSONG.TTF", 18);
	_textFieldPassword->setPasswordEnabled(true);
	_textFieldPassword->setPasswordStyleText("*");
	_textFieldPassword->ignoreContentAdaptWithSize(false);
	_textFieldPassword->setContentSize(Size(250, 33));
	_textFieldPassword->setMaxLength(16);
	_textFieldPassword->setMaxLengthEnabled(true);
	_textFieldPassword->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldPassword->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldPassword->setPosition(Vec2(400, 239));
	_textFieldPassword->addEventListener(CC_CALLBACK_2(LoginLayer::textFieldPasswordEvent, this));
	this->addChild(_textFieldPassword, LOGIN_TEXTFIELD_Z);

	auto loginItem = MenuItemImage::create(
		"menuScene/settingLayer/login_login_btn0.png",
		"menuScene/settingLayer/login_login_btn1.png",
		CC_CALLBACK_1(LoginLayer::menuLoginCallback, this));
	loginItem->setPosition(400, 173);

	auto forgetItem = MenuItemImage::create(
		"menuScene/settingLayer/login_forget_btn.png",
		"menuScene/settingLayer/login_forget_btn.png",
		CC_CALLBACK_1(LoginLayer::menuForgetCallback, this));
	forgetItem->setPosition(456, 113);

	auto menu = Menu::create(loginItem, forgetItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu,LOGIN_MENU_Z);

	auto nameSprite = Sprite::create("menuScene/settingLayer/login_input_btn.png");
	nameSprite->setPosition(400, 301);
	this->addChild(nameSprite, LOGIN_SPRITE_Z);

	auto passwordSprite = Sprite::create("menuScene/settingLayer/login_input_btn.png");
	passwordSprite->setPosition(400, 239);
	this->addChild(passwordSprite, LOGIN_SPRITE_Z);

	auto label1 = Sprite::create("menuScene/settingLayer/login_label_account.png");
	label1->setPosition(287, 330);
	this->addChild(label1,LOGIN_LABEL_Z);

	auto label2 = Sprite::create("menuScene/settingLayer/login_label_password.png");
	label2->setPosition(287, 267);
	this->addChild(label2, LOGIN_LABEL_Z);

	_background = Sprite::create("menuScene/settingLayer/login_background.png");
	_background->setPosition(400, 225);
	this->addChild(_background, LOGIN_BACKGROUND_Z);	

	this->setScale(0);
	this->runAction(Sequence::create(
		ScaleTo::create(0.1f, 1),
		CallFunc::create(CC_CALLBACK_0(LoginLayer::addMask,this)),
		NULL));

	_eventDispatcher->addCustomEventListener("LoginResult", CC_CALLBACK_1(LoginLayer::loginResultEvent, this));

	return true;
}

void LoginLayer::textFieldNameEvent(Ref * pSender, TextField::EventType type)
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

void LoginLayer::textFieldPasswordEvent(Ref * pSender, TextField::EventType type)
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

void LoginLayer::menuLoginCallback(Ref * pSender)
{
	std::string nameStr = _textFieldName->getString();
	std::string passwordStr = _textFieldPassword->getString();
	std::string loginName = WebSocketManager::getInstance()->getAccountName();
	if (loginName != "" && loginName == nameStr)
	{
		std::string text = CsvUtils::getInstance()->getMapData(19, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, LOGIN_PROMPT_Z);
		}
		return;
	}

	if (nameStr == "")
	{
		std::string text = CsvUtils::getInstance()->getMapData(14, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, LOGIN_PROMPT_Z);
		}
		return;
	}

	if (passwordStr=="")
	{
		std::string text = CsvUtils::getInstance()->getMapData(15, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, LOGIN_PROMPT_Z);
		}
		return;
	}

	char * name = new char[20];
	strcpy(name, nameStr.c_str());

	rapidjson::Document doc;
	doc.SetObject();
	rapidjson::Document::AllocatorType & allocator = doc.GetAllocator();
	rapidjson::Value obj(rapidjson::kObjectType);
	
	obj.AddMember("Name", rapidjson::Value(nameStr.c_str(),allocator), allocator);
	obj.AddMember("Password", rapidjson::Value(passwordStr.c_str(),allocator),allocator);
	doc.AddMember("MsgType", MessageType::eMsg_LOGIN, allocator);
	doc.AddMember("Data", obj, allocator);

	rapidjson::StringBuffer buffer;
	rapidjson::Writer<StringBuffer> write(buffer);
	doc.Accept(write);
	
	std::string msg = buffer.GetString();
	WebSocketManager::getInstance()->sendMsg(msg);
}

void LoginLayer::menuForgetCallback(Ref * pSender)
{

}

bool LoginLayer::onTouchBegan(Touch * touch, Event * event)
{
	return true;
}

void LoginLayer::onTouchMoved(Touch * touch, Event * event)
{

}

void LoginLayer::onTouchEnded(Touch * touch, Event * event)
{
	Vec2 position = touch->getLocation();
	position = _background->convertToNodeSpace(position);

	Rect rect;
	rect.size = _background->getContentSize();
	if (!rect.containsPoint(position))
	{
		this->removeFromParentAndCleanup(true);
	}
}

void LoginLayer::addMask()
{
	auto maskLayer = LayerColor::create(Color4B(0, 0, 0, 76), 800, 450);
	this->addChild(maskLayer, LOGIN_MASKLAYER_Z);
}

void LoginLayer::loginResultEvent(EventCustom * event)
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
		int nameResult = doc["NameResult"].GetInt();
		

		if (!nameResult)
		{
			std::string text = CsvUtils::getInstance()->getMapData(16,1, "i18n/public.csv");
			auto prompt = PromptBox::getInstance()->createPrompt(text);
			if (prompt)
			{
				this->addChild(prompt, LOGIN_PROMPT_Z);
			}
			return;
		}

		int passwordResult = doc["PasswordResult"].GetInt();

		if (!passwordResult)
		{
			std::string text = CsvUtils::getInstance()->getMapData(17, 1, "i18n/public.csv");
			auto prompt = PromptBox::getInstance()->createPrompt(text);
			if (prompt)
			{
				this->addChild(prompt, LOGIN_PROMPT_Z);
			}
			return;
		}

		std::string text = CsvUtils::getInstance()->getMapData(18, 1, "i18n/public.csv");
		auto prompt = PromptBox::getInstance()->createPrompt(text);
		if (prompt)
		{
			this->addChild(prompt, LOGIN_PROMPT_Z);
		}

		WebSocketManager::getInstance()->setAccountName(_textFieldName->getString());

		UserDefault::getInstance()->setBoolForKey("Login", true);
		UserDefault::getInstance()->setStringForKey("AccountName", _textFieldName->getString());
		UserDefault::getInstance()->setStringForKey("AccountPassword", _textFieldPassword->getString());
		_eventDispatcher->dispatchCustomEvent("LoginSuccess", NULL);
	}
}