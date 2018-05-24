#include "CustomModeLayer.h"

CustomModeLayer::CustomModeLayer()
{

}

CustomModeLayer::~CustomModeLayer()
{

}

bool CustomModeLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}