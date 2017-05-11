//*******************************************
//
// (c) Copyright 2010 Cody Neuburger
//       All rights reserved.
//
//*******************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.IO;

namespace Stars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region variables
        static Random rand = new Random();
        static Color Black = Color.FromArgb(255, 0, 0, 0);
        static Color White = Color.FromArgb(255, 255, 255, 255);
        static Color Red = Color.FromArgb(255, 255, 0, 0);
        static Color Green = Color.FromArgb(255, 0, 255, 0);
        static Color Blue = Color.FromArgb(255, 0, 0, 255);
        static Color Cyan = Color.FromArgb(255, 0, 255, 255);
        static Color Magenta = Color.FromArgb(255, 255, 0, 255);
        static Color Yellow = Color.FromArgb(255, 255, 255, 0);
        static Star previewStar = new Star();
        static double rotation = 0;
        static double size = 40;
        static List<Color> chosenColors = new List<Color>();

        bool solidMode = true;
        bool linearGradiantMode = false;
        bool radialGradiantMode = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            SolidColorBrush scBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 250));
            previewStar = new Star(scBrush, 40);
            previewStar.center = new Point(0, 0);
        }
        public void drawPreviewStar(Canvas c, Star s, double degrees)
        {
            s.resize(40);

            Polygon star = new Polygon();
            star.Fill = s.brush;
            star.Points = s.Points;
            RotateTransform rt = new RotateTransform(degrees);
            star.RenderTransform = rt;
            Canvas.SetLeft(star, c.Width / 2);
            Canvas.SetTop(star, c.Height / 2);
            c.Children.Add(star);
        }

        public void drawStar(Canvas c, Star s, double degrees, Point p)
        {
            s.resize(size);

            Polygon star = new Polygon();
            star.Fill = s.brush;
            star.Points = s.Points;
            RotateTransform rt = new RotateTransform(degrees);
            star.RenderTransform = rt;
            Canvas.SetLeft(star, p.X);
            Canvas.SetTop(star, p.Y);
            c.Children.Add(star);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            canvasPreview.Children.Clear();
            DropShadowEffect dsEffect = new DropShadowEffect();
            dsEffect.ShadowDepth = 3;
            border1.Effect = dsEffect;
            dsEffect.ShadowDepth = 2;
            border2.Effect = dsEffect;
            dsEffect.ShadowDepth = 1;
            border3.Effect = dsEffect;

            SolidColorBrush scBrush = new SolidColorBrush(Color.FromArgb(255, 0, 120, 250));
            drawPreviewStar(canvasPreview, previewStar, rotation);

            addrndBlob();
            scBrush = new SolidColorBrush(Black);
            newColor(scBrush);
            scBrush = new SolidColorBrush(White);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Red);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Green);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Blue);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Magenta);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Yellow);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Cyan);
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 120, 120, 255));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 255, 120, 120));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 120, 255, 120));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 60, 255, 60));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 60, 60, 255));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 255, 60, 60));
            newColor(scBrush);
            scBrush = new SolidColorBrush(Color.FromArgb(255, 120, 30, 30));
            newColor(scBrush);
        }

        DockPanel lastColorsDockPanel = new DockPanel();
        DockPanel filler = new DockPanel();
        DockPanel fillerBlob = new DockPanel();
        List<Border> blobs = new List<Border>();
        List<DockPanel> rows = new List<DockPanel>();
        private void newColor(Brush brush)
        {
            //blob
            DropShadowEffect dsEffect = new DropShadowEffect();
            dsEffect.ShadowDepth = 0;
            Border blob = new Border();
            blob.Background = brush;
            blob.BorderBrush = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            blob.CornerRadius = new CornerRadius(2);
            blob.BorderThickness = new Thickness(1);
            blob.Width = 20;
            blob.Height = 20;
            blob.Margin = new Thickness(2);
            blob.Effect = dsEffect;
            blob.MouseLeftButtonDown += new MouseButtonEventHandler(blob_MouseLeftButtonDown);
            blob.MouseRightButtonDown += new MouseButtonEventHandler(blob_MouseRightButtonDown);
            blob.MouseEnter += new MouseEventHandler(blob_MouseEnter);
            blob.MouseLeave += new MouseEventHandler(blob_MouseLeave);
            blob.MouseDown += new MouseButtonEventHandler(blob_MouseDown);
            blob.MouseUp += new MouseButtonEventHandler(blob_MouseUp);

            colorsDockPanel.Children.Remove(filler);
            lastColorsDockPanel.Children.Remove(fillerBlob);
            if (colorsDockPanel.Children.Count == 0 || lastColorsDockPanel.Children.Count >= 4)
            {
                DockPanel dp = new DockPanel();
                dp.Height = colorsDockPanel.Height / 4;
                dp.Width = colorsDockPanel.Width;
                DockPanel.SetDock(dp, Dock.Top);
                colorsDockPanel.Children.Add(dp);
                lastColorsDockPanel = dp;
            }

            DockPanel.SetDock(blob, Dock.Left);
            lastColorsDockPanel.Children.Add(blob);
            //filler blob
            if (lastColorsDockPanel.Children.Count > 0) fillerBlob.Width = lastColorsDockPanel.Width - blob.Width * lastColorsDockPanel.Children.Count;
            else fillerBlob.Width = lastColorsDockPanel.Width;
            fillerBlob.Height = blob.Height;
            DockPanel.SetDock(fillerBlob, Dock.Left);
            lastColorsDockPanel.Children.Add(fillerBlob);
            //filler panel
            if (colorsDockPanel.Children.Count > 0) filler.Height = colorsDockPanel.Height - lastColorsDockPanel.Height * colorsDockPanel.Children.Count;
            else filler.Height = colorsDockPanel.Height;
            filler.Width = colorsDockPanel.Width;
            DockPanel.SetDock(filler, Dock.Top);
            colorsDockPanel.Children.Add(filler);
        }
        private void addrndBlob()
        {
            //blob
            DropShadowEffect dsEffect = new DropShadowEffect();
            dsEffect.ShadowDepth = 0;
            Border rndblob = new Border();
            rndblob.Background = rndColorBlob.Background;
            rndblob.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            rndblob.CornerRadius = new CornerRadius(2);
            rndblob.BorderThickness = new Thickness(1);
            rndblob.Width = 20;
            rndblob.Height = 20;
            rndblob.Margin = new Thickness(2);
            rndblob.Effect = dsEffect;
            rndblob.MouseLeftButtonDown += new MouseButtonEventHandler(rndblob_MouseLeftButtonDown);
            rndblob.MouseEnter += new MouseEventHandler(blob_MouseEnter);
            rndblob.MouseLeave += new MouseEventHandler(blob_MouseLeave);
            rndblob.MouseDown += new MouseButtonEventHandler(blob_MouseDown);
            rndblob.MouseUp += new MouseButtonEventHandler(blob_MouseUp);

            colorsDockPanel.Children.Remove(filler);
            lastColorsDockPanel.Children.Remove(fillerBlob);
            if (colorsDockPanel.Children.Count == 0 || lastColorsDockPanel.Children.Count >= 4)
            {
                DockPanel dp = new DockPanel();
                dp.Height = colorsDockPanel.Height / 4;
                dp.Width = colorsDockPanel.Width;
                DockPanel.SetDock(dp, Dock.Top);
                colorsDockPanel.Children.Add(dp);
                lastColorsDockPanel = dp;
            }

            DockPanel.SetDock(rndblob, Dock.Left);
            lastColorsDockPanel.Children.Add(rndblob);
            //filler blob
            if (lastColorsDockPanel.Children.Count > 0) fillerBlob.Width = lastColorsDockPanel.Width - rndblob.Width * lastColorsDockPanel.Children.Count;
            else fillerBlob.Width = lastColorsDockPanel.Width;
            fillerBlob.Height = rndblob.Height;
            DockPanel.SetDock(fillerBlob, Dock.Left);
            lastColorsDockPanel.Children.Add(fillerBlob);
            //filler panel
            if (colorsDockPanel.Children.Count > 0) filler.Height = colorsDockPanel.Height - lastColorsDockPanel.Height * colorsDockPanel.Children.Count;
            else filler.Height = colorsDockPanel.Height;
            filler.Width = colorsDockPanel.Width;
            DockPanel.SetDock(filler, Dock.Top);
            colorsDockPanel.Children.Add(filler);
        }

        #region random tools
        static Color randColor()
        {
            return Color.FromRgb(
                (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256));
        }

        Brush randLGBrush()
        {
            Brush result = new LinearGradientBrush(randGSCollection(), rand.NextDouble() * Math.PI * 2.0);
            return result;
        }

        Brush randSCBrush()
        {
            Brush result = new SolidColorBrush(randColor());
            return result;
        }

        GradientStopCollection randGSCollection()
        {
            GradientStopCollection gsc = new GradientStopCollection();
            int randSize = rand.Next(2,10);
            for (int i = 2; i < randSize; i++)
            {
                GradientStop gs = new GradientStop();
                gs.Color = randColor();
                gs.Offset = rand.NextDouble();
                gsc.Add(gs);
            }
            return gsc;
        }

        Brush randRGBrush()
        {
            Brush result = new RadialGradientBrush(randGSCollection());
            return result;
        }
        #endregion
        #region blob events
        void rndblob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (solidMode)
            {
                previewStar.brush = randSCBrush();
            }else
            if (linearGradiantMode)
            {
                previewStar.brush = randLGBrush();
            }else
            if (radialGradiantMode)
            {
                previewStar.brush = randRGBrush();
            }
            canvasPreview.Children.Clear();
            drawPreviewStar(canvasPreview, previewStar, rotation);
        }
        void blob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            elementMouseUpEffect((Border)sender);
        }

        void blob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            elementMouseDownEffect((Border)sender);
        }

        void blob_MouseLeave(object sender, MouseEventArgs e)
        {
            elementMouseLeaveEffect((Border)sender);
        }

        void blob_MouseEnter(object sender, MouseEventArgs e)
        {
            elementMouseEnterEffect((Border)sender);
        }

        void blob_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        void blob_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            previewStar.brush = ((Border)sender).Background;
            canvasPreview.Children.Clear();
            drawPreviewStar(canvasPreview, previewStar, rotation);
        }
        #endregion
        #region gradiant buttons
        private void elementMouseDownEffect(UIElement element)
        {
            DropShadowEffect dse = new DropShadowEffect();
            dse.ShadowDepth = 1;
            dse.BlurRadius = 1;
            element.Effect = dse;
        }
        private void elementMouseUpEffect(UIElement element)
        {
            DropShadowEffect dse = new DropShadowEffect();
            dse.ShadowDepth = 1;
            element.Effect = dse;
            DoubleAnimation da = new DoubleAnimation(3, 8, new Duration(TimeSpan.FromSeconds(.2)));
            element.Effect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, da);
        }
        private void elementMouseEnterEffect(UIElement element)
        {
            DropShadowEffect dse = new DropShadowEffect();
            dse.ShadowDepth = 1;
            element.Effect = dse;
            DoubleAnimation da = new DoubleAnimation(3, 8, new Duration(TimeSpan.FromSeconds(.02)));
            element.Effect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, da);
        }
        private void elementMouseLeaveEffect(UIElement element)
        {
            DropShadowEffect dse = new DropShadowEffect();
            dse.ShadowDepth = 1;
            element.Effect = dse;
            DoubleAnimation da = new DoubleAnimation(8, 3, new Duration(TimeSpan.FromSeconds(.02)));
            element.Effect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, da);
        }
        private void borderSolid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            elementMouseDownEffect(borderSolid);
            if (linearGradiantMode) elementMouseLeaveEffect(borderLinearGradiant);
            if (radialGradiantMode) elementMouseLeaveEffect(borderRadialGradiant);
            solidMode = true;
            linearGradiantMode = false;
            radialGradiantMode = false;
        }
        private void borderSolid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            elementMouseUpEffect(borderSolid);
        }
        private void borderSolid_MouseEnter(object sender, MouseEventArgs e)
        {
            elementMouseEnterEffect(borderSolid);
        }
        private void borderSolid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!solidMode)
            {
                elementMouseLeaveEffect(borderSolid);
            }
        }
        private void borderLinearGradiant_MouseDown(object sender, MouseButtonEventArgs e)
        {
            elementMouseDownEffect(borderLinearGradiant);
            if (solidMode) elementMouseLeaveEffect(borderSolid);
            if (radialGradiantMode) elementMouseLeaveEffect(borderRadialGradiant);
            solidMode = false;
            linearGradiantMode = true;
            radialGradiantMode = false;
        }
        private void borderLinearGradiant_MouseEnter(object sender, MouseEventArgs e)
        {
            elementMouseEnterEffect(borderLinearGradiant);
        }
        private void borderLinearGradiant_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!linearGradiantMode)
            {
                elementMouseLeaveEffect(borderLinearGradiant);
            }
        }
        private void borderLinearGradiant_MouseUp(object sender, MouseButtonEventArgs e)
        {
            elementMouseUpEffect(borderLinearGradiant);
        }
        private void borderRadialGradiant_MouseDown(object sender, MouseButtonEventArgs e)
        {
            elementMouseDownEffect(borderRadialGradiant);
            if(solidMode)elementMouseLeaveEffect(borderSolid);
            if(linearGradiantMode)elementMouseLeaveEffect(borderLinearGradiant);
            solidMode = false;
            linearGradiantMode = false;
            radialGradiantMode = true;
        }
        private void borderRadialGradiant_MouseEnter(object sender, MouseEventArgs e)
        {
            elementMouseEnterEffect(borderRadialGradiant);
        }
        private void borderRadialGradiant_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!radialGradiantMode)
            {
                elementMouseLeaveEffect(borderRadialGradiant);
            }
        }
        private void borderRadialGradiant_MouseUp(object sender, MouseButtonEventArgs e)
        {
            elementMouseUpEffect(borderRadialGradiant);
        }
        #endregion

        private void sliderPoints_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            previewStar.changeNumPoints((int)sliderPoints.Value);
            canvasPreview.Children.Clear();

            drawPreviewStar(canvasPreview, previewStar, rotation);
        }

        private void sliderSpin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rotation = sliderSpin.Value;
            canvasPreview.Children.Clear();
            drawPreviewStar(canvasPreview, previewStar, rotation);
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(canvas1);
            Star newStar = new Star(previewStar.brush, size, previewStar.numPoints);
            drawStar(canvas1, previewStar, rotation, clickPoint);
        }

        private void sliderSize_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            size = sliderSize.Value;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap targetBitmap = new RenderTargetBitmap((int)canvas1.ActualWidth, (int)canvas1.ActualHeight, 96d, 96d, PixelFormats.Default); targetBitmap.Render(canvas1);
            
            // add the RenderTargetBitmap to a Bitmapencoder
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

            //save dialog box
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "stars"; // Default file name
            dlg.DefaultExt = ".bmp"; // Default file extension
            dlg.Filter = "bmp pictures (.bmp)|*.bmp"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                // save file to disk
                FileStream fs = File.Open(filename, FileMode.OpenOrCreate);
                encoder.Save(fs);
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //print dialog box    
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(canvas1, "Star Canvas");
            }
        }
    }
}
