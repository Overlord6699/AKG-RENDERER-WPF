using CGA_1_wpf.Controls;
using CGA_1_wpf.CutoffPixelsManagers;
using CGA_1_wpf.Entities;
using CGA_1_wpf.LightningManagers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CGA_1_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IRotation _userInputProcess;
        private DrawMethodsAbstract _visualisator;
        private ICutoffPixelsManager _cutoffManager;
        private ILightningManager _lightningManager;
        private Parameters _parameters;
        private Entities.Model _model;

        const float CUTOFF_GAP = -0.3f;
        const int DPI = 96;
        const int COLOR_MULTIPLIER = 200;

        double width, height;

        public MainWindow()
        {
            InitializeComponent();

            MM_cb.IsEnabled = false;
            RM_cb.IsEnabled = false;
            MC_cb.IsEnabled = false;
            RC_cb.IsEnabled = false;

            _cutoffManager = new SimpleCutoffManager(CUTOFF_GAP);
            _lightningManager = new LambertLightningManager(COLOR_MULTIPLIER);

            _userInputProcess = new MoveModel();
            //_visualisator = new DrawLine();
            _visualisator = new DrawPlane(_cutoffManager, _lightningManager);
        }

        private void Draw(int width, int height)
        {
            // Создание новой палитры с новыми цветами
            Color[] newColors = new Color[]
            {
                Colors.WhiteSmoke
            };
            BitmapPalette newPalette = new BitmapPalette(newColors);

            var bitmap = new WriteableBitmap(width, height, DPI, DPI, PixelFormats.Bgra32, newPalette);

            Model modelMain = _model.Clone() as Model;
            TransformationMatrices.TransformFromModelToView(modelMain, _parameters);
            _visualisator.DrawModel(bitmap, modelMain, _parameters, TransformationMatrices.TransformFromModelToWorld(_model.Clone() as Model, _parameters));

            img.Source = bitmap;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_parameters != null)
            {
                _userInputProcess.Process(_parameters, e);
                //var width = picContainer.ActualWidth;
                //var height = picContainer.ActualHeight;

                Draw((int)width, (int)height);
            }
        }

        private void MM_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (MM_cb.IsChecked == true)
            {
                _userInputProcess = new MoveModel();
                RM_cb.IsChecked = false;
                MC_cb.IsChecked = false;
                RC_cb.IsChecked = false;
            }
        }

        private void RM_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (RM_cb.IsChecked == true) 
            { 
                _userInputProcess = new RotateModel();
                MM_cb.IsChecked = false;
                MC_cb.IsChecked = false;
                RC_cb.IsChecked = false;
            }
        }

        private void MC_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (MC_cb.IsChecked == true)
            {
                _userInputProcess = new MoveCamera();
                MM_cb.IsChecked = false;
                RM_cb.IsChecked = false;
                RC_cb.IsChecked = false;
            }
        }

        private void RC_cb_Checked(object sender, RoutedEventArgs e)
        {
            if (RC_cb.IsChecked == true)
            {
                _userInputProcess = new RotateCamera();
                MM_cb.IsChecked = false;
                RM_cb.IsChecked = false;
                MC_cb.IsChecked = false;
            }
        }

        private void Grid_cb_Checked(object sender, RoutedEventArgs e)
        {
            if(Grid_cb.IsChecked == true)
            {
                //panel3D1.RendererSettings.ShowXZGrid = chkShowXZGrid.Checked; 
                //panel3D1.Invalidate();
            }
        }

        public static string ChooseFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            openFileDialog.Filter = "Object files (*.obj) | *.obj";

            if (openFileDialog.ShowDialog() != null)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MM_cb.IsEnabled = true;
            RM_cb.IsEnabled = true;
            MC_cb.IsEnabled = true;
            RC_cb.IsEnabled = true;

            MM_cb.IsChecked = true;

            _model = ReadObj.ReadObjFile(ChooseFile());
            if (_model != null)
            {
                var width = picContainer.ActualWidth;
                var height = picContainer.ActualHeight;
                _parameters = new Parameters(width, height);

                Draw((int)width, (int)height);
            }

            width = picContainer.ActualWidth;
            height = picContainer.ActualHeight;
        }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point oldMousePosition = e.GetPosition(img);

            MouseControls.getMouseButtons(e, out bool left, out bool right);

            if (left && right)
            {
                //oldCameraPosition = camera.Position;
                Cursor = Cursors.SizeNS;
            }
            else if (left)
            {
                //oldCameraRotation = camera.Rotation;
                Cursor = Cursors.None;
            }
            else if (right)
            {
                //oldCameraPosition = camera.Position;
                Cursor = Cursors.SizeAll;
            }
        }
    }
}
