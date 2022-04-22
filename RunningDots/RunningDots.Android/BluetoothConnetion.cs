
using System.Text;
using Android.Bluetooth;
using System.Threading.Tasks;
using System.IO;
using static RunningDots.МatrixСontrol;
using Java.Util;

namespace RunningDots.Droid
{
    public class BluetoothConnetion : IBluetoothConnetion
    {
        BluetoothDevice bluetoothDevice;

        //Подключаться тут или нет
        override public void CreateBluetooth()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            var devices = adapter.BondedDevices;
            foreach (var currentDevice in devices)
            {
                bluetoothDevice = currentDevice;
                break;
            }

        }
        public override async Task SendMessage(string Message)
        {
            var _socket = bluetoothDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            await _socket.ConnectAsync();
            await _socket.OutputStream.WriteAsync(Encoding.ASCII.GetBytes(Message), 0, Message.Length);
            //var a = new Byte[1];

            //await _socket.InputStream.ReadAsync(a);
            //Console.WriteLine(Encoding.UTF8.GetString(a));

            _socket.Close();

        }
    }
}