using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{
    [System.Serializable]
    public struct SerializableMessageInfoData
    {
        public ulong _ID;
        public DateTime _Date;
        public MessageStateType _MessageStateType;
        public string _Content;
        public MessageFormatType _MessageFormatType;
        public MessageDirectionType _MessageDirectionType;

        public SerializableMessageInfoData(ulong pID, DateTime pDate, MessageStateType pMessageStateType, string pContent, MessageFormatType pMessageFormatType, MessageDirectionType pMessageDirectionType)
        {
            _ID = pID;
            _Date = pDate;
            _MessageStateType = pMessageStateType;
            _Content = pContent;
            _MessageFormatType = pMessageFormatType;
            _MessageDirectionType = pMessageDirectionType;
        }
    }
}
