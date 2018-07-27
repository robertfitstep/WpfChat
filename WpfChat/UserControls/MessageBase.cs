using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfChat.UserControls
{
    public class MessageBase : UserControl
    {

        public static DependencyProperty _MessageContentProperty = DependencyProperty.Register("_MessageContent", typeof(String), typeof(UserControls.MessageBase));

        public String _MessageContent
        {
            get { return (String)GetValue(_MessageContentProperty); }
            set
            {
                SetValue(_MessageContentProperty, value);
            }
        }
        
        public MessageBase()
        {

        }

        public MessageBase(string pMessageContent)
        {
            _MessageContent = pMessageContent;
        }
    }
}
