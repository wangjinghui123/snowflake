#ifndef __JOYSTICK_H__
#define __JOYSTICK_H__

#include "cocos2d.h"

USING_NS_CC;

/*��Ϸ��ʹ�õ�����ҡ����*/

class Joystick : public Node{
public:
	Joystick();
	~Joystick();

	static Joystick * create(const std::string & backgroundPath, const std::string & thumbPath);

	virtual bool init(const std::string & backgroundPath, const std::string & thumbPath);

	bool onTouchBegan(Touch * touch, Event * event);
	void onTouchMoved(Touch * touch, Event * event);
	void onTouchEnded(Touch * touch, Event * event);
	void onTouchCancelled(Touch * touch, Event * event);

	Vec2 getVelocity();
	void updateVelocity(Vec2 point);

	void setDirectionNum(int num);		//ҡ�˿ɿ��Ƶķ�������
	void setDeadRadius(float r);		//���Ĳ��ɴ�������뾶
	void setAutoCenter(bool b);			//ȡ����������Ƹ��Զ�����

	float round(float r);			//��������
private:
	Sprite * _thumb;
	float _joystickRadius;
	float _joystickRadiusSq;  //�����뾶ƽ��
	float _thumbRadius;
	float _thumbRadiusSq;  //ҡ�˰뾶ƽ��
	float _deadRadius;
	float _deadRadiusSq;    //���ɴ�������뾶ƽ��
	Vec2 _velocity;
	int _directionNum;
	bool _hasDead;
	bool _isDPad;           //�е�λ����
	bool _autoCenter;		//�Զ�����
};



#endif