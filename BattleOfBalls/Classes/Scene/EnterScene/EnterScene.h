#ifndef _EnterScene_H_
#define _EnterScene_H_

#include "cocos2d.h"

USING_NS_CC;

class EnterScene : public Layer{
public:
	static Scene * createScene();

	EnterScene();
	~EnterScene();

	virtual bool init();
	CREATE_FUNC(EnterScene);

	virtual void update(float delta);
	virtual void onExit();
private:
	void startLoding();		//��ʼ������Դ
	void loading(float dt);
	void enterMenuScene(float dt);		//�����������
	void imageLoadingCallback(Ref * pSender);  //ͼƬ��Դ���ػص�
	void plistLoadingCallback(Ref * pSender);	//plist�ļ����ػص�
private:
	ProgressTimer * _timer;		//�������ؼ�
	Label * _loadingLabel;		//����������ʾ
};

#endif