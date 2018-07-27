using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfChat.ChatApp;
using WpfChat.WPFDataClasses;

namespace WpfChat.User_Controls
{
    /// <summary>
    /// Interaction logic for UsersContact.xaml
    /// </summary>
    public partial class UsersContact : UserControl
    {
        public WPFContact _WPFContact;

        public event EventHandler UserContacMouseDown;

        public UsersContact()
        {
            InitializeComponent();
            _WPFContact  = new WPFContact();
            DataContext = _WPFContact;
        }

        public void UpdateContact(WpfChat.ChatApp.Contact pContact)
        {
            _WPFContact.SetContactData(pContact._ContactInfoData);            
        }

        private void ContactClick(object sender, MouseEventArgs e)
        {
            UserContacMouseDown(this, e);
        }
    }
}
