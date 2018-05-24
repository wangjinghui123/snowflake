#ifndef __WebSocketManager_H__
#define __WebSocketManager_H__

#include "cocos2d.h"
#include "network\WebSocket.h"

USING_NS_CC;
using namespace cocos2d::network;

/*与服务器收发的网络通信管理类，继承了WebSocket接口，消息格式为Json字符串*/

class WebSocketManager : public Ref, public WebSocket::Delegate{
public:
	WebSocketManager();
	~WebSocketManager();

	static WebSocketManager * getInstance();

	virtual bool init();

	virtual void onOpen(WebSocket * ws);		//与服务器建立连接

	virtual void onMessage(WebSocket * ws, const WebSocket::Data & data);		//收到消息

	virtual void onClose(WebSocket * ws);		//连接关闭

	virtual void onError(WebSocket * ws, const WebSocket::ErrorCode & data);		//错误

	void sendMsg(const std::string & msg);		//向服务器发送消息

	void processReturnMsg(const std::string & msg);		//解析

	void getGameConfig(int type,std::string & config);		
	void setAccountName(const std::string & name);
	std::string & getAccountName();
private:
	static WebSocketManager * s_WebSocketManager;
	WebSocket * _socket;
	std::map<int, std::string> _gameConfig;		//地图初始化数据
	std::string _accountName;		//账户名
	
};

#endif