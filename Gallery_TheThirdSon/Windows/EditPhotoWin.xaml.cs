using Gallery_TheThirdSon.Data;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Gallery_TheThirdSon.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditPhotoWin.xaml
    /// </summary>
    public partial class EditPhotoWin : Window
    {
        public EditPhotoWin(Models.ImageModel img)
        {
            InitializeComponent();
            CurrentImg = img;
            DataContext = CurrentImg;
        }
        public static Models.ImageModel CurrentImg { get; set; }
        private void Load()
        {
            var tags = from links in App.Context.TagsLists
                       where links.Image == CurrentImg.Id
                       join tag in App.Context.Tags on links.Tag equals tag.Id
                       select tag;
            TagsContainer.ItemsSource = tags.ToList();
            var alltags = from tag in App.Context.Tags
                       select tag.Title;
            SearchTagCB.ItemsSource = alltags.ToList();
        }
        private void RemoveTagButton_Click(object sender, RoutedEventArgs e)
        {
            var dt = (sender as Button).DataContext;
            if (dt != null)
            {
                var tag = dt as Models.TagModel;
                var link = App.Context.TagsLists.Where(x => x.Image == CurrentImg.Id).Where(x => x.Tag == tag.Id).FirstOrDefault();
                App.Context.TagsLists.Remove(link);
                App.Context.SaveChanges();
                Load();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Context.SaveChanges();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var win = Owner as MainWindow;
            win.WindowGrid.DataContext = CurrentImg;
            var tagsofImage = (from tags in App.Context.TagsLists
                               where tags.Image == CurrentImg.Id
                               join tag in App.Context.Tags on tags.Tag equals tag.Id
                               select tag).ToList().Take(3);
            win.TagsCollection.Collection = tagsofImage;
            Close();
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            var title = SearchTagCB.Text;
            if (title.Length > 0)
            {
                var tag = App.Context.Tags.Where(x => x.Title == title).FirstOrDefault();
                if (tag == null)
                {
                    tag = new Models.TagModel()
                    {
                        Title = title,
                    };
                    App.Context.Tags.Add(tag);
                }
                var TagLink = new Models.TagsList()
                {
                    Image = CurrentImg.Id,
                    Tag = tag.Id
                };
                App.Context.TagsLists.Add(TagLink);
                App.Context.SaveChanges();
                Load();
            }
            else
            {
                Debug.WriteLine("Tag must be at least 1 letter lenght");
            }
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            SidePanel.Visibility = Visibility.Hidden;
            HideGrid.Visibility = Visibility.Hidden;
            Background = new SolidColorBrush(Colors.Black);
        }

        private void WindowGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SidePanel.Visibility = Visibility.Visible;
            HideGrid.Visibility = Visibility.Visible;
            Background = new SolidColorBrush(Colors.Black) { Opacity = 0.65};
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                SidePanel.Visibility = Visibility.Visible;
                HideGrid.Visibility = Visibility.Visible;
                Background = new SolidColorBrush(Colors.Black) { Opacity = 0.65 };
            }
        }
    }
}
