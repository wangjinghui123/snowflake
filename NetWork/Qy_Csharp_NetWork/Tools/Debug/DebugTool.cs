/***********************************************************
*                   *  调试输出类  *                       *
*                                                          *
*   1.根据宏判断是否需要输出                               *
*   2.可调节调试时所输出的平台（默认为Unity_Engine平台）   *
*   3.考虑日后的多种类扩展输出                             *
*                                                          *
************************************************************/



//#define CSHARP_CONSOLE
#define UNITY_ENGINE

using System.Threading;

namespace Qy_CSharp_NetWork.Tools.Debug
{
    public class DebugTool
    {
        public static object m_lockHelper = new object();

        private static bool m_Global = false;

        private static bool m_CanLog = false;
        private static bool m_CanWarning = false;
        private static bool m_CanError = false;
        private static bool m_CanLogTag = false;
        private static bool m_CanLogException = false;
        public static bool NeedLog
        {
            get
            {
                return m_Global;
            }
            set
            {
                m_Global = value;
                m_CanLog = m_Global;
                m_CanWarning = m_Global;
                m_CanError = m_Global;
                m_CanLogTag = m_Global;
                m_CanLogException = m_Global;
            }
        }
        public static void SelectWhichLogCanWork(bool CanLog = true, bool CanWarning = true, bool CanError = true, bool CanLogTag = true, bool CanLogException = true)
        {
            m_CanLog = CanLog;
            m_CanWarning = CanWarning;
            m_CanError = CanError;
            m_CanLogTag = CanLogTag;
            m_CanLogException = CanLogException;
        }
        //=====================================================================================
        public static void Log(string logMsg)
        {
            _MyLog(LogType.Log, "[LAM_log]:" + logMsg);
        }
        public static void LogWarning(string logMsg)
        {
            _MyLog(LogType.LogWarning, "[LAM_warning]:" + logMsg);
        }
        public static void LogError(string logMsg)
        {
            _MyLog(LogType.LogError, "[LAM_error]:" + logMsg);
        }
        public static void LogTag(string tag = "", string logMsg = "", string color = "#AEA74BFF")
        {
            _MyLog(LogType.LogTag, "[LAM_tag]:" + "<color=" + color + ">[" + tag + "]---></color> " + logMsg);
        }
        public static void LogException(System.Exception ex)
        {
            _MyLog(LogType.LogException, "[LAM_Exception]:" + ex.Message);
        }

        private static void _MyLog(LogType type, string str)
        {
            Monitor.Enter(m_lockHelper);
            switch (type)
            {
                case LogType.Log:
                    if (m_CanLog)
                    {
#if CSHARP_CONSOLE
                        System.Console.WriteLine(str);
#elif UNITY_ENGINE
                        UnityEngine.Debug.Log(str);
#endif
                    }
                    break;
                case LogType.LogWarning:
                    if (m_CanWarning)
                    {
#if CSHARP_CONSOLE
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
                        System.Console.WriteLine("-Warning-: " + str);
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.Gray;
#elif UNITY_ENGINE
                        UnityEngine.Debug.LogWarning(str);
#endif
                    }

                    break;
                case LogType.LogTag:
                    if (m_CanLogTag)
                    {
#if CSHARP_CONSOLE
                        System.Console.BackgroundColor = System.ConsoleColor.DarkGreen;
                        System.Console.ForegroundColor = System.ConsoleColor.Black;
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.Gray;
                        System.Console.WriteLine("--->" + str);
#elif UNITY_ENGINE
                        UnityEngine.Debug.Log(str);
#endif
                    }
                    break;
                case LogType.LogError:
                    if (m_CanError)
                    {
#if CSHARP_CONSOLE
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
                        System.Console.WriteLine("-Error-: " + str);
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.Gray;
#elif UNITY_ENGINE
                        UnityEngine.Debug.LogError(str);
#endif
                    }
                    break;
                case LogType.LogException:

                    if (m_CanLogException)
                    {
#if CSHARP_CONSOLE
                        System.Console.BackgroundColor = System.ConsoleColor.Gray;
                        System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
                        System.Console.WriteLine("[LAM]:" + "-exception- :");
                        System.Console.WriteLine("              -Message-   : " + str);

                        //System.Console.WriteLine("              -Message-   : " + ex.Message);
                        //System.Console.WriteLine("              -Source-    : " + ex.Source);
                        //System.Console.WriteLine("              -StackTrace-: " + ex.StackTrace);
                        System.Console.BackgroundColor = System.ConsoleColor.Black;
                        System.Console.ForegroundColor = System.ConsoleColor.Gray;
#elif UNITY_ENGINE
                        UnityEngine.Debug.LogError(str);
#endif
                    }
                    break;
            }
            Monitor.Exit(m_lockHelper);
        }

        //==============================================================

        //        public static void Log(string logMsg)
        //        {
        //            Monitor.Enter(m_lockHelper);
        //            if (m_CanLog)
        //            {
        //#if CSHARP_CONSOLE
        //                System.Console.WriteLine("[LAM]:"+logMsg);
        //#elif UNITY_ENGINE
        //                UnityEngine.Debug.Log("[LAM]:" + logMsg);
        //#endif
        //            }
        //            Monitor.Exit(m_lockHelper);
        //        }
        //        public static void LogWarning(string logMsg)
        //        {
        //            Monitor.Enter(m_lockHelper);
        //            if (m_CanWarning)
        //            {
        //#if CSHARP_CONSOLE
        //                System.Console.BackgroundColor = System.ConsoleColor.Black;
        //                System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
        //                System.Console.WriteLine("[LAM]:" + "-Warning-: " + logMsg);
        //                System.Console.BackgroundColor = System.ConsoleColor.Black;
        //                System.Console.ForegroundColor = System.ConsoleColor.Gray;
        //#elif UNITY_ENGINE
        //                UnityEngine.Debug.LogWarning("[LAM]:" + logMsg);
        //#endif
        //            }
        //            Monitor.Exit(m_lockHelper);
        //        }
        //        public static void LogError(string logMsg)
        //        {
        //            Monitor.Enter(m_lockHelper);
        //            if (m_CanError)
        //            {
        //#if CSHARP_CONSOLE
        //                System.Console.BackgroundColor = System.ConsoleColor.Black;
        //                System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
        //                System.Console.WriteLine("[LAM]:" + "-Error-: " + logMsg);
        //                System.Console.BackgroundColor = System.ConsoleColor.Black;
        //                System.Console.ForegroundColor = System.ConsoleColor.Gray;
        //#elif UNITY_ENGINE
        //                UnityEngine.Debug.LogError("[LAM]:" + logMsg);
        //#endif
        //            }
        //            Monitor.Exit(m_lockHelper);
        //        }
        //        public static void LogTag(string tag = "", string logMsg = "", string color = "#AEA74BFF")
        //        {
        //            Monitor.Enter(m_lockHelper);
        //            if (m_CanLogTag)
        //            {
        //#if CSHARP_CONSOLE
        //            System.Console.BackgroundColor = System.ConsoleColor.DarkGreen;
        //            System.Console.ForegroundColor = System.ConsoleColor.Black;
        //            System.Console.Write("[LAM]:" + "[" + tag + "]");
        //            System.Console.BackgroundColor = System.ConsoleColor.Black;
        //            System.Console.ForegroundColor = System.ConsoleColor.Gray;
        //            System.Console.WriteLine("--->" + logMsg);
        //#elif UNITY_ENGINE
        //                UnityEngine.Debug.Log("[LAM]:" + "<color=" + color + ">[" + tag + "]---></color> " + logMsg);
        //#endif
        //            }
        //            Monitor.Exit(m_lockHelper);
        //        }
        //        public static void LogException(System.Exception ex)
        //        {
        //            Monitor.Enter(m_lockHelper);
        //            if (m_CanLogException)
        //            {
        //#if CSHARP_CONSOLE
        //                System.Console.BackgroundColor = System.ConsoleColor.Gray;
        //                System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
        //                System.Console.WriteLine("[LAM]:" + "-exception- :");
        //                System.Console.WriteLine("              -Message-   : " + ex.Message);
        //                System.Console.WriteLine("              -Source-    : " + ex.Source);
        //                System.Console.WriteLine("              -StackTrace-: " + ex.StackTrace);
        //                System.Console.BackgroundColor = System.ConsoleColor.Black;
        //                System.Console.ForegroundColor = System.ConsoleColor.Gray;
        //#elif UNITY_ENGINE
        //                UnityEngine.Debug.LogError("[LAM]:" + ex.Message);
        //#endif
        //            }
        //            Monitor.Exit(m_lockHelper);
        //        }
    }

    public enum LogType
    {
        Log = 1,
        LogWarning = 2,
        LogTag = 3,
        LogError = 4,
        LogException = 5
    }
}
