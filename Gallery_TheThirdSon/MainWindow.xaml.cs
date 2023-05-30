using Gallery_TheThirdSon.Data;
using Gallery_TheThirdSon.Forms;
using Gallery_TheThirdSon.Windows;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Gallery_TheThirdSon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public static Models.ImageModel CurrentImage { get; set; } = new();
        public static List<Models.ImageModel> CurrentList { get; set; } = new();
        private void Load()
        {
            ItemsGrid.ItemsSource = null;
            ItemsGrid.ItemsSource = CurrentList;
        }
        public void Load(Models.SaveModel save)
        {
            var query = from s in App.Context.Saves
                        where s.Id == save.Id
                        join savelist in App.Context.SaveLists on s.Id equals savelist.Save
                        join images in App.Context.Images on savelist.Image equals images.Id
                        select images;
            CurrentList = query.ToList();
            ItemsGrid.ItemsSource = null;
            ItemsGrid.ItemsSource = CurrentList;
        }
        private static int Check(int index, int step)
        {
            if (CurrentList.Any())
            {
                if (index + step < 0)
                {
                    return CurrentList.Count - 1;
                }
                else if (index + step > CurrentList.Count - 1)
                {
                    return 0;
                }
                else
                {
                    return index + step;
                }
            }
            else
            {
                return -1;
            }
        }
        private void Step(int step)
        {
            var img = CurrentList.Find(x => x == CurrentImage);
            if (img != null)
            {
                var index = CurrentList.IndexOf(img);
                var going = Check(index, step);
                if (!(going < 0))
                {
                    var uiElement = ItemsGrid.ItemContainerGenerator.ContainerFromIndex(going);
                    var child = VisualTreeHelper.GetChild(uiElement, 0);
                    var button = child as RadioButton;
                    Debug.WriteLine("Found: " + button);
                    button.IsChecked = true;
                    Debug.WriteLine($"Moved to: {img.Id}");
                }
            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog files = new OpenFileDialog()
            {
                Title = "Выберете фото",
                Filter = "Image Files(*.JPEG;*.JPG;*.PNG)|*.JPEG;*.JPG;*.PNG|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg",
                Multiselect = true,
                RestoreDirectory = false
            };
            if (files.ShowDialog() == true)
            {
                foreach (string file in files.FileNames)
                {
                    byte[] data = File.ReadAllBytes(file);
                    var check = App.Context.Images.Where(x => x.Data == data);
                    if (check.Any())
                    {
                        var img = check.FirstOrDefault();
                        if (CurrentList.Contains(img))
                        {
                            MessageBox.Show("Изображение уже в галерее");
                        }
                        else
                        {
                            CurrentList.Add(img);
                        }
                    }
                    else
                    {
                        FileInfo info = new FileInfo(file);
                        //var bitmap = ByteConverter(data);
                        Debug.WriteLine($"Added {file} to gallery");
                        var image = new Models.ImageModel()
                        {
                            Data = data,
                            Title = System.IO.Path.GetFileNameWithoutExtension(file),
                            Extention = info.Extension
                        };
                        CurrentList.Add(image);
                    }
                }
                Load();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var img = App.Context.Images.Find(CurrentImage.Id);
            if (img != null)
            {
                Debug.WriteLine($"Removing: {img.Id}");
                Step(-1);
                CurrentList.Remove(img);
                Load();
            }
        }

        private void Image_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var image = button.DataContext as Models.ImageModel;
            Debug.WriteLine($"Image: {image.Id}");
            if (image != null)
            {
                WindowGrid.DataContext = image;
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            Step(-1);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Step(1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var query = from save in App.Context.Saves
                        where save.DateUpdated <= DateTime.Now
                        orderby save.DateUpdated descending
                        select save;
            if (query.Any())
            {
                Load(query.FirstOrDefault());
            }
            else
            {
                Load();
            }
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            var img = App.Context.Images.Find(CurrentImage.Id);
            if (img != null)
            {
                Debug.WriteLine($"Removing: {img.Id}");
                Step(-1);
                CurrentList.Remove(img);
                Load();
            }
        }

        private void EditImageButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new EditPhotoWin(CurrentImage)
            {
                Owner = this
            };
            win.Show();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sourse = e.OriginalSource;
            if (sourse is Image)
            {
                var win = new EditPhotoWin(CurrentImage)
                {
                    Owner = this
                };
                win.Show();
            }
        }

        private void PreviewScrolViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToHorizontalOffset(scv.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        public void WindowGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CurrentImage = WindowGrid.DataContext as Models.ImageModel;
            var tagsofImage = (from tags in App.Context.TagsLists
                               where tags.Image == CurrentImage.Id
                               join tag in App.Context.Tags on tags.Tag equals tag.Id
                               select tag).ToList().Take(3);
            TagsCollection.Collection = tagsofImage;
        }
    }
}
