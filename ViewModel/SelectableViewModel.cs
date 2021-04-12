using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace CargoLampTest.ViewModel
{

    public class SelectableViewModel : ViewModelBase
    {
        private bool _isSelected;
        private string _name;
        private string _description;
        private int _code;


        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                string sendMessage = Code.ToString() + "/" + value;
                Messenger.Default.Send<NotificationMessage, USB7230ViewModel>(new NotificationMessage(this, sendMessage));
                RaisePropertyChanged("IsSelected");
            }
        }

        public int Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                RaisePropertyChanged("Code");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    var handler = PropertyChanged;
        //    if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
