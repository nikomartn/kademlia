using LinesUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinesUI
{
    public class Ui
    { 
        public delegate void UserCommandEvent(string command);

        LinkedList<string> buffer = new LinkedList<string>();

        public void Start()
        {
            if (Controller != null)
            {
                FetchAndReload();
            }
            Invalidate();
            Task.Run(() => {
                while (true)
                {
                    HandleInput();
                }
            });
        }


        IController _controller;
        public IController Controller
        {
            get => _controller;
            set
            {
                if (_controller != null)
                {
                    _controller.Text.OnTextChanged -= FetchAndReload;
                    OnUserCommand -= _controller.UserCommandEventHandler;
                }

                _controller = value;
                if (_controller != null)
                {
                    _controller.Text.OnTextChanged += FetchAndReload;
                    OnUserCommand += _controller.UserCommandEventHandler;
                }

            }
        }

        private void FetchAndReload()
        {
            Task.Run(() =>
            {
                lock (buffer)
                {
                    LinkedList<string> splitLines = SplitInLines(Controller.Text.Data);
                    SplitLinesIfBigger(splitLines, Console.BufferWidth);
                    buffer = splitLines;
                    Invalidate();
                }
            });
        }

        LinkedList<string> SplitInLines(string text)
        {
            return new LinkedList<string>(
                Controller.Text.Data.Split(new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None));
        }

        void SplitLinesIfBigger(LinkedList<string> lines, int than)
        {
            string line;
            while ((line = lines.FirstOrDefault((line) => line.Length > than)) != null)
            {
                var before = line.Substring(0, than);
                var after = line.Substring(than);

                var lineNode = lines.Find(line);
                var beforeNode = lines.AddAfter(lineNode, before);
                var afterNode = lines.AddAfter(beforeNode, after);
                lines.Remove(lineNode);
            }
        }

        int NumberOfLines => Console.WindowHeight - 1;
        int _index = 0;
        int Index
        {
            get => _index;
            set
            {
                if (value < 0)
                    _index = 0;
                else if ((value + NumberOfLines) > buffer.Count)
                    Index = buffer.Count - NumberOfLines;
                else
                    _index = value;
            }
        }
        protected List<string> ScreenBuffer
        {
            get
            {
                while (buffer.Count < NumberOfLines)
                {
                    buffer.AddLast("");
                }
                var temp = new string[buffer.Count];
                buffer.CopyTo(temp, 0);
                return new List<string>(temp).GetRange(Index, NumberOfLines);
            }
        }
        private void Redraw()
        {
            lock (buffer)
            {
                Console.Clear();
                foreach (var line in ScreenBuffer)
                {
                    Console.Write(line+"\r\n");
                }
                Console.Write($":{UserCommand}\r");
            }

        }

        string UserCommand { get; set; }
        private void HandleInput()
        {
            InterpretKey(Console.ReadKey());
            Redraw();
        }


        void InterpretKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.DownArrow: Index++; break;
                case ConsoleKey.UpArrow: Index--; break;
                case ConsoleKey.Backspace:
                    if (UserCommand.Length > 0)
                    {
                        UserCommand = UserCommand.Remove(UserCommand.Length - 1);
                    }
                    break;
                case ConsoleKey.Enter: OnUserCommand?.Invoke(UserCommand); break;
                default: UserCommand += key.KeyChar; break;
            }
        }

        private event UserCommandEvent OnUserCommand;

        private void Invalidate()
        {
            Index = 0;
            UserCommand = "";
            Redraw();
        }
    }
}
