using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RunningDots.ConnectFile;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RunningDots
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

        }




        public const int MXY = 15;

        //Проверка
        // ##
        //###
        //#
        private void Generate_Clicked(object sender, EventArgs e)
        {
            if (
                 PINnumber.SelectedItem == null ||
                 MatrixSize.SelectedItem == null ||
                 LeftofRigth.SelectedItem == null ||
                 UporDown.SelectedItem == null
                ) return;



            int intMatrixSize = Convert.ToInt16(MatrixSize.SelectedItem);
            int intPin = Convert.ToInt16(PINnumber.SelectedItem.ToString());


            switch (LeftofRigth.SelectedItem.ToString())
            {
                case "Слева":

                    if (matrixInfo.lastX - intMatrixSize >= 0)
                    {
                        matrixInfo.lastX -= intMatrixSize;

                    }
                    break;
                case "Справа":
                    matrixInfo.lastX += intMatrixSize;
                    break;
            }
            switch (UporDown.SelectedItem.ToString())
            {
                case "Сверху":
                    if (matrixInfo.lastY - intMatrixSize >= 0)
                    {
                        matrixInfo.lastY -= intMatrixSize;


                    }
                    break;
                case "Снизу":
                    matrixInfo.lastY += intMatrixSize;
                    break;
            }

            var xt = matrixInfo.lastX;
            var yt = matrixInfo.lastY;
            int PlaseOnListX = matrixInfo.lastX / intMatrixSize;
            int PlaseOnListY = matrixInfo.lastY / intMatrixSize;

            var a = matrixInfo.info;

            if (!matrixInfo.info.ContainsKey(PlaseOnListX))
                matrixInfo.info.Add(PlaseOnListX, new Dictionary<int, PinAndPoint>());

            if (matrixInfo.info[PlaseOnListX].ContainsKey(PlaseOnListY))
                return;


            if (!matrixInfo.lastMXYonPin.ContainsKey(intPin))
                matrixInfo.lastMXYonPin.Add(intPin, MXY);
            

            matrixInfo.info[PlaseOnListX].Add(PlaseOnListY, new PinAndPoint() { X = matrixInfo.lastX, Y = matrixInfo.lastY, spoint = matrixInfo.lastMXYonPin[intPin], pin = intPin });
            matrixInfo.lastMXYonPin[intPin] += intMatrixSize * intMatrixSize;
            if (matrixInfo.panelSize == 0)
                matrixInfo.panelSize = intMatrixSize;

            new ConnectFile().SerializeJson();

        }




    }
}