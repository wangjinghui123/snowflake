#include "RivalLayer.h"

RivalLayer::RivalLayer()
{

}

RivalLayer::~RivalLayer()
{

}

bool RivalLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}