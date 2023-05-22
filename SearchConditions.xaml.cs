using MaterialDesignThemes.Wpf.Transitions;
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

namespace UI
{
    /// <summary>
    /// Interaction logic for SearchConditions.xaml
    /// </summary>
    public partial class SearchConditions : UserControl
    {
        List<string> stations = new List<string>();
        public SearchConditions()
        {
            InitializeComponent();
            MainWindow.searchConditions = this;
            ConnectionDatePicker.Value= DateTime.Now;
            ConnectionTimePicker.Value = DateTime.Now;
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.connections = Database.Select.Connections(FromStationTextbox.Text, ToStationTextbox.Text,
                                                          ConnectionDatePicker.Value ?? DateTime.Now, ConnectionTimePicker.Value ?? DateTime.Now,
                                                          DepartureRadioButton.IsChecked ?? false, ExpressCheckBox.IsChecked ?? false,
                                                          IntercityCheckBox.IsChecked ?? false, RegionalCheckBox.IsChecked ?? false,
                                                          WiFiCheckBox.IsChecked ?? false, BicycleCarriageCheckBox.IsChecked ?? false);
            MainWindow.searchConditions = this;
            MainWindow.searchResults.Update();
        }
        public void Stations()
        {
            FromStationTextbox.ItemsSource = Database.Select.Stations();
            ToStationTextbox.ItemsSource = Database.Select.Stations();
        }
        public void Reset()
        {
            FromStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 0.25 };
            ToStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 0.25 };

            FromStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();
            ToStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();

            ConnectionDatePicker.Value = DateTime.Now;
            ConnectionTimePicker.Value = DateTime.Now;
            DepartureRadioButton.IsChecked = true;
            ExpressCheckBox.IsChecked = true;
            IntercityCheckBox.IsChecked = true;
            RegionalCheckBox.IsChecked = true;
            WiFiCheckBox.IsChecked = false;
            BicycleCarriageCheckBox.IsChecked = false;
        }

        private void FromStationTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(FromStationTextbox.ItemsSource == null)
            {
                stations = Database.Select.Stations();
                FromStationTextbox.ItemsSource = stations;
                ToStationTextbox.ItemsSource = stations;
            }
            if (FromStationTextbox.Text == "Wpisz stację" || FromStationTextbox.Text == "Enter the station")
            {
                FromStationTextbox.Text = "";
                FromStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 1 };
            }
        }
        private void ToStationTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FromStationTextbox.ItemsSource == null)
            {
                stations = Database.Select.Stations();
                FromStationTextbox.ItemsSource = stations;
                ToStationTextbox.ItemsSource = stations;
            }
            if (ToStationTextbox.Text == "Wpisz stację" || ToStationTextbox.Text == "Enter the station")
            {
                ToStationTextbox.Text = "";
                ToStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 1 };
            }
        }
        private void FromStationTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FromStationTextbox.Text == "")
            {
                FromStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 0.25 };
                FromStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();
            }
        }
        private void ToStationTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ToStationTextbox.Text == "")
            {
                ToStationTextbox.Foreground = new SolidColorBrush(Colors.White) { Opacity = 0.25 };
                ToStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();
            }
        }
        public void ChangeLanguage()
        {
            if(FromStationTextbox.Text == "Wpisz stację" || FromStationTextbox.Text == "Enter the station")
            {
                FromStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();
            }
            if (ToStationTextbox.Text == "Wpisz stację" || ToStationTextbox.Text == "Enter the station")
            {
                ToStationTextbox.Text = Application.Current.Resources["SearchConditions_DefaultTextbox"].ToString();
            }
        }
        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            string temp = FromStationTextbox.Text;
            FromStationTextbox.Text = ToStationTextbox.Text;
            ToStationTextbox.Text = temp;
        }
    }
}
