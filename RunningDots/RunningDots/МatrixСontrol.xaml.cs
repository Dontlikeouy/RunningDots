
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static RunningDots.ConnectFile;

namespace RunningDots
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class МatrixСontrol : ContentPage
    {

        public МatrixСontrol()
        {
            InitializeComponent();


        }








        ScrollView scrollView;
        ScrollView GaneralScrollView;

        double GSVscroll;
        double ABscroll;

        // Идеи про скролл
        // Можно сравнитать две кодинаты при эвенте move
        // Наложить поверх Scroll и через абстракт смещать . Scroll сделать определенныйм размером


        private void GrawOrScroll_Clicked(object sender, EventArgs e)
        {
            switch (GrawOrScroll.Text)
            {

                case "Draw":
                    {
                        GrawOrScroll.Text = "Scroll";

                        TuchVisual.Touch += SKCanvasView_Touch_1;

                        GSVscroll = GaneralScrollView.ScrollY;
                        AbsoluteLayout.SetLayoutBounds(FrizeScroll, new Rectangle(0, GSVscroll * -1, 1, 1));


                        AbsoluteLayout Stack = StayLa;
                        GaneralGrid.Children.Remove(GaneralScrollView);
                        GaneralScrollView = null;
                        GaneralGrid.Children.Add(Stack);


                        ABscroll = scrollView.ScrollX;
                        AbsoluteLayout.SetLayoutBounds(AbsoluGrid, new Rectangle(ABscroll * -1, 0, PanelGrid.Width, PanelGrid.Height));

                        AbsoluteLayout stack = AbsoForBlack;

                        AllControllsGrid.Children.Remove(scrollView);
                        AllControllsGrid.Children.Add(stack, 0, 2);




                        Grid _AllButtons = AllButtons;
                        GaneralGrid.Children.Remove(AllButtons);
                        GaneralGrid.Children.Add(_AllButtons);



                        break;
                    }
                case "Scroll":
                    {
                        GrawOrScroll.Text = "Draw";
                        TuchVisual.Touch -= SKCanvasView_Touch_1;


                        AbsoluteLayout.SetLayoutBounds(FrizeScroll, new Rectangle(0, 0, 1, 1));

                        GaneralGrid.Children.Add(GaneralScrollView = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = StayLa });


                        var task = Task.Run(() => GaneralScrollView.ScrollToAsync(0, GSVscroll, false));
                        task.Wait();



                        AbsoluteLayout.SetLayoutBounds(AbsoluGrid, new Rectangle(0, 0, PanelGrid.Width, PanelGrid.Height));

                        AllControllsGrid.Children.Add(scrollView = new ScrollView() { Orientation = ScrollOrientation.Horizontal, Content = AbsoForBlack }, 0, 2);

                        task = Task.Run(() => scrollView.ScrollToAsync(ABscroll, 0, false));
                        task.Wait();




                        //Кнопки
                        Grid _AllButtons = AllButtons;
                        GaneralGrid.Children.Remove(AllButtons);
                        GaneralGrid.Children.Add(_AllButtons);



                        break;
                    }
            }
        }










        private void PressToVisualize_Clicked(object sender, EventArgs e)
        {


            int MatrixSize = matrixInfo.panelSize * matrixInfo.info.Count();

            if (MatrixSize == 0) return;

            for (int i = 0; i < MatrixSize; i++)
            {
                PanelGrid.RowDefinitions.Add(new RowDefinition() { Height = 20 });
                PanelGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 20 });

            }

            int R = 0;
            int G = 255;
            int B = 255;
            PanelGrid.Children.Add(new Xamarin.Forms.Shapes.Rectangle() { Fill = new SolidColorBrush(new Color(R, G, B)) }, 30, 0);
            PixelCoordinatess.Add(new Dictionary<int, Dictionary<Tuple<int, int>, PixelForList>>());
            foreach (var pin in matrixInfo.lastMXYonPin.Keys)
            {
                if (!PixelCoordinatess[shot].ContainsKey(pin))
                {
                    PixelCoordinatess[shot].Add(pin,new Dictionary<Tuple<int, int>, PixelForList>());
                }
            }
            //R = 0;
            //G = 255;
            //B = 255;

            //for (int i = 0; i < MatrixSize * 2; i++)
            //{
            //    PanelGrid.Children.Add(new Xamarin.Forms.Shapes.Rectangle() { Fill = new SolidColorBrush(new Color(R, G, B))}, i, 0);
            //}



        }

        private void PanelGrid_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout.SetLayoutBounds(AbsoluGrid, new Rectangle(0, 0, PanelGrid.Width, PanelGrid.Height));
        }
        // РАЗМЕР
        // 31*40+(60-40)
        //31 - ОБЩИЕ КОЛ ВО ПИКСЕЛЕЙ
        // 40 - РАЗМЕР ОДНОГО ПИКСЕЛЯ
        // 60 - 40 -- РАСТОЯНИМЕ МЕЖДУ ПИКСЕЛЯМИ

        // КОРДИНАТЫ ВОЗВРАШЕНИЕ GRID ?
        // 202 / 60 С ОКРУГЛЕНИЕМ ?
        // X = 202
        // 60 -   ПИКСЕЛЬ И РАССТОЯНИЕ





        // как определить как рисовать
        //(202 / 60)*60=180 С ОКРУГЛЕНИЕМ ?
        // 180 - начальная кордината на пиксель

        //Инстументы которые необходимы
        //Пипетка - переключение с помощью долгого зажатия
        //Переключать фон с черного на белый и обртано
        //Маштабирование




        public class PixelForList
        {
            public Xamarin.Forms.Shapes.Rectangle ShapesRectangle { get; set; } = new Xamarin.Forms.Shapes.Rectangle();
            public int R { get; set; }
            public int G { get; set; }
            public int B { get; set; }
        }

        //кадр-pin-X-Y-объект на виртуальной матрице
        List<Dictionary<int, Dictionary<Tuple<int, int>, PixelForList>>> PixelCoordinatess = new List<Dictionary<int, Dictionary<Tuple<int, int>, PixelForList>>>();

        //Трансляция данных по блютуз

        //Отправляем также , но если тот же порт то пропускаем создание
        //Создаем массив в который помещаем в зависимости от порта информацию , а потом объединяем
        private int GetPixel(int X, int Y)
        {
            var nowPoint = matrixInfo.info[X / matrixInfo.panelSize][Y / matrixInfo.panelSize];

            switch (Y % 2)
            {

                case 0:
                    return (nowPoint.spoint + matrixInfo.panelSize * (Y - nowPoint.Y)) - (X - nowPoint.X);
                default:
                    return (nowPoint.spoint + matrixInfo.panelSize * (Y - nowPoint.Y) - matrixInfo.panelSize - 1) + (X - nowPoint.X);
            }

        }
        private void BluetoothConnetion_Clicked(object sender, EventArgs e)
        {
            //@ - началбые конфигурации @6;512 6-пин 512 - колическво пикселей
            //; - без начального спец символа означает пиксель 14;0;255;255;4 пиксель R G B яркость
            //# - переход на следующий кадр
            // storage/emulated/0/Android/data/com.comapyname.runningdots/FileArduino
            string Path = @"/storage/emulated/0/Android/data/com.companyname.runningdots/Txt";

            if (!File.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            using (FileStream ForSR = new FileStream(Path + $"/{FileName}.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                // добавление в файл

                using (StreamWriter writer = new StreamWriter(ForSR))
                {

                    foreach (var shot in PixelCoordinatess)
                    {
                        foreach (var pin in shot)
                        {
                            writer.WriteLine($"@{pin.Key};{matrixInfo.panelSize * matrixInfo.panelSize * matrixInfo.info.Count()}");
                            foreach (var tuple in pin.Value)
                            {
                                writer.WriteLine($"{GetPixel(tuple.Key.Item1, tuple.Key.Item2)};{tuple.Value.R};{tuple.Value.G};{tuple.Value.B};{tuple.Value.ShapesRectangle.Opacity * 10}");


                            }
                        }
                    }

                    writer.Flush();
                }

            }

            //Console.WriteLine("Start");
            //App.BluetoothConnetion.CreateBluetooth();
            //await App.BluetoothConnetion.SendMessage($"@{InfoForMatrix.}");
            //await App.BluetoothConnetion.SendMessage("6;16;2;14;0;255;255;4;");
            //await App.BluetoothConnetion.SendMessage("6;16;2;13;0;255;255;4;");

        }
        //..Трансляция данных по блютуз


        //Переключение на следующий кадр
        int shot = 0;
        private void DoubleLeft_Clicked(object sender, EventArgs e)
        {



            if (shot < 0)
            {
                shot = 0;
            }
            else
            {
                PixelCoordinatess.RemoveAt(shot);
                shot--;
            }

        }

        private void DoubleRigth_Clicked(object sender, EventArgs e)
        {
            shot++;
            PixelCoordinatess.Add(new Dictionary<int, Dictionary<Tuple<int, int>, PixelForList>>());
            foreach (var pin in matrixInfo.lastMXYonPin.Keys)
            {
                if (!PixelCoordinatess[shot].ContainsKey(pin))
                {
                    PixelCoordinatess[shot].Add(pin, new Dictionary<Tuple<int, int>, PixelForList>());
                }
            }
        }
        //..Переключение на следующий кадр

        public void DrawPixel(int X, int Y)
        {
            if (
                Color_R.Text == null || Color_R.Text == "" ||
                Color_G.Text == null || Color_G.Text == "" ||
                Color_B.Text == null || Color_B.Text == "" ||
                Color_Biht.Text == null || Color_Biht.Text == ""
                ) return;

            int R = Convert.ToInt16(Color_R.Text);
            int G = Convert.ToInt16(Color_G.Text);
            int B = Convert.ToInt16(Color_B.Text);
            double Biht = Convert.ToDouble(Color_Biht.Text) / 10;

            int getPin = matrixInfo.info[X / matrixInfo.panelSize][Y / matrixInfo.panelSize].pin;


            if (Biht == 0)
            {

                if (PixelCoordinatess[shot].ContainsKey(getPin))
                {
                    if (PixelCoordinatess[shot][getPin].ContainsKey(Tuple.Create(X, Y)))
                    {
                        PanelGrid.Children.Remove(PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].ShapesRectangle);
                        //if (PixelCoordinatess[shot][getPin].Count() - 1 > 0)
                        //{
                        PixelCoordinatess[shot][getPin].Remove(Tuple.Create(X, Y));
                        //}
                        //else
                        //{
                        //    PixelCoordinatess[shot].Remove(getPin);
                        //}
                    }
                }
            }
            else
            {



                if (!PixelCoordinatess[shot][getPin].ContainsKey(Tuple.Create(X, Y)))
                    PixelCoordinatess[shot][getPin].Add(Tuple.Create(X, Y), new PixelForList());

                Stopwatch stopwatch1 = Stopwatch.StartNew();

                PanelGrid.Children.Remove(PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].ShapesRectangle);
                PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].ShapesRectangle = new Xamarin.Forms.Shapes.Rectangle { Fill = new SolidColorBrush(Color.FromRgb(R, G, B)), Opacity = Biht };
                PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].R = R;
                PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].G = G;
                PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].B = B;  

                PanelGrid.Children.Add(PixelCoordinatess[shot][getPin][Tuple.Create(X, Y)].ShapesRectangle, X, Y);
                stopwatch1.Stop();
                CodS_X.Text = stopwatch1.Elapsed.Milliseconds.ToString();
            }
            Stopwatch stopwatch = Stopwatch.StartNew();

            Xamarin.Forms.Shapes.Rectangle rectangle = SelectPixel;
            PanelGrid.Children.Remove(SelectPixel);
            PanelGrid.Children.Add(rectangle, X, Y);
            stopwatch.Stop();
            CodS_Text1.Text = stopwatch.Elapsed.Milliseconds.ToString();

        }
        private void SKCanvasView_Touch_1(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();

            e.Handled = true;

            if (e.ActionType == SKTouchAction.Pressed || e.ActionType == SKTouchAction.Moved)
            {




                DrawPixel((Convert.ToInt16(e.Location.X) / 70), (Convert.ToInt16(e.Location.Y) / 70));

            }
            stopwatch.Stop();
            CodS_Y.Text = stopwatch.Elapsed.Milliseconds.ToString();

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

            DrawPixel(Grid.GetColumn(SelectPixel), Grid.GetRow(SelectPixel));
        }


        //Кнопка-градиет
        float LocInt = 40;
        SKCanvasView canvasView;
        SKBitmap bmp;
        private void ColorTouch_Touch(object sender, SKTouchEventArgs e)
        {
            e.Handled = true;
            LocInt = e.Location.X;
            if (LocInt < 0) LocInt = 0;
            if (canvasView != null)
            {
                canvasView.PaintSurface -= SimpleCircle_PaintSurface;
                Polz.Children.Remove(canvasView);
            }
            canvasView = new SKCanvasView() { Opacity = 0.7 };
            canvasView.PaintSurface += SimpleCircle_PaintSurface;

            Polz.Children.Add(canvasView);




            var _selectedColor = bmp.GetPixel((int)LocInt, 0);

            Color_R.Text = _selectedColor.Red.ToString();
            Color_G.Text = _selectedColor.Green.ToString();
            Color_B.Text = _selectedColor.Blue.ToString();

            CodS_Type.Text = _selectedColor.ToString();



        }



        private void SimpleCircle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            //var succeed = surface.ReadPixels(info, dstpixels);

            canvas.Clear();


            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Gray.ToSKColor(),
                StrokeWidth = 25
            };
            canvas.DrawCircle(LocInt, 35, 20, paint);

            paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.White.ToSKColor(),

            };

            canvas.DrawCircle(LocInt, 35, 20, paint);





            //_touchCircleFill.Color = _selectedColor;


        }

        private void Gradient_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            bmp = new SKBitmap(info);
            SKCanvas canvas1 = new SKCanvas(bmp);



            SKRect rect = new SKRect(0, 0, info.Width, info.Height);


            using (SKPaint paint = new SKPaint())
            {


                SKColor[] colors = { new Color(255, 0, 0).ToSKColor(), new Color(255, 0, 255).ToSKColor(), new Color(0, 0, 255).ToSKColor(), new Color(0, 255, 255).ToSKColor(), new Color(0, 255, 0).ToSKColor(), new Color(255, 255, 0).ToSKColor(), new Color(255, 0, 0).ToSKColor() };
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(rect.Left, 0),
                                    new SKPoint(rect.Right, 0),
                                    colors,
                                null,
                                SKShaderTileMode.Repeat);



                canvas.DrawRect(info.Rect, paint);
                canvas1.DrawRect(info.Rect, paint);

                //canvas.DrawBitmap(bmp, rect, paint);

            }
        }
        //..Кнопка-градиет



    }



}