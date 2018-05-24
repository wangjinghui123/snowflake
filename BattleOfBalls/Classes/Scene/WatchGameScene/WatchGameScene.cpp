#include "WatchGameScene.h"

Scene * WatchGameScene::createScene()
{
	auto scene = Scene::create();

	auto layer = WatchGameScene::create();

	scene->addChild(layer);

	return scene;
}

bool WatchGameScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}