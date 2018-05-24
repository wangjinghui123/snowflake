#ifndef _TreasureLayer_H_
#define _TreasureLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class TreasureLayer : public Layer{
public:
	TreasureLayer();
	~TreasureLayer();

	virtual bool init();

	CREATE_FUNC(TreasureLayer);

	virtual void onExit();

	virtual void update(float dt);

	void createSpriteGroup();

	void menuUnlockCallback(Ref * pSender);
private:
	Vector<Sprite *> _spriteGroup;
};

#endif