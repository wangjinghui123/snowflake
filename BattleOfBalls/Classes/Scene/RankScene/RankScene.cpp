#include "RankScene.h"

Scene * RankScene::createScene()
{
	auto scene = Scene::create();

	auto layer = RankScene::create();

	scene->addChild(layer);

	return scene;
}

bool RankScene::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}