#include "EnterScene.h"
#include "../SceneManager.h"

Scene * EnterScene::createScene()
{
	auto scene = Scene::create();

	auto layer = EnterScene::create();

	scene->addChild(layer);

	return scene;
}

EnterScene::EnterScene()
{
	_timer = NULL;
	_loadingLabel = NULL;
}

EnterScene::~EnterScene()
{

}

bool EnterScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	Size size = CCDirector::getInstance()->getVisibleSize();

	auto title = Sprite::create("enterScene/enter_background0.png");
	title->setPosition(Vec2(size.width / 2, size.height / 2));
	title->setOpacity(0);

	auto fadeIn = FadeIn::create(0.5);
	auto fadeOut = FadeOut::create(1.0);
	auto callFunc = CallFunc::create(CC_CALLBACK_0(EnterScene::startLoding, this));
	auto seq = Sequence::create(fadeIn, fadeOut, callFunc, NULL);

	title->runAction(seq);
	this->addChild(title);

	UserDefault::getInstance()->setBoolForKey("AutoLogin", false);
	if (!UserDefault::getInstance()->getBoolForKey("AutoLogin"))
	{
		UserDefault::getInstance()->setBoolForKey("Login", false);
		UserDefault::getInstance()->setStringForKey("AccountName", "");
		UserDefault::getInstance()->setStringForKey("AccountPassword", "");
	}

	return true;
}

void EnterScene::startLoding()
{
	auto size = CCDirector::getInstance()->getVisibleSize();
	auto background = Sprite::create("enterScene/enter_background1.png");
	background->setPosition(size.width/2,size.height/2);
	this->addChild(background);

	auto loadingBackground = Sprite::create("enterScene/enter_loadingBar0.png");
	loadingBackground->setPosition(400,40);
	this->addChild(loadingBackground);

	_loadingLabel = Label::createWithTTF("loding:0%%","fonts/arial.ttf",20);
	_loadingLabel->setColor(Color3B(0, 0, 0));
	_loadingLabel->setPosition(400,55);
	this->addChild(_loadingLabel);

	auto loadingSprite = Sprite::create("enterScene/enter_loadingBar1.png");
	_timer = ProgressTimer::create(loadingSprite);
	_timer->setPosition(400,40);
	_timer->setType(ProgressTimerType::BAR);
	_timer->setMidpoint(Vec2(0,1));
	_timer->setBarChangeRate(Vec2(1,0));
	_timer->setPercentage(0);
	this->addChild(_timer);

	SpriteFrameCache::getInstance()->addSpriteFramesWithFile("public/bean.plist", "public/bean.png");
	//CCTextureCache::sharedTextureCache()->addImageAsync("ball.png",this,callfuncO_selector(EnterScene::loadingCallback));
	//CCTextureCache::sharedTextureCache()->addImageAsync("ball.png",this,callfuncO_selector(EnterScene::plistLoadingCallback));

	this->scheduleUpdate();
}

void EnterScene::update(float delta)
{
	float percent = _timer->getPercentage();
	percent+=0.5;
	_timer->setPercentage(percent);

	char str[50];
	sprintf(str, "loading:%d%%", int(percent));
	//CCString * str = CCString::createWithFormat("loading:%d%%",int(percent));
	_loadingLabel->setString(str);

	if(percent>=100)
		this->scheduleOnce(schedule_selector(EnterScene::enterMenuScene),1);
}

void EnterScene::enterMenuScene(float dt)
{
	SceneManager::getInstance()->changeScene(SceneManager::en_MenuScene);
}

void EnterScene::imageLoadingCallback(Ref * pSender)
{

}

void EnterScene::plistLoadingCallback(Ref * pSender)
{
	//CCTexture2D * texture = dynamic_cast<CCTexture2D *>(pSender);
	//CCSpriteFrameCache::sharedSpriteFrameCache()->addSpriteFramesWithFile("ball.plist",texture);
}

void EnterScene::loading(float dt)
{

}

void EnterScene::onExit()
{
	this->unscheduleAllCallbacks();
	Layer::onExit();
	
}