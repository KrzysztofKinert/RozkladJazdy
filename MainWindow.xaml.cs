using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SearchConditions searchConditions;
        public static SearchResults searchResults;
        public static List<Database.Connection> connections = new List<Database.Connection>();
        public MainWindow()
        {
            InitializeComponent();
            SelectCulture("pl-PL");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult key = MessageBox.Show(Application.Current.Resources["MainWindow_Closing"].ToString(), 
                                                                  Application.Current.Resources["MainWindow_Confirm"].ToString(),
                                                                  MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);
        }
        private void PL_Click(object sender, RoutedEventArgs e)
        {
            SelectCulture("pl-PL");
            searchResults.ChangeLanguage();
            searchConditions.ChangeLanguage();
        }

        private void ENG_Click(object sender, RoutedEventArgs e)
        {
            SelectCulture("en-EN");
            searchResults.ChangeLanguage();
            searchConditions.ChangeLanguage();
        }

        public static void SelectCulture(string culture)
        {
            var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

            string requestedCulture = string.Format("StringResources.{0}.xaml", culture);
            var resourceDictionary = dictionaryList.
                FirstOrDefault(d => d.Source.OriginalString == requestedCulture);

            if (resourceDictionary == null)
            {           
                requestedCulture = "StringResources.xaml";
                resourceDictionary = dictionaryList.
                    FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
            }

            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
   
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
