
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{
    public class Chat
    {
        //Example of custom delegate, it is almost same as Action type
        //public delegate void OnUserDelegate(User pNewUser, User pOldUser);
        //public OnUserDelegate OnUserDelegateChanged;

        public Action<User> OnUserLogin;
        public Action       OnUserLogout;
        public Action       OnUserRegister;

        public Action<Message,int>       OnSendMessage;
        public Action<Message,int>       OnReceiveMessage;
        private MessageController   _MessageController = null;
        private ContactController   _ContactController = null;

        //Shorter syntax of property with only GET section as ExampleUser
        public User _User { get; private set; } = null;

        /*public User ExmapleUser
{
get { return _User; }
}*/

        public Chat()
        {
            _User = new User();
            _ContactController = new ContactController(this);
            _MessageController = new MessageController(this);            
        }

        public void Register()
        {
            OnUserRegister?.Invoke();
        }

        public void Login(string pUserName, string pPassword)
        {
            //TODO implement user login towards server
            _User = new User();

            _User.UpdataUserInfo(new UserInfoData()
            {
                _ID = 1,
                _Email = "chat@user.sk",
                _NickName = "chatuser1",
                _PhoneNumber = "+421903111222"
            });

            List<Contact> contactList;
            _User.UpdataUserInfo(LocalSerializer.LoadBackup(_User.GetUserInfoData(), out contactList));
            _User.AddContacts(contactList);
            
            OnUserLogin?.Invoke(_User);
        }

        public void Logout()
        {
            _User = null;

            OnUserLogout?.Invoke();
        }

        public List<Contact> GetContactContaining(string pStringToFind)
        {
            return _ContactController.GetContactContaining(pStringToFind);
        }

        #region MessageController methods

        public void SendMessage(Message pMessage,int pContactId)
        {
            _MessageController.SendMessage(pMessage, pContactId);

            OnSendMessage?.Invoke(pMessage, pContactId);
        }

        public void ReceiveMessage(Message pMessage, int pContactId)
        {
            _MessageController.ReceiveMessage(pMessage, pContactId);

            OnReceiveMessage?.Invoke(pMessage, pContactId);
        }

        public void DeleteMessage(Message pMessage)
        {
            _MessageController.DeleteMessage(pMessage);
        }

        //Fake part

        public void StartFakeReceivingMessages(int pPeriodLenght, int pRepeatNTimes)
        {
            _MessageController.StartFakeReceivingMessages(pPeriodLenght, pRepeatNTimes);
        }

        #endregion

        #region Contact controller methods

        public List<Contact> GetContacts()
        {
            return _ContactController.GetContacts();
        }

        //Fake part

        public void SetFakeMessages()
        {
            _ContactController.setFakeContacts();
        }               

        #endregion
    }
}
