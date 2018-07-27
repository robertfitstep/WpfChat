using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{    
        [System.Serializable]
        public struct SerializableUserData
        {
            public int _ID;
            public string _NickName;
            public string _Email;
            public string _PhoneNumber;
            public ContactStateType _UserStateType;
            public List<SerializableContactInfoData> _ContactInfoDataList;

            public SerializableUserData(int pID, string pNickName, string pEmail, string pPhoneNumber, ContactStateType pUserStateType, List<SerializableContactInfoData> pContactInfoDataList)
            {
                _ID = pID;
                _NickName = pNickName;
                _Email = pEmail;
                _PhoneNumber = pPhoneNumber;
                _UserStateType = pUserStateType;
                _ContactInfoDataList = pContactInfoDataList;
            }

        }    
}
