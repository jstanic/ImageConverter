using ImageConverterClient.Models;
using ImageConverterClient.Services;
using Microsoft.Win32;
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

namespace ImageConverterClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ImageWrapper> _imageWrappers;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectImages_Click(object sender, RoutedEventArgs e)
        {
            grdLoadedFiles.ItemsSource = null;
            
            //We select the images from the folder and then create wrappers for them just for pretty display in the grid
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "ImageFiles|*.jpg;";
            openFileDialog.Title = "Select images to import to TIFF";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileNames = openFileDialog.FileNames.ToList();
                try
                {
                    _imageWrappers = ImageConverterClientService.GetImageWrappers(fileNames);
                    grdLoadedFiles.ItemsSource = _imageWrappers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (fileNames?.Count > 0)
                    {
                        btnConvertImages.Visibility = Visibility.Visible;
                        txtFileName.Visibility = Visibility.Visible;
                        grdLoadedFiles.Visibility = Visibility.Visible;
                        lblFileName.Visibility = Visibility.Visible;
                    }
                }
            }
                
        }

        private void btnConvertImages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Controls disabled until the data is converted
                btnConvertImages.IsEnabled = false;
                txtFileName.IsEnabled = false;
                btnSelectImages.IsEnabled = false;
                if (ImageConverterClientService.ConvertData(_imageWrappers, txtFileName.Text))
                    MessageBox.Show("File saved!", "Completed!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnConvertImages.IsEnabled = true;
                txtFileName.IsEnabled = true;
                btnSelectImages.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSelectImages.Visibility = Visibility.Visible;
            btnConvertImages.Visibility = Visibility.Hidden;
            txtFileName.Visibility = Visibility.Hidden;
            grdLoadedFiles.Visibility = Visibility.Hidden;
            lblFileName.Visibility = Visibility.Hidden;
        }
    }
}
