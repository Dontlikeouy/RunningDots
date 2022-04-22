using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RunningDots
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }
        static SerialPort _serialPort;

        private void Button_Clicked(object sender, EventArgs e)
        {

            string[] ports = SerialPort.GetPortNames();
            if (Combobox.ItemsSource != null)
                Combobox.ItemsSource.Clear();
            Combobox.ItemsSource = ports;
            Combobox.SelectedIndex = 0;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            _serialPort = new SerialPort(Combobox.SelectedItem.ToString());


            if (OnOff.Text == "Подключится")
            {
                _serialPort.Open();
                OnOff.Text = "Отключится";

            }
            else
            {
                _serialPort.Close();
                OnOff.Text = "Подключится";

            }





        }

        private void LedOnOff_Clicked(object sender, EventArgs e)
        {
            _serialPort.Write("b");

        }
    }
}