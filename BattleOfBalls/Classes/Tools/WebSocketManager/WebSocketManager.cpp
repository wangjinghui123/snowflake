#pragma execution_character_set("utf-8")

#include "WebSocketManager.h"
#include "Header/Common.h"

WebSocketManager * WebSocketManager::s_WebSocketManager = NULL;

WebSocketManager::WebSocketManager()
:_socket(NULL)
{
	_accountName = "";
}

WebSocketManager::~WebSocketManager()
{

}

WebSocketManager * WebSocketManager::getInstance()
{
	if (!s_WebSocketManager)
	{
		s_WebSocketManager = new WebSocketManager();
		if (s_WebSocketManager && s_WebSocketManager->init())
		{
			s_WebSocketManager->autorelease();
		}
		else
		{
			CC_SAFE_DELETE(s_WebSocketManager);
		}
	}
	return s_WebSocketManager;
}

bool WebSocketManager::init()
{
	_socket = new WebSocket();

	if (!_socket->init(*this, "120.77.159.165:8766"))  //ip:端口
	{
		CC_SAFE_DELETE(_socket);
		return false;
	}

	return true;
}

void WebSocketManager::onOpen(WebSocket * ws)
{
	log("WebSocket (%p)opened", ws);
}

void WebSocketManager::onMessage(WebSocket * ws, const WebSocket::Data & data)
{
	if (!data.isBinary)
	{
		std::string textStr = std::string("response text msg: ") + data.bytes;
		//log("%s", textStr.c_str());
		processReturnMsg(data.bytes);
	}
	else
	{
		std::string binaryStr = "response bin msg: ";

		for (int i = 0; i < data.len; ++i) {
			if (data.bytes[i] != '\0')
			{
				binaryStr += data.bytes[i];
			}
			else
			{
				binaryStr += "\'\\0\'";
			}
		}
		log("%s", binaryStr.c_str());
	}
}

void WebSocketManager::onClose(WebSocket * ws)
{
	log("websocket instance  closed.");
	if (ws == _socket)
	{
		_socket = nullptr;
	}

	CC_SAFE_DELETE(ws);
}

void WebSocketManager::onError(WebSocket * ws, const WebSocket::ErrorCode & data)
{
	log("Error: %d", data);
}

void WebSocketManager::sendMsg(const std::string & msg)
{
	if (!_socket)
	{
		return;
	}

	if (_socket->getReadyState() == WebSocket::State::OPEN)
	{
		log("send msg: %s", msg.c_str());
		_socket->send(msg);
	}
	else
	{
		log("Client not yet ready");
	}
}

void WebSocketManager::processReturnMsg(const std::string & msg)
{
	rapidjson::Document doc;
	doc.Parse<0>(msg.c_str());

	if (doc.HasParseError())  //打印解析错误
	{
		log("GetParseError %d\n", doc.GetParseError());
		return;
	}

	if (doc.IsObject() && doc.HasMember("MsgType"))
	{
		int msgType = doc["MsgType"].GetInt();

		/*根据协议号解析消息*/
		if (msgType == MessageType::eMsg_LOGIN_RESULT)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("LoginResult", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_ACCOUNT_INFO_RESULT)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("AccountInfoResult", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_CHAT_RECEIVE)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("ChatMsgReceive", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_START_GAME_SINGLE_RESULT)
		{
			_gameConfig[GameMode::eMode_SINGLE] = msg;
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("GameStart", NULL);
		}
		else if (msgType == MessageType::eMsg_MOVE)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("PlayerMove", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_DIVIDE)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("PlayerDivide", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_SPIT_SPORE_RESULT)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("SpitSporeResult", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_ADD_PRICK)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("AddPrick", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_ENTER_PLAYER)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("EnterPlayer", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_PLAYER_CONCENTRATE)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("PlayerConcentrate", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_UPDATE_POSITION)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("UpdatePlayer", (void *)msg.c_str());
		}
		else if (msgType == MessageType::eMsg_UPDATE_TIME)
		{
			Director::getInstance()->getEventDispatcher()->dispatchCustomEvent("UpdateTime", (void *)msg.c_str());
		}
	}
	
}

void WebSocketManager::getGameConfig(int type, std::string & config)
{
	std::map<int, std::string>::iterator itr = _gameConfig.find(type);
	if (itr!=_gameConfig.end())
	{
		config = itr->second;
		return;
	}

	config = "";

}

void WebSocketManager::setAccountName(const std::string & name)
{
	_accountName = name;
}

std::string & WebSocketManager::getAccountName()
{
	return _accountName;
}