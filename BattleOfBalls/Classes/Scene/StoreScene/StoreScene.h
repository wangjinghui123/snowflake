#ifndef _StoreScene_H_
#define _StoreScene_H_

#include "cocos2d.h"
USING_NS_CC;

class StoreScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(StoreScene);

	/*菜单回调函数*/
	void menuTreasureCallback(Ref * pSender);		//宝箱按钮回调

	void menuSkinCallback(Ref * pSender);		//皮肤按钮回调

	void menuVestmentCallback(Ref * pSender);		//圣衣按钮回调

	void menuMemberCallback(Ref * pSender);		//会员按钮回调

	void menuBeanCallback(Ref * pSender);		//彩豆按钮回调

	void menuLollyCallback(Ref * pSender);		//棒棒糖按钮回调
	
	void menuMushroomCallback(Ref * pSender);		//金蘑菇按钮回调

	void menuMagicCallback(Ref * pSender);		//魔法屋按钮回调

	void menuReturnCallback(Ref * pSender);		//返回按钮回调

private:
	MenuItemImage * _treasureItem1, *_treasureItem2;
	MenuItemImage * _skinItem1, *_skinItem2;
	MenuItemImage *_vestmentItem1, *_vestmentItem2;
	MenuItemImage *_memberItem1, *_memberItem2;
	
	Layer * _currentLayer;		//当前激活图层
};
#endif