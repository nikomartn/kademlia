using System;
using System.Collections.Generic;
using System.Text;

namespace LinesUI.Domain
{
    public class Observable<T>
    {
        public delegate void TextChanged();
        T _data;
        public T Data
        {
            get => _data;
            set
            {
                _data = value;
                OnTextChanged?.Invoke();
            }
        }
        public event TextChanged OnTextChanged;
    }
}
