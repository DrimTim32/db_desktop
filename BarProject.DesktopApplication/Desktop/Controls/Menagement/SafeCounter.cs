using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BarProject.DatabaseProxy.Annotations;

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    public sealed class SafeCounter
    {
        private int _counter = -1;
        private readonly object locker = new object();
        public int Counter
        {
            get
            {
                lock (locker)
                {
                    return _counter;
                }
            }
            set
            {
                lock (locker)
                {
                    _counter = value;
                    if (_counter == 0)
                    {
                        OnPropertyChanged();
                    }
                }
            }
        }

        public event EventHandler Event;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Event?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}