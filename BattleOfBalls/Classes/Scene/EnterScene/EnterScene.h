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
	void startLoding();		//开始加载资源
	void loading(float dt);
	void enterMenuScene(float dt);		//进入大厅场景
	void imageLoadingCallback(Ref * pSender);  //图片资源加载回调
	void plistLoadingCallback(Ref * pSender);	//plist文件加载回调
private:
	ProgressTimer * _timer;		//进度条控件
	Label * _loadingLabel;		//加载文字提示
};

#endif