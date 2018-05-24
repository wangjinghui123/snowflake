#ifndef _GetLollyLayer_H_
#define _GetLollyLayer_H_

#include "cocos2d.h"

USING_NS_CC;

class GetLollyLayer : public Layer{
public:
	GetLollyLayer();
	~GetLollyLayer();

	virtual bool init();

	CREATE_FUNC(GetLollyLayer);

	/*ͼ�������а�ť�Ļص�����*/
	void menuGetLollyCallback(Ref * pSender);

	void menuWeiXinCallback(Ref * pSender);

	void menuMiaoLingCallback(Ref * pSender);

	void menuSaveCallback(Ref * pSender);

	void menuCopyCallback(Ref * pSender);

	void menuLingQuCallback(Ref * pSender);

	void menuCloseCallback(Ref * pSender);

	void createLollyLayer();	//������ȡ������ͼ��
	void createWeiXinLayer();		//����΢�ŷ���ͼ��
private:
	MenuItemImage * _getLollyItem1, *_getLollyItem2;
	MenuItemImage * _weiXinItem1, *_weiXinItem2;
};

#endif