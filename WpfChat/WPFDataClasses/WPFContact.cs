using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WpfChat.ChatApp;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfChat.WPFDataClasses
{
    public class WPFContact : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ContactInfoData _Data;
        private bool _redDotVisibility = false;
        private bool _borderVisibility = false;
        
        public string _NoOfNotSeenMessages { get; private set; }

        public WPFContact()
        {
            _NoOfNotSeenMessages = null ;
        }
                

        public bool _BorderVisibility
        {
            set
            {
                _borderVisibility = value;
                OnPropertyChanged("_BorderVisibility");
            }
            get
            {
                return _borderVisibility;
            }
        }

        public bool _RedDotVisibility
        {
            set
            {
                _redDotVisibility = value;
                OnPropertyChanged("_RedDotVisibility");
            }

            get
            {
                return _redDotVisibility;
            }
        }

        public bool _GreenDotVisibility
        {
            get
            {
                return GetGreenDotVisibility();
            }
        }

        public int ID
        {
            get
            {
                return _Data._ID;
            }
        }

        public string Name
        {
            get
            {
                return _Data._NickName;
            }
        }

        public ContactStateType ContactStateType
        {
            get
            {
                return _Data._ContactStateType;
            }
        }

        public void SetContactData(ContactInfoData pData)
        {
            _Data = pData;
            OnPropertyChanged("Name");
            OnPropertyChanged("_GreenDotVisibility");
        }
        
        public void SetUnseenMessages(string pNoOfNotSeenMessages)
        {
            _NoOfNotSeenMessages = pNoOfNotSeenMessages;

            if (_NoOfNotSeenMessages == null)
            {
                _RedDotVisibility = false;
            }
            else
            {
                _RedDotVisibility = true;
            }

            OnPropertyChanged("_NoOfNotSeenMessages");
        }

        private bool GetGreenDotVisibility()
        {
            switch (_Data._ContactStateType)
            {
                case ContactStateType.Online:
                    return true;
                default:
                    return false;
            }
        }

        private void OnPropertyChanged(string pParam)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(pParam));
            }
        }
    }
}
