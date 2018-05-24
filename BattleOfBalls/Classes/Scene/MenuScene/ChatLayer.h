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

	/*处理触摸逻辑*/
	bool friendListTouchBegan(Touch * touch, Event * event);
	void friendListTouchMoved(Touch * touch, Event * event);
	void friendListTouchEnded(Touch * touch, Event * event);

	/*图层中所有按钮的回调函数*/
	void menuSendCallback(Ref * pSender);

	void menuSearchCallback(Ref * pSender);

	void menuTalkModeCallback(Ref * pSender);

	void menuExpressionCallback(Ref * pSender);

	
	void textFieldInputEvent(Ref * pSender, TextField::EventType type);		//聊天内容输入框回调

	void createFriendList();		//创建左边好友列表
	void createFriendItem(ScrollView * list,const Vec2 &position,const std::string &name);  //创建好友列表项
	void friendItemEvent(Ref * pSender, CheckBox::EventType type);		//好友项点击回调
	void menuPlayerCallback(Ref * pSender);		//好友具体信息回调

	void createMessageList();		//创建聊天记录列表

	void receiveMessageEvent(EventCustom * event);		//接收他人聊天信息
	void addMessage(const std::string & senderName,const std::string & msg);		//向聊天记录列表添加项
private:
	TextField * _textFieldInput;
	Sprite * _searchSprite;
	ScrollView * _friendList;		//好友列表
	ScrollView * _messageList;		//聊天记录列表
	Label * _playerName;

	float _touchDistance;		//触摸移动距离
	CheckBox * _selectedFriendItem;
	CheckBox * _touchFriendItem;

	int _msgCount;
};

#endif