#ifndef _MessageLayer_H_
#define _MessageLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class MessageLayer : public Layer{
public:
	MessageLayer();
	~MessageLayer();

	virtual bool init();

	CREATE_FUNC(MessageLayer);

	void menuInputCallback(Ref * pSender);
	void menuSendCallback(Ref * pSender);
	void menuTopCallback(Ref * pSender);
	void menuQuestionCallback(Ref * pSender);

	void createMessageList();
private:
	ScrollView * _messageList;
};

#endif