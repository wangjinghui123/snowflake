#ifndef _PlayerDivision_H_
#define _PlayerDivision_H_

#include "Entity.h"

class Bean;
class Prick;
class Spore;

class PlayerDivision : public Entity{
public:
	PlayerDivision();
	~PlayerDivision();

	static PlayerDivision * create(const std::string& filename);
	bool init(const std::string& filename);

	static PlayerDivision * create(const std::string &name, int vestmentID, int keywordID, int score);
	bool init(const std::string &name, const int vestmentID, int keywordID, int score);

	bool collideBean(Bean * bean);
	bool collideSpore(Spore *spore);
	bool collidePrick(Prick *prick);

	float getSpeed();
	void calculateData();

	void eatBean(Bean * bean);
	void eatSpore(Spore * spore);
	void eatRival(int score);
	void eatPrick(int score);

	virtual void setScore(int score);

	void spitSpore();
	void divide();
	void scaleSprite();
	void setVestmentID(int id);
	void setPlayerName(const std::string name);
	void addWeight(float weight);
	void setWeight(float weight);
	float getWeight();
	void setVelocity(Vec2 v);
	Vec2 getVelocity();

	void setPrePosition(const Vec2 & position);
	Vec2 getPrePosition();
private:
	Label * _nameLabel;
	std::string _name;
	int _vestmentID;
	int _keywordID;

	float _speed;
	float _weight;
	Vec2 _velocity;
	
	bool _showVestment;

	Vec2 _prePosition;  //上一帧的位置
};

#endif