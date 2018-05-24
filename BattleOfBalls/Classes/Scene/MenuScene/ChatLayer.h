#ifndef _ChatLayer_H_
#define _ChatLayer_H_

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

class ChatLayer : public Layer{
public:
	ChatLayer();
	~ChatLayer();

	virtual bool init();

	CREATE_FUNC(ChatLayer);

	/*�������߼�*/
	bool friendListTouchBegan(Touch * touch, Event * event);
	void friendListTouchMoved(Touch * touch, Event * event);
	void friendListTouchEnded(Touch * touch, Event * event);

	/*ͼ�������а�ť�Ļص�����*/
	void menuSendCallback(Ref * pSender);

	void menuSearchCallback(Ref * pSender);

	void menuTalkModeCallback(Ref * pSender);

	void menuExpressionCallback(Ref * pSender);

	
	void textFieldInputEvent(Ref * pSender, TextField::EventType type);		//�������������ص�

	void createFriendList();		//������ߺ����б�
	void createFriendItem(ScrollView * list,const Vec2 &position,const std::string &name);  //���������б���
	void friendItemEvent(Ref * pSender, CheckBox::EventType type);		//���������ص�
	void menuPlayerCallback(Ref * pSender);		//���Ѿ�����Ϣ�ص�

	void createMessageList();		//���������¼�б�

	void receiveMessageEvent(EventCustom * event);		//��������������Ϣ
	void addMessage(const std::string & senderName,const std::string & msg);		//�������¼�б������
private:
	TextField * _textFieldInput;
	Sprite * _searchSprite;
	ScrollView * _friendList;		//�����б�
	ScrollView * _messageList;		//�����¼�б�
	Label * _playerName;

	float _touchDistance;		//�����ƶ�����
	CheckBox * _selectedFriendItem;
	CheckBox * _touchFriendItem;

	int _msgCount;
};

#endif