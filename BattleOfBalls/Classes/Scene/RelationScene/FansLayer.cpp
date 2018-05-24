#include "FansLayer.h"

FansLayer::FansLayer()
{

}

FansLayer::~FansLayer()
{

}

bool FansLayer::init()
{
	if (!Layer::init())
	{
		return false;
	}

	return true;
}