using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.ChatApp
{

    public static class LocalSerializer
    {
        private static string currentDirectory;

        static LocalSerializer()
        {
            currentDirectory = Directory.GetCurrentDirectory();
        }

        public static void SaveBackup(UserInfoData pUserInfoData, List<Contact> pContactList)
        {
            string serializedUserData = JsonConvert.SerializeObject(GetSerializableUserData(pUserInfoData, pContactList));

            Console.WriteLine(serializedUserData);

            //TODO implement saving images to disk
            WriteDataToFile(serializedUserData, pUserInfoData._ID);
        }

        public static UserInfoData LoadBackup(UserInfoData pUserInfoData, out List<Contact> pContactList)
        {
            string dataFromFile = ReadDataFromFile(pUserInfoData._ID);

            SerializableUserData serializableUserData = JsonConvert.DeserializeObject<SerializableUserData>(dataFromFile);

            if (dataFromFile != String.Empty)
            {
                return GetUnserializableUserData(serializableUserData, out pContactList);
            }
            else
            {
                pContactList = new List<Contact>();

                return pUserInfoData;
            }            
        }

        private static UserInfoData GetUnserializableUserData(SerializableUserData serializableUserData, out List<Contact> pContactList)
        {
            pContactList = GetContactList(serializableUserData._ContactInfoDataList);

            SerializableUserData s = serializableUserData;

            UserInfoData userInfoData = new UserInfoData()
            {
                _ID = s._ID,
                _Email = s._Email,
                _NickName = s._NickName,
                _PhoneNumber = s._PhoneNumber,
                _UserStateType = s._UserStateType
                //TODO implement Image local backup
            };

            return userInfoData;
        }

        private static List<Contact> GetContactList(List<SerializableContactInfoData> contactInfoDataList)
        {
            List<Contact> contacts = new List<Contact>();

            for(int i=0;i<contactInfoDataList.Count; i++)
            {
               SerializableContactInfoData c = contactInfoDataList[i];

                ContactInfoData contactInfoData = new ContactInfoData()
                {
                    _ContactStateType = c._ContactStateType,
                    _ID = c._ID,
                    //TODO implement loading images from localdisk
                    _NickName = c._NickName
                };

                contacts.Add(new Contact(contactInfoData, GetMessageList(c._MessageInfoDataList)));                                                
            }

            return contacts;
        }

        private static List<Message> GetMessageList(List<SerializableMessageInfoData> messageInfoDataList)
        {
            List<Message> messageList = new List<Message>();

            for(int i=0; i<messageInfoDataList.Count; i++)
            {
                SerializableMessageInfoData m = messageInfoDataList[i];

                messageList.Add(new Message(new MessageInfoData() {
                    _Content = m._Content,
                    _Date = m._Date,
                    _ID = m._ID,
                    _MessageDirectionType = m._MessageDirectionType,
                    _MessageFormatType = m._MessageFormatType,
                    _MessageStateType = m._MessageStateType
                }));
            }

            return messageList;
        }

        private static string ReadDataFromFile(int pUserId)
        {
            string serializedUserData = String.Empty;

            try
            {
                serializedUserData = System.IO.File.ReadAllText(currentDirectory + @"\usr" + pUserId + ".txt");

                return serializedUserData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return serializedUserData;
            }
        }

        private static void WriteDataToFile(string pSerializedUserData, int pUserId)
        {
            try
            {
                System.IO.File.WriteAllText(currentDirectory + @"\usr" + pUserId + ".txt", pSerializedUserData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static SerializableUserData GetSerializableUserData(UserInfoData pUserInfoData, List<Contact> pContactList)
        {
            UserInfoData u = pUserInfoData;
            return new SerializableUserData(u._ID, u._NickName, u._Email, u._PhoneNumber, u._UserStateType, GetContactInfoDataList(pContactList));
        }

        private static List<SerializableContactInfoData> GetContactInfoDataList(List<Contact> pContactList)
        {
            List<SerializableContactInfoData> ContactListData = new List<SerializableContactInfoData>();

            for (int i = 0; i < pContactList.Count; i++)
            {
                ContactInfoData c = pContactList[i]._ContactInfoData;
                ContactListData.Add(new SerializableContactInfoData(c._ID, c._NickName, c._ContactStateType, GetMessageInfoDataList(pContactList[i].GetMessages())));
            }

            return ContactListData;
        }

        private static List<SerializableMessageInfoData> GetMessageInfoDataList(List<Message> pMessageList)
        {
            List<SerializableMessageInfoData> MessageInfoDataList = new List<SerializableMessageInfoData>();

            for (int i = 0; i < pMessageList.Count; i++)
            {
                MessageInfoData m = pMessageList[i]._MessageInfoData;
                MessageInfoDataList.Add(new SerializableMessageInfoData(m._ID, m._Date, m._MessageStateType, m._Content, m._MessageFormatType, m._MessageDirectionType));
            }

            return MessageInfoDataList;
        }
    }
}
