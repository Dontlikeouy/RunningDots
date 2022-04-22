using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RunningDots
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectFile : ContentPage
    {
        public ConnectFile()
        {
            InitializeComponent();

        }
        public class PinAndPoint
        {
            public int pin { get; set; }
            public int spoint { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

        }
        public class ObjecM
        {
            public Dictionary<int, Dictionary<int, PinAndPoint>> info = new Dictionary<int, Dictionary<int, PinAndPoint>>();
            public Dictionary<int, int> lastMXYonPin= new Dictionary<int, int>() { };
            public int lastX = 0;
            public int lastY = 0;
            public int panelSize { get; set; }
        }
        static public ObjecM matrixInfo;
        //static public Dictionary<string, Dictionary<string, int>> matrixInfo;
        static public string FileName;
        private async void SelectFile_Clicked(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return;

                NameFile.Text = fileData.FileName;
            }
            catch
            {

            }

        }

        private void SelectGlobal_Clicked(object sender, EventArgs e)
        {
            if (NameFile.Text == null || NameFile.Text == "") return;
            FileName = NameFile.Text;
            DeserializeJson();
        }


        public void DeserializeJson()
        {
            string Path = @"/storage/emulated/0/Android/data/com.companyname.runningdots/Json";
            if (!File.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            using (FileStream ForSR = new FileStream(Path + $"/{FileName}.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                StreamReader reader = new StreamReader(ForSR);

                matrixInfo = JsonConvert.DeserializeObject<ObjecM>(reader.ReadToEnd()) ?? new ObjecM();
            }
        }
        public void SerializeJson()
        {
            string Path = @"/storage/emulated/0/Android/data/com.companyname.runningdots/Json";
            var a = JsonConvert.SerializeObject(matrixInfo);
            File.WriteAllText(Path + $"/{FileName}.json", JsonConvert.SerializeObject(matrixInfo));

        }


    }
}