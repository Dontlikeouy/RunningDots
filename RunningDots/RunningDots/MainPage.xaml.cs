using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RunningDots
{
    public partial class MainPage : TabbedPage
    {
    public MainPage()
        {
            InitializeComponent();
            switch (Device.RuntimePlatform)
            {


                case Device.WPF:
                    MyTabbedPage.Children.Add(new Page1());
                    break;


            }
            MyTabbedPage.Children.Add(new ConnectFile());
            MyTabbedPage.Children.Add(new SettingsPage());
            MyTabbedPage.Children.Add(new МatrixСontrol());
        }
    }
}
