using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WpfChat.ChatApp
{
    public class MessageController
    {
        private Chat _Chat;

        public MessageController(Chat pChat)
        {
            _Chat = pChat;

            //if (pChat.GetContacts().Count > 0)
            //    StartFakeReceivingMessages(5000, 10);
        }

        public void SendMessage(Message pMessage, int pContactId)
        {
            _Chat._User.AddMessage(pMessage, pContactId);
        }

        public void ReceiveMessage(Message pMessage, int pContactId)
        {
            _Chat._User.AddMessage(pMessage, pContactId);
        }

        public void DeleteMessage(Message pMessage)
        {

        }

        #region Fake Part

        private static ulong _messageID = 0;
        private static Random _randomizer = new Random();

        private int _receivedNMessages;
        private int _repeatNTimes;
        private System.Windows.Threading.DispatcherTimer _randMessageTimer;


        public void StartFakeReceivingMessages(int pPeriodLenght, int pRepeatNTimes)
        {
            _repeatNTimes = pRepeatNTimes;

            _randMessageTimer = new System.Windows.Threading.DispatcherTimer();
            _randMessageTimer.Tick += new EventHandler(ReceiveFakeMessage);
            _randMessageTimer.Interval = new TimeSpan(0, 0, 5);
            _randMessageTimer.Start();
        }

        private void ReceiveFakeMessage(object source, EventArgs e)
        {
            Message m = GetFakeReceivedMessage();

            //Thread thread = new Thread(() => { _Chat.ReceiveMessage(m, GetRandomContactID()); });
            //thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            //thread.Start();
            //thread.Join(); //Wait for the thread to end

            _Chat.ReceiveMessage(m, GetRandomContactID());

            if (_receivedNMessages < _repeatNTimes - 1)
            {
                _receivedNMessages++;
            }
            else
            {
                _randMessageTimer.Stop();
                _receivedNMessages = 0;
            }
        }

        private int GetRandomContactID()
        {
            List<Contact> contactList = _Chat._User.GetContactList();
            int randomContactIndex = MessageController._randomizer.Next(0, contactList.Count);
            return contactList[randomContactIndex]._ContactInfoData._ID;
        }

        private Message GetFakeReceivedMessage()
        {
            MessageInfoData data = new MessageInfoData
            {
                _Content = "Fake Received Message",
                _Date = DateTime.Now,
                _ID = MessageController.ReturnNewMessageID(),
                _MessageDirectionType = MessageDirectionType.Income,
                _MessageFormatType = MessageFormatType.Text,
                _MessageStateType = MessageStateType.Received
            };

            return new Message(data);
        }

        public static ulong ReturnNewMessageID()
        {
            _messageID++;
            return _messageID;
        }

        # endregion 
    }
}
