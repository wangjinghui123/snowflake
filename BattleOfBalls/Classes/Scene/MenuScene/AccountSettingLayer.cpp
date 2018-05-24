#include "AccountSettingLayer.h"
#include "LoginLayer.h"
#include "Tools/DropDownList/DropDownList.h"
#include "Tools/CsvUtils/CsvUtils.h"
#include "Header/Common.h"

enum AccountSettingZOrder
{
	ACCOUNT_BACKGROUND_Z,
	ACCOUNT_MENU_Z,
	ACCOUNT_DROPDOWNLIST_Z,
	ACCOUNT_TEXTFIELD_Z,
	ACCOUNT_LAYER_Z
};

AccountSettingLayer::AccountSettingLayer()
{

}

AccountSettingLayer::~AccountSettingLayer()
{
	_eventDispatcher->removeCustomEventListeners("LoginSuccess");
	_eventDispatcher->removeCustomEventListeners("AccountInfoResult");
}

bool AccountSettingLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	auto changePhoneItem = MenuItemImage::create(
		"menuScene/settingLayer/setting_account_changePhone_btn0.png",
		"menuScene/settingLayer/setting_account_changePhone_btn1.png",
		CC_CALLBACK_1(AccountSettingLayer::menuChangePhoneCallback, this));
	changePhoneItem->setPosition(211,92);

	auto modifyNameItem = MenuItemImage::create(
		"menuScene/settingLayer/setting_account_modify_btn0.png",
		"menuScene/settingLayer/setting_account_modify_btn1.png",
		CC_CALLBACK_1(AccountSettingLayer::menuModifyNameCallback, this));
	modifyNameItem->setPosition(687,290);

	auto modifyPasswordItem = MenuItemImage::create(
		"menuScene/settingLayer/setting_account_modify_btn0.png",
		"menuScene/settingLayer/setting_account_modify_btn1.png",
		CC_CALLBACK_1(AccountSettingLayer::menuModifyPasswordCallback, this));
	modifyPasswordItem->setPosition(687,224);

	auto menu = Menu::create(changePhoneItem, modifyNameItem, modifyPasswordItem, NULL);
	menu->setPosition(Vec2::ZERO);
	this->addChild(menu, ACCOUNT_MENU_Z);

	_maleBox = CheckBox::create(
		"menuScene/settingLayer/setting_account_check_btn0.png",
		"menuScene/settingLayer/setting_account_check_btn1.png");
	_maleBox->setPosition(Vec2(441, 79));
	_maleBox->setZoomScale(0);
	_maleBox->addEventListener(CC_CALLBACK_2(AccountSettingLayer::sexChangeCallback, this));
	this->addChild(_maleBox, ACCOUNT_MENU_Z);

	_femaleBox = CheckBox::create(
		"menuScene/settingLayer/setting_account_check_btn0.png",
		"menuScene/settingLayer/setting_account_check_btn1.png");
	_femaleBox->setPosition(Vec2(564, 79));
	_femaleBox->setZoomScale(0);
	_femaleBox->addEventListener(CC_CALLBACK_2(AccountSettingLayer::sexChangeCallback, this));
	this->addChild(_femaleBox, ACCOUNT_MENU_Z);

	auto background = Sprite::create("menuScene/settingLayer/setting_account_background.png");
	background->setPosition(400, 204);
	this->addChild(background, ACCOUNT_BACKGROUND_Z);

	auto ageSprite = Sprite::create("menuScene/settingLayer/setting_account_age_btn.png");
	ageSprite->setPosition(530, 150);
	this->addChild(ageSprite, ACCOUNT_BACKGROUND_Z);

	ValueVector vec;
	vec.push_back(Value(CsvUtils::getInstance()->getMapData(13, 1, "i18n/public.csv")));

	auto dropDonwList = DropDownList::create(
		"menuScene/settingLayer/setting_account_dropdown_top.png",
		"menuScene/settingLayer/setting_account_dropdown_item.png",
		"menuScene/settingLayer/setting_account_dropdown_scale9.png",
		vec);
	dropDonwList->setPosition(206, 150);
	dropDonwList->setCallback(CC_CALLBACK_1(AccountSettingLayer::menuSwitchAccountCallback, this));
	this->addChild(dropDonwList, ACCOUNT_DROPDOWNLIST_Z);


	_textFieldName = TextField::create("", "fonts/STSONG.TTF", 18);
	_textFieldName->ignoreContentAdaptWithSize(false);
	_textFieldName->setContentSize(Size(220, 32));
	_textFieldName->setMaxLength(12);
	_textFieldName->setMaxLengthEnabled(true);
	_textFieldName->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldName->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldName->setPosition(Vec2(530, 290));
	_textFieldName->setTouchEnabled(false);
	_textFieldName->addEventListener(CC_CALLBACK_2(AccountSettingLayer::textFieldNameEvent, this));
	this->addChild(_textFieldName, ACCOUNT_TEXTFIELD_Z);


	_textFieldPassword = TextField::create("", "fonts/STSONG.TTF", 18);
	_textFieldPassword->setPasswordEnabled(true);
	_textFieldPassword->setPasswordStyleText("*");
	_textFieldPassword->ignoreContentAdaptWithSize(false);
	_textFieldPassword->setContentSize(Size(220, 32));
	_textFieldPassword->setMaxLength(16);
	_textFieldPassword->setMaxLengthEnabled(true);
	_textFieldPassword->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldPassword->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldPassword->setPosition(Vec2(530, 224));
	_textFieldPassword->setTouchEnabled(false);
	_textFieldPassword->addEventListener(CC_CALLBACK_2(AccountSettingLayer::textFieldPasswordEvent, this));
	this->addChild(_textFieldPassword, ACCOUNT_TEXTFIELD_Z);

	_textFieldAge = TextField::create("", "fonts/STSONG.TTF", 18);
	_textFieldAge->ignoreContentAdaptWithSize(false);
	_textFieldAge->setContentSize(Size(220, 32));
	_textFieldAge->setMaxLength(3);
	_textFieldAge->setMaxLengthEnabled(true);
	_textFieldAge->setTextHorizontalAlignment(TextHAlignment::LEFT);
	_textFieldAge->setTextVerticalAlignment(TextVAlignment::CENTER);
	_textFieldAge->setPosition(Vec2(530, 150));
	_textFieldAge->setTouchEnabled(true);
	_textFieldAge->addEventListener(CC_CALLBACK_2(AccountSettingLayer::textFieldAgeEvent, this));
	this->addChild(_textFieldAge, ACCOUNT_TEXTFIELD_Z);

	_eventDispatcher->addCustomEventListener("LoginSuccess", CC_CALLBACK_1(AccountSettingLayer::loginSuccessEvent,this));
	_eventDispatcher->addCustomEventListener("AccountInfoResult", CC_CALLBACK_1(AccountSettingLayer::accountInfoResult, this));

	return true;
}

void AccountSettingLayer::menuChangePhoneCallback(Ref * pSender)
{

}

void AccountSettingLayer::menuModifyNameCallback(Ref * pSender)
{

}

void AccountSettingLayer::menuModifyPasswordCallback(Ref * pSender)
{

}

void AccountSettingLayer::sexChangeCallback(Ref * pSender, CheckBox::EventType type)
{
	auto currentBox = dynamic_cast<CheckBox *>(pSender);
	switch (type)
	{
	case CheckBox::EventType::SELECTED:
		if (currentBox == _maleBox)
		{
			_femaleBox->setSelected(false);
		}
		else
		{
			_maleBox->setSelected(false);
		}
		break;
	case CheckBox::EventType::UNSELECTED:
		if (currentBox == _maleBox)
		{
			_femaleBox->setSelected(true);
		}
		else
		{
			_maleBox->setSelected(true);
		}
		break;
	default:
		break;
	}
}


void AccountSettingLayer::menuSwitchAccountCallback(int index)
{
	auto loginLayer = LoginLayer::create();
	this->addChild(loginLayer, ACCOUNT_LAYER_Z);
}

void AccountSettingLayer::textFieldNameEvent(Ref * pSender, TextField::EventType type)
{

}

void AccountSettingLayer::textFieldPasswordEvent(Ref * pSender, TextField::EventType type)
{

}

void AccountSettingLayer::textFieldAgeEvent(Ref * pSender, TextField::EventType type)
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

void AccountSettingLayer::loginSuccessEvent(EventCustom * event)
{

	std::string name = WebSocketManager::getInstance()->getAccountName();
	if (name != "")
	{
		rapidjson::Document doc;
		doc.SetObject();
		rapidjson::Document::AllocatorType & allocator = doc.GetAllocator();
		doc.AddMember("MsgType", MessageType::eMsg_ACCOUNT_INFO, allocator);
		doc.AddMember("Name", rapidjson::Value(name.c_str(), allocator), allocator);

		rapidjson::StringBuffer buffer;
		rapidjson::Writer<rapidjson::StringBuffer> write(buffer);
		doc.Accept(write);

		std::string msg = buffer.GetString();
		WebSocketManager::getInstance()->sendMsg(msg);
	}
	
}

void AccountSettingLayer::accountInfoResult(EventCustom * event)
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
		std::string name = doc["Data"]["Name"].GetString();
		std::string password = doc["Data"]["Password"].GetString();
		int age = doc["Data"]["Age"].GetInt();
		int sex = doc["Data"]["Sex"].GetInt();

		_textFieldName->setString(name);
		_textFieldPassword->setString(password);
		_textFieldAge->setString(StringUtils::format("%d",age));
		if (sex)
		{
			_maleBox->setSelected(false);
			_femaleBox->setSelected(true);
		}
		else
		{
			_maleBox->setSelected(true);
			_femaleBox->setSelected(false);
		}
	}
}