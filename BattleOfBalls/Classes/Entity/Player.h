#ifndef _Player_H_
#define _Player_H_

#include "cocos2d.h"

USING_NS_CC;

class Prick;
class PlayerDivision;
class Bean;
class Spore;

class Player :public Node{
public:

	enum class State
	{
		NORMAL, //����״̬
		DIVIDE,  //����״̬
		SPIT,  //����״̬
		CONCENTRATE  //�����к�״̬
	};

	Player();
	~Player();

	static Player * create(Vec2  position,Node * map);
	bool init(Vec2  position,Node * map);

	static Player * create(std::string & name,int vestmentID,int keywordID,Vec2 velocity, Node * map);
	bool init(std::string & name, int vestmentID, int keywordID, Vec2 velocity, Node * map);

	virtual void onExit();

	PlayerDivision * createDivision(Vec2 position, Vec2 velocity, int score);

	int countSpitSporeNum();		//�������һ�ο����³���������
	void spitSpore(Node * map, Map<int, Spore *> & sporeMap, int globalID);		//������
	void dividePlayer();		//����

	bool collideBean(Bean * bean);		//�붹�ӵ���ײ���
	bool collideSpore(Spore * spore);		//�����ӵ���ײ���
	bool collidePrick(Prick *prick);		//���̴̵���ײ���
	bool collideRival(Player *rival);		//����ֵ���ײ���
	int collideDivision(PlayerDivision * division);		//����ַ������ײ

	void updateDivision();		//�����Ϣ����
	void resetPlayer();			//��Ҹ���

	void divideFinish();		//��������ص�
	void concentrate();			//����к�
	void setConcentrate();		//�����к�״̬
	void collidePrickFinish();	//�����̻ص�

	void setVelocity(Vec2 v);	//�����ٶ�
	Vec2 getVelocity();			//��ȡ�ٶ�

	Rect getPlayerRect();		//��ȡ��Ұ�Χ��
	void setCombine(float dt);	
	int getDivisionNum();		//��ȡ��ǰ��������
	float getTotalWeight();
	std::string getPlayerName();
	int getTotalScore();
	Vector<PlayerDivision *> & getDivisionList();

	
	void clearDivisionList();
private:
	Vector<PlayerDivision *> _divisionList;		//��ҷ����б�
	int _divisionNum;							//��������

	Node * _map;			//��ͼָ��
	std::string _playerName;		//�����
	int _vestmentID;		//ʥ��id
	int _keywordID;			//�ؼ���id

	Vec2 _velocity;			//�ٶ�
	int _color;			//��ɫ,û�йؼ���ʱ����
	
	State _state;		//���״̬
	bool _combineEnable; //�����Ƿ���Ժ���
};
#endif