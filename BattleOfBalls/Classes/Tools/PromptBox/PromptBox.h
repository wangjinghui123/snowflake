#ifndef __PromptBox_H__
#define __PromptBox_H__

#include "cocos2d.h"
#include "ui/CocosGUI.h"

USING_NS_CC;
using namespace ui;

/*����ҵ�ϵͳ��ʾ��*/

class PromptBox : public Ref{
public:
	PromptBox();
	~PromptBox();

	static PromptBox * getInstance();

	virtual bool init();

	Node * createPrompt(const std::string & msg);		//������ʾ�򣬲�������ʾ����
	void removePrompt(Ref * pSender);
	void clearList();
private:
	static PromptBox * s_PromptBox;
	Vector<Node *> _promptList;
};

#endif