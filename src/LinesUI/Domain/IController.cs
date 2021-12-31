using System;
using System.Collections.Generic;
using System.Text;

namespace LinesUI.Domain
{
    public interface IController
    {
        Observable<string> Text { get; }
        void UserCommandEventHandler(string command);
        public void OnRoutedAsync();
    }
}
