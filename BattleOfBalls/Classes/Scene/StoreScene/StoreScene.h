#ifndef _StoreScene_H_
#define _StoreScene_H_

#include "cocos2d.h"
USING_NS_CC;

class StoreScene : public Layer{
public:
	static Scene * createScene();

	virtual bool init();

	CREATE_FUNC(StoreScene);

	/*�˵��ص�����*/
	void menuTreasureCallback(Ref * pSender);		//���䰴ť�ص�

	void menuSkinCallback(Ref * pSender);		//Ƥ����ť�ص�

	void menuVestmentCallback(Ref * pSender);		//ʥ�°�ť�ص�

	void menuMemberCallback(Ref * pSender);		//��Ա��ť�ص�

	void menuBeanCallback(Ref * pSender);		//�ʶ���ť�ص�

	void menuLollyCallback(Ref * pSender);		//�����ǰ�ť�ص�
	
	void menuMushroomCallback(Ref * pSender);		//��Ģ����ť�ص�

	void menuMagicCallback(Ref * pSender);		//ħ���ݰ�ť�ص�

	void menuReturnCallback(Ref * pSender);		//���ذ�ť�ص�

private:
	MenuItemImage * _treasureItem1, *_treasureItem2;
	MenuItemImage * _skinItem1, *_skinItem2;
	MenuItemImage *_vestmentItem1, *_vestmentItem2;
	MenuItemImage *_memberItem1, *_memberItem2;
	
	Layer * _currentLayer;		//��ǰ����ͼ��
};
#endif