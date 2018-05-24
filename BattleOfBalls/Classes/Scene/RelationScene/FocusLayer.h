#ifndef _FocusLayer_H_
#define _FocusLayer_H_

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

class FocusLayer : public Layer{
public:
	FocusLayer();
	~FocusLayer();

	virtual bool init();

	CREATE_FUNC(FocusLayer);

	void menuSearchCallback(Ref * pSender);
	void menuSortCallback(Ref * pSender);
	void menuPreviousCallback(Ref * pSender);
	void menuNextCallback(Ref * pSender);
	void menuFocusLimitCallback(Ref * pSender, CheckBox::EventType type);
	void menuItemExtendCallback(Ref * pSender);
	void menuPlayerCallback(Ref * pSender);

	void createFriendList();
	Node * createListItem(int index,Map<std::string, Ref *> & info);
private:
	Vector<Node *> _list;
	Label * _pageLabel;
	int _pageIndex;
	int _pageMax;
};

#endif