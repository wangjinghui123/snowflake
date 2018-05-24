#include "TeamModeLayer.h"

TeamModeLayer::TeamModeLayer()
{

}

TeamModeLayer::~TeamModeLayer()
{

}

bool TeamModeLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}