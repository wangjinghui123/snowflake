using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Qy_Socket
{
    delegate void ClientCotrollerEventDelegate(SocketClientController sender, SocketClieCtrlEventArgs eventArgs);

    class SocketClieCtrlEventArgs
    {
        public EVENT_TYPE EventType { get; private set; }
        public COMPLETE_OR_FAILED IsComplete { get; private set; }
        public string Message { get; private set; }

        public void SetParam(EVENT_TYPE eventType, COMPLETE_OR_FAILED isComplete, string message)
        {
            EventType = eventType;
            IsComplete = isComplete;
            Message = message;
        }

    }


}
