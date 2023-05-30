using Gallery_TheThirdSon.Data;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gallery_TheThirdSon.Forms
{
    /// <summary>
    /// Логика взаимодействия для SaveForm.xaml
    /// </summary>
    public partial class SaveForm : Window
    {
        public SaveForm(List<Models.ImageModel> list)
        {
            InitializeComponent();
            if (list.Any())
            {
                ToSave = list;
            }
            else
            {
                MessageBox.Show("No images to save");
                Close();
            }
        }
        public static Models.SaveModel LocalSelected { get; set; }
        public static List<Models.ImageModel> ToSave { get; set; } = new();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (LocalSelected != null)
            {
                if(LocalSelected.Title != Title.Text)
                {
                    LocalSelected = new()
                    {
                        Title = Title.Text,
                        DateUpdated = DateTime.Now,
                    };
                    App.Context.Saves.Add(LocalSelected);
                }
                else
                {
                    LocalSelected.DateUpdated = DateTime.Now;
                    App.Context.Saves.Update(LocalSelected);
                }
                var mysavelist = App.Context.SaveLists.Where(x => x.Save == LocalSelected.Id).ToList();
                App.Context.SaveLists.RemoveRange(mysavelist);
            }
            else
            {
                LocalSelected = new()
                {
                    Title = Title.Text,
                    DateUpdated = DateTime.Now
                };
                App.Context.Saves.Add(LocalSelected);
            }
            foreach (var item in ToSave)
            {
                if(App.Context.Images.Any(x => x.Data == item.Data))
                {
                    Debug.WriteLine($"Trying to update {item.Title}");
                    var id = App.Context.Images.Where(x => x.Data == item.Data).Select(x => x.Id).FirstOrDefault();
                    Debug.WriteLine($"This item has id {item.Id}");
                    item.Id = id;
                    Debug.WriteLine($"But in the DataBase its id is {id}");
                    App.Context.Images.Update(item);
                }
                else
                {
                    Debug.WriteLine($"Trying to add {item.Title}");
                    App.Context.Images.Add(item);
                }
                var saveList = new Models.SavesList()
                {
                    Save = LocalSelected.Id,
                    Image = item.Id
                };
                App.Context.SaveLists.Add(saveList);
            }
            App.Context.SaveChanges();
            Close();
        }

        private void Select_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            var save = button.DataContext as Models.SaveModel;
            LocalSelected = save;
            Title.Text = save.Title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SavesControl.ItemsSource = App.Context.Saves.ToList();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            //if (LocalSelected == null)
            //{
            //    MessageBox.Show("Select a save");
            //}
            //else
            //{
            //    App.SaveSelected = LocalSelected;
            //    var win = (MainWindow)Owner;
            //    win.Load(App.SaveSelected);
            //    Close();
            //}
        }
    }
}
