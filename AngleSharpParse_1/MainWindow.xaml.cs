using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using AngleSharp;

namespace AngleSharpParse_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Boolean change_sort_type = false;

        public MainWindow()
        {
            InitializeComponent();
            Parse();
        }

        public class TableStruct
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Time { get; set; }
            public string Link { get; set; }
        }

        public async void Parse()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = "https://habr.com/ru/";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(address);

            var Title = document.QuerySelectorAll("h2.post__title").Select(m => m.TextContent);
            var Author = document.QuerySelectorAll("span.user-info__nickname").Select(m => m.TextContent);
            var Time = document.QuerySelectorAll("span.post__time").Select(m => m.TextContent);
            var Link = document.QuerySelectorAll("h2.post__title>a[href]").Where(m => m.Attributes["href"]!=null).Select(m =>m.Attributes["href"].Value);

            for (int i = 0; i < Title.Count(); i++)
                listview1.Items.Add(
                    new TableStruct{
                        Title =  Title.ElementAt(i),
                        Author = Author.ElementAt(i),
                        Time = Time.ElementAt(i),
                        Link = Link.ElementAt(i)   });
            
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            
            var h = e.OriginalSource as GridViewColumnHeader;
            if (h != null)
            {
                var propertyName = h.Column.GetValue(TextSearch.TextPathProperty) as string;
                var cvs = listview1.ItemsSource as ICollectionView ??
                    CollectionViewSource.GetDefaultView(listview1.ItemsSource) ??
                    listview1.Items;
                if (cvs != null)
                {
                    if (!change_sort_type)
                    {
                        cvs.SortDescriptions.Clear();
                        cvs.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Ascending));
                        change_sort_type = !change_sort_type;
                    }
                    else
                    {
                        cvs.SortDescriptions.Clear();
                        cvs.SortDescriptions.Add(new SortDescription(propertyName, ListSortDirection.Descending));
                        change_sort_type = !change_sort_type;
                    }
                }
            }
        }
    }
}
