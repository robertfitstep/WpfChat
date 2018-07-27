using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfChat.ChatApp;
using Newtonsoft.Json;
using System.Threading;
using System.ComponentModel;

namespace WpfChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User_Controls.UsersContact> contacts = new List<User_Controls.UsersContact>();
        private Dictionary<int, User_Controls.UsersContact> contactsDict = new Dictionary<int, User_Controls.UsersContact>();
        private WpfChat.ChatApp.Chat chat;
        private List<UserControl> messages = new List<UserControl>();
        private const string _DefMessageTxtBoxVal = "Type your message here...";
        private const string _DefSearchTxtBoxVal = "Search";
        private int _ActiveContactID;

        public static DependencyProperty _MessageTxtBoxValueProperty = DependencyProperty.Register("_MessageTxtBoxValue", typeof(String), typeof(MainWindow));

        public String _MessageTxtBoxValue
        {
            get { return (String)GetValue(_MessageTxtBoxValueProperty); }
            set
            {
                SetValue(_MessageTxtBoxValueProperty, value);
            }
        }

        public static DependencyProperty _SearchTxtBoxValueProperty = DependencyProperty.Register("_SearchTxtBoxValue", typeof(String), typeof(MainWindow));

        public String _SearchTxtBoxValue
        {
            get { return (String)GetValue(_SearchTxtBoxValueProperty); }
            set
            {
                SetValue(_SearchTxtBoxValueProperty, value);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == _SearchTxtBoxValueProperty)
            {
                refreshUserContactList(chat.GetContactContaining(_SearchTxtBoxValue));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            chat = new WpfChat.ChatApp.Chat();
            chat.OnUserLogin += OnUserLogin;
            chat.OnSendMessage += OnSendMessage;
            chat.OnReceiveMessage += OnReceiveMessage;
            chat.Login("chatuser1", "password");                //TODO implement user login window
            //chat.SetFakeMessages();
            chat.StartFakeReceivingMessages(5000, 10);

            _MessageTxtBoxValue = _DefMessageTxtBoxVal;
            _SearchTxtBoxValue = _DefSearchTxtBoxVal;

            refreshUserContactList(chat.GetContacts());
            MessageList.ItemsSource = messages;

            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);
        }

        void MainWindowClosing(object sender, CancelEventArgs e)
        {
            LocalSerializer.SaveBackup(chat._User.GetUserInfoData(), chat._User.GetContactList());
        }
        
        private void refreshUserContactList(List<Contact> pContactList)
        {
            UsersContacts.Children.Clear();
            contacts.Clear();
            contactsDict.Clear();

            for (int i = 0; i < pContactList.Count; i++)
            {
                User_Controls.UsersContact control = new WpfChat.User_Controls.UsersContact();
                control.UpdateContact(pContactList[i]);
                control.UserContacMouseDown += LoadMessagesUserControlList;
                control.UserContacMouseDown += SetActiveContact;
                control._WPFContact.SetUnseenMessages(chat._User.GetUnseenMessages(control._WPFContact.ID));
                UsersContacts.Children.Add(control);
                contacts.Add(control);
                contactsDict.Add(pContactList[i]._ContactInfoData._ID, control);
            }

            if (contactsDict.ContainsKey(_ActiveContactID))
            {
                SetActiveContact(contactsDict[_ActiveContactID], new EventArgs());
            }
        }

        public void SetActiveContact(Object sender, EventArgs e)
        {
            if (contactsDict.ContainsKey(_ActiveContactID))
            {
                contactsDict[_ActiveContactID]._WPFContact._BorderVisibility = false;
            }
            WpfChat.User_Controls.UsersContact UsersContact = (WpfChat.User_Controls.UsersContact)sender;
            _ActiveContactID = UsersContact._WPFContact.ID;
            contactsDict[_ActiveContactID]._WPFContact._BorderVisibility = true;
        }

        public void LoadMessagesUserControlList(Object sender, EventArgs e)
        {
            messages.Clear();
            WpfChat.User_Controls.UsersContact UsersContact = (WpfChat.User_Controls.UsersContact)sender;
            FillMessageList(chat._User.GetMessageList(UsersContact._WPFContact.ID));
            ScrollMessagesToBottom();
            chat._User.SetAllMessagesAsSeen(UsersContact._WPFContact.ID);
            UsersContact._WPFContact.SetUnseenMessages(chat._User.GetUnseenMessages(UsersContact._WPFContact.ID));
        }

        private void OnUserLogin(User pUser)
        {

        }

        private void OnSendMessage(Message pMessage, int pContactId)
        {
            if (pContactId == _ActiveContactID)
            {
                AddItemToMessageList(pMessage._MessageInfoData._MessageDirectionType, pMessage._MessageInfoData._Content);
            }
        }

        private void OnReceiveMessage(Message pMessage, int pContactId)
        {
            if (pContactId == _ActiveContactID)
            {
                AddItemToMessageList(pMessage._MessageInfoData._MessageDirectionType, pMessage._MessageInfoData._Content);
                chat._User.SetAllMessagesAsSeen(pContactId);
            }
            else
            {
                if (contactsDict.ContainsKey(pContactId))
                    contactsDict[pContactId]._WPFContact.SetUnseenMessages(chat._User.GetUnseenMessages(pContactId));
            }
        }

        private void OnBtSendMessageClick(object sender, MouseButtonEventArgs e)
        {
            SendMessage();
        }

        private void OnTxtBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_MessageTxtBoxValue == _DefMessageTxtBoxVal) _MessageTxtBoxValue = string.Empty;
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        private void OnSearchBoxMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_SearchTxtBoxValue == _DefSearchTxtBoxVal)
                _SearchTxtBoxValue = string.Empty;
        }

        private void SendMessage()
        {
            if (_ActiveContactID != 0 && _MessageTxtBoxValue != _DefMessageTxtBoxVal && _MessageTxtBoxValue != "")
            {
                chat.SendMessage(new Message(GetMessageData()), _ActiveContactID);

                _MessageTxtBoxValue = string.Empty;

                ScrollMessagesToBottom();
            }
        }

        private MessageInfoData GetMessageData()
        {
            return new MessageInfoData()
            {
                _Content = _MessageTxtBoxValue,
                _ID = MessageController.ReturnNewMessageID(),
                _MessageDirectionType = MessageDirectionType.Outcome,
                _MessageFormatType = MessageFormatType.Text,
                _Date = DateTime.Now,
                _MessageStateType = MessageStateType.Send
            };

        }

        private void ScrollMessagesToBottom()
        {
            this.MessageListScrollViewer.ScrollToBottom();
        }

        private void FillMessageList(List<Message> pMessageList)
        {
            for (int i = 0; i < pMessageList.Count; i++)
            {
                switch (pMessageList[i]._MessageInfoData._MessageDirectionType)
                {
                    case MessageDirectionType.Income:
                        messages.Add(new UserControls.IncomingMessage(pMessageList[i]._MessageInfoData._Content));
                        break;
                    case MessageDirectionType.Outcome:
                        messages.Add(new WpfChat.UserControls.OutcomingMessage(pMessageList[i]._MessageInfoData._Content));
                        break;
                }
            }

            MessageList.Items.Refresh();
        }

        private void AddItemToMessageList(MessageDirectionType pMessageDirection, string pContent)
        {

            switch (pMessageDirection)
            {
                case MessageDirectionType.Income:
                    messages.Add(new UserControls.IncomingMessage(pContent));
                    break;
                case MessageDirectionType.Outcome:
                    messages.Add(new UserControls.OutcomingMessage(pContent));
                    break;
            }

            MessageList.Items.Refresh();
        }

    }
}
