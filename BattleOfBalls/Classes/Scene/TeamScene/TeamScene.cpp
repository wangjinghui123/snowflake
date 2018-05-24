#include "TeamScene.h"

Scene * TeamScene::createScene()
{
	auto scene = Scene::create();

	auto layer = TeamScene::create();

	scene->addChild(layer);

	return scene;
}

bool TeamScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}