#ifndef _PictureLayer_H_
#define _PictureLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class PictureLayer : public Layer{
public:
	PictureLayer();
	~PictureLayer();

	virtual bool init();

	CREATE_FUNC(PictureLayer);

	void menuAddCallback(Ref * pSender);
	void menuDeleteCallback(Ref * pSender);
private:
	ScrollView * _pictureView;
};

#endif