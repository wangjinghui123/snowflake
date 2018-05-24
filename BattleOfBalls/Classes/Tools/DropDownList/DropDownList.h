#ifndef __DropDownList_H__
#define __DropDownList_H__

#include "cocos2d.h"
#include "ui\CocosGUI.h"

USING_NS_CC;
using namespace ui;

/*�����˵��б�cocos2d-x��û���ṩ����������*/

class DropDownList : public Node{
public:
	DropDownList();
	~DropDownList();

	static DropDownList * create(const std::string & normalImage, const std::string & selectedImage,
		const std::string & scale9Image, ValueVector & labelList);

	bool init(const std::string & normalImage, const std::string & selectedImage,
		const std::string & scale9Image, ValueVector & labelList);

	void menuTopItemCallback(Ref * pSender);		//������ʾ����ص�
	void menuListItemCallback(Ref * pSender);		//�б�˵���ص�

	typedef std::function<void(int)> ccCustomCallback;
	
	void setCallback(const ccCustomCallback & callback);		//ע��ص�����
private:
	Vector<Sprite *> _spriteList;		//ѡ���б���ͼƬ
	Vector<Label *> _labelList;			//�����б�������˵��
	Node * _list;
	Label * _topLabel;
	int _currentIndex;			//ѡ��˵���
	ccCustomCallback _callback;		//ע��Ļص�����
};

#endif