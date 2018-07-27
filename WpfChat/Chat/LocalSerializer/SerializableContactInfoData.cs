using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{
    [System.Serializable]
    public struct SerializableContactInfoData
    {
        public int _ID;
        public string _NickName;
        public ContactStateType _ContactStateType;
        public List<SerializableMessageInfoData> _MessageInfoDataList;

        public SerializableContactInfoData(int pID, string pNickName, ContactStateType pContactStateType, List<SerializableMessageInfoData> pMessageInfoDataList)
        {
            _ID = pID;
            _NickName = pNickName;
            _ContactStateType = pContactStateType;
            _MessageInfoDataList = pMessageInfoDataList;
        }
    }
}
