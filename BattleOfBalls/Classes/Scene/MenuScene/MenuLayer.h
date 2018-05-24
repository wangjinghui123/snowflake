#ifndef _MenuLayer_H_
#define _MenuLayer_H_

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

class MenuLayer : public Layer{
public:
	MenuLayer();
	~MenuLayer();

	virtual bool init();

	CREATE_FUNC(MenuLayer);

	virtual void onExit();

	/*��������İ�ťʹ�õ���MenuItemImage��������ְ�ť����δʵ��*/

	void menuFindFriendCallback(Ref * pSender);  //�����Ѱ�ť�ص�

	void menuExtendCallback(Ref * pSender);		//�ӺŰ�ť�ص�

	void menuSignCallback(Ref * pSender);		//ǩ����ť�ص�

	void menuSettingCallback(Ref * pSender);		//���ð�ť�ص�

	void menuWatchGameCallback(Ref * pSender);		//�ۿ�������ť�ص�

	void menuGetLollyCallback(Ref * pSender);		//��ȡ�����ǰ�ť�ص�

	void menuStoryCallback(Ref * pSender);			//������°�ť�ص�

	void menuStrategyCallback(Ref * pSender);		//���԰�ť�ص�

	void menuTeamCallback(Ref * pSender);			//ս�Ӱ�ť�ص�

	void menuRelationCallback(Ref * pSender);		//��ϵ��ť�ص�

	void menuRankCallback(Ref * pSender);			//���а�ť�ص�

	void menuStoreCallback(Ref * pSender);			//�̵갴ť�ص�

	void menuPlayerCallback(Ref * pSender);			//���ͷ��ť�ص�

	void menuStartCallback(Ref * pSender);			//��ʼ������ť�ص�

	void menuSurvivalModeCallback(Ref * pSender);	//����ģʽ��ť�ص�

	void menuTeamModeCallback(Ref * pSender);		//��սģʽ��ť�ص�

	void menuCustomModeCallback(Ref * pSender);		//�Զ���ģʽ��ť�ص�

	void menuResetNameCallback(Ref * pSender);		//�������������ť


	void setMenuEnable(Ref * pSender);		//���õ����ĸ���ť�Ƿ�ɴ���

	void menuServerCallback(int index);		//�����������˵��ص�

	void gameStartEvent(EventCustom * event);		//����������·��Ŀ�ʼ��Ϸ����

	void gameNameEvent(Ref * pSender, TextField::EventType type);  //�����ؼ��ص�
private:
	MenuItemImage * _extendItem1;		//�ӺŰ�ť
	MenuItemImage * _extendItem2;		//���Ű�ť
	Vector<Menu *> _menuList;			//��Ŵ��������а�ť

	TextField * _gameName;				//��Ϸ�������
};

#endif