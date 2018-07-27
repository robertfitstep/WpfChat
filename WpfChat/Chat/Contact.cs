using System;
using System.Collections.Generic;

namespace WpfChat.ChatApp
{  

    [System.Serializable]
    public struct ContactInfoData
    {
        public int _ID;
        public string _NickName;
        public ContactStateType _ContactStateType;
        
        [System.NonSerialized]
        public object _Image;
    }

    public class Contact
    {
        public ContactInfoData _ContactInfoData;
                
        private List<Message> _MessageList = new List<Message>();

        public Contact(ContactInfoData pContactInfo)
        {
            UpdateContactInfo(pContactInfo);
        }

        public Contact(ContactInfoData pContactInfo, List<Message> pMessageList)
        {
            UpdateContactInfo(pContactInfo);
            _MessageList = pMessageList;
        }

        public void AddMessage(Message pMessage)
        {
            _MessageList.Add(pMessage);
        }
                

        public void DeleteMessage(Message pMessage)
        {
            _MessageList.Remove(pMessage);
        }

        public List<Message> GetMessages()
        {
            return _MessageList;
        }

        public void UpdateContactInfo(ContactInfoData pContactInfo)
        {
            _ContactInfoData = pContactInfo;
        }
        
        public string GetNoOfNotSeenMessages()
        {
            int noOfNotSeenMessages = countNotSeenMesages();

            if (noOfNotSeenMessages == 0)
            {
                return null;
            }
            else
            {
                return noOfNotSeenMessages.ToString();
            }
        }

        public void SetAllMessagesAsSeen()
        {
            for(int i = 0; i< _MessageList.Count; i++)
            {
                _MessageList[i]._MessageInfoData._MessageStateType = MessageStateType.Seen;
            }
        }

        private int countNotSeenMesages()
        {
            int noOfNotSeenMessages = 0;

            for (int i = _MessageList.Count - 1; i > 0; i--)
            {
                if (_MessageList[i]._MessageInfoData._MessageStateType != MessageStateType.Seen)
                {
                    noOfNotSeenMessages++;
                }
                else
                {
                    return noOfNotSeenMessages;
                }
            }

            return noOfNotSeenMessages;
        }
        
    }
}
