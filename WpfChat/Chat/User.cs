using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace WpfChat.ChatApp
{
    /// <summary>
    /// [System.Serializable] tomuto se rika Atribut
    /// Tento atribut tady musi byt aby tato struktura se mohla prevest do stringu nebo treba do pole bytu
    /// </summary>
    [System.Serializable]
    public struct UserInfoData
    {
        public int              _ID;
        public string           _NickName;
        public string           _Email;
        public string           _PhoneNumber;
        public ContactStateType    _UserStateType;

        [System.NonSerialized]
        public object _Image;
    }

    public class User
    {
        private UserInfoData _UserInfoData;

        private List<Contact>               _ContactList = new List<Contact>();
        private Dictionary<int, Contact>    _ContactDict = new Dictionary<int, Contact>();
        
        public void AddContact(Contact pContact)
        {
            _ContactList.Add(pContact);
            _ContactDict.Add(pContact._ContactInfoData._ID, pContact);
        }

        public void AddContacts(List<Contact> pContactList)
        {
            for (int i = 0; i < pContactList.Count; i++)
            {
                Contact contact = pContactList[i];
                _ContactList.Add(contact);
                _ContactDict.Add(contact._ContactInfoData._ID, contact);
            }
        }

        public void DeleteContact(Contact pContact)
        {
            _ContactList.Remove(pContact);
            _ContactDict.Remove(pContact._ContactInfoData._ID);
        }

        public List<Contact> GetContactList()
        {
            return _ContactList;
        }

        public Dictionary<int, Contact> GetContactDict()
        {
            return _ContactDict;
        }

        public UserInfoData GetUserInfoData()
        {
            return _UserInfoData;
        }
        
        public void UpdataUserInfo(UserInfoData pUserInfoData)
        {
            _UserInfoData = pUserInfoData;
        }

        public void AddMessage(Message pMessage, int pContactId)
        {
            _ContactDict[pContactId].AddMessage(pMessage);
        }

        public List<Message> GetMessageList(int pContactId)
        {
            return _ContactDict[pContactId].GetMessages();
        }

        public string GetUnseenMessages(int pContactId)
        {
            return _ContactDict[pContactId].GetNoOfNotSeenMessages();
        }

        public void SetAllMessagesAsSeen(int pContactId)
        {
            _ContactDict[pContactId].SetAllMessagesAsSeen();
        }
    }
}
