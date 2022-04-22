using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RunningDots
{
    public abstract class IBluetoothConnetion
    {
        public abstract void CreateBluetooth();
        public abstract Task SendMessage(string Message);

    }
    public partial class App : Application
    {
        public static IBluetoothConnetion BluetoothConnetion;

        public App(IBluetoothConnetion bluetoothConnetion)
        {
            InitializeComponent();
            MainPage = new MainPage();
            BluetoothConnetion = bluetoothConnetion;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
