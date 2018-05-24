#ifndef _SkinLayer_H_
#define _SkinLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class SkinLayer : public Layer{
public:
	SkinLayer();
	~SkinLayer();

	virtual bool init();

	CREATE_FUNC(SkinLayer);

	bool onTouchBegan(Touch * touch, Event * event);
	void onTouchMoved(Touch * touch, Event * event);
	void onTouchEnded(Touch * touch, Event * event);

	/*�˵��ص�����*/
	void menuHaloCallback(Ref * pSender, CheckBox::EventType type);
	void menuSporeCallback(Ref * pSender, CheckBox::EventType type);
	void menuShadowCallback(Ref * pSender, CheckBox::EventType type);
	void menuKeywordCallback(Ref * pSender, CheckBox::EventType type);

	void menuSortCallback(Ref * pSender);
	void menuBuyCallback(Ref * pSender);
	void menuPriceCallback(Ref * pSender);

	/*������ͼ��*/
	void createHaloLayer();
	void createSporeLayer();
	void createShadowLayer();
	void createKeywordLayer();

	void createListItem(ScrollView * scrollView, Vec2 & position, int id);

private:
	Layer * _haloLayer;		//�⻷�б�
	Layer * _sporeLayer;		//�����б�
	Layer * _shadowLayer;		//��Ӱ�б�
	Layer * _keywordLayer;		//�ؼ����б�
	Layer * _currentLayer;		//��ǰ�����б�
	ScrollView * _haloList;		
	Vec2 _list1Position;
	Menu * _touchMenu;
	float _touchDistance;
};

#endif