using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{
    public class ContactController
    {
        private Chat _Chat;
        private Random _randomizer;

        public ContactController(Chat pChat)
        {
            _Chat = pChat;
            //setFakeContacts();
        }


        public void SearchContact(string pNickName)
        {

        }

        public void ChooseContact()
        {

        }

        public void AddContact(Contact pContact)
        {

        }

        public void DeleteContact(Contact pContact)
        {

        }

        public void BlockContact(Contact pContact)
        {

        }

        public void ReportContact(Contact pContact)
        {

        }

        public List<Contact> GetContacts()
        {
            return _Chat._User.GetContactList();
        }

        public List<Contact> GetContactContaining(string pStringToFind)
        {
            return _Chat.GetContacts().FindAll(contactX => contactX._ContactInfoData._NickName.Contains(pStringToFind));
        }

        #region Fake Part

        public static int _contactId = 0;

        public static int getNewContactId()
        {
            _contactId++;
            return _contactId;
        }

        public void setFakeContacts()
        {            
            List<Contact> contacts = new List<Contact>();
            MessageInfoData messageInfoData = new MessageInfoData();
            _randomizer = new Random();


            for (int i = 0; i < 5; i++)
            {
                int id = ContactController.getNewContactId();

                ContactInfoData data = new ContactInfoData()
                {
                    _NickName = "Nickname" + id.ToString(),
                    _ID = id,
                    _ContactStateType = (ContactStateType)_randomizer.Next(0, 2)
                };
                contacts.Add(new Contact(data));
                addFakeMessages(_randomizer.Next(1, 10));
            }

            _Chat._User.AddContacts(contacts);

            void addFakeMessages(int pNumperOfMessages)
            {
                for (int i = 0; i < pNumperOfMessages; i++)
                {
                    contacts[contacts.Count - 1].AddMessage(new Message(GetNewFakeMessage(messageInfoData)));
                }
            }
        }

        private MessageInfoData GetNewFakeMessage(MessageInfoData pMessageInfoData)
        {
            ulong _messageID = MessageController.ReturnNewMessageID();

            pMessageInfoData._Content = "Message " + _messageID;
            pMessageInfoData._Date = DateTime.Now;
            pMessageInfoData._ID = _messageID;
            pMessageInfoData._MessageDirectionType = (MessageDirectionType)_randomizer.Next(1, 3);
            pMessageInfoData._MessageStateType = MessageStateType.Seen;
            pMessageInfoData._MessageFormatType = (MessageFormatType)0;
            return pMessageInfoData;
        }

        #endregion
    }
}
