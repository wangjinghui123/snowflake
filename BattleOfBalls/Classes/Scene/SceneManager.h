#ifndef _SceneManager_H_
#define _SceneManager_H_

#include "cocos2d.h"
USING_NS_CC;

/*���������������������������ת�����ֳ���δʵ��*/

class SceneManager : public Ref
{
public:
	enum SceneType
	{
		en_MenuScene,		//��������
		en_GameScene,		//��Ϸ����
		en_PlayerScene,		//�����Ϣ����
		en_ExitScene,		//�˳�����
		en_EnterScene,		//��Ϸ���볡��
		en_StoryScene,		//������³���
		en_StoreScene,		//�̵곡��
		en_GameOverScene,	//��Ϸ��������	
		en_TeamScene,		//ս�ӳ���
		en_RelationScene,	//��ϵ����
		en_RankScene,		//���а񳡾�
		en_WatchGameScene	//��ս����
	};

	static SceneManager* getInstance();
	virtual bool init();

	void changeScene(SceneType enSceneType);
private:
	static SceneManager * s_SceneManager;

};


#endif