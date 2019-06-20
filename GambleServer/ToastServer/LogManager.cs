using System;
using System.Text;

namespace ToastServer
{
    class LogManager
    {
        public static void WriteClientMessage(string _text, int _connectionID)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("클라이언트[{0}]: {1}", _connectionID, _text);
            WriteLog(builder.ToString());

            builder = null;
        }

        public static void WriteClientWorkLog(string _text, int _connectionID)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("클라이언트[{0}]: {1}", _connectionID, _text);
            WriteLog(builder.ToString());

            builder = null;
        }

        public static void WriteRoomLog(string _text, int _roomIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Room[{0}]: {1}", _roomIndex, _text);
            WriteLog(builder.ToString());

            builder = null;
        }

        public static void WriteLog(string _text)
        {
            WriteConsole("로그", _text, ConsoleColor.Gray);
        }

        public static void WriteInfo(string _text)
        {
            WriteConsole("정보", _text, ConsoleColor.White);
        }

        public static void WriteWarning(string _text)
        {
            WriteConsole("경고", _text, ConsoleColor.Yellow);
        }

        public static void WriteError(string _text)
        {
            WriteConsole("오류", _text, ConsoleColor.Red);
        }

        private static void WriteConsole(string _consoleType, string _text, ConsoleColor _color)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("[{0} {1}]: {2}", DateTime.Now, _consoleType, _text);

            Console.ForegroundColor = _color;
            Console.WriteLine(builder.ToString());
            Console.ResetColor();

            builder = null;
        }
    }
}
