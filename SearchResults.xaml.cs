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
using Xceed.Wpf.Toolkit.Core;

namespace UI
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : UserControl
    {
        public SearchResults()
        {
            InitializeComponent();
            MainWindow.searchResults = this;
            ResultsTable.ItemsSource = Results().OrderBy(p => p.Departure);
        }
        public List<Result> Results()
        {
            List<Result> results = new List<Result>();

            foreach (Database.Connection connection in MainWindow.connections)
            {
                Database.Stage stageDeparture = connection.stages.Find(i => i.Departure_Station == MainWindow.searchConditions.FromStationTextbox.Text.ToString());
                Database.Stage stageArrival = connection.stages.Find(i => i.Arrival_Station == MainWindow.searchConditions.ToStationTextbox.Text.ToString());

                double totalConnectionDistance = 0;

                foreach (Database.Stage stage in connection.stages)
                    if (stage.Sequence >= stageDeparture.Sequence && stage.Sequence <= stageArrival.Sequence)
                        totalConnectionDistance += Double.Parse(stage.Distance);

                results.Add(new Result(stageDeparture.Departure_Date,
                                     stageDeparture.Departure_Time,
                                     stageArrival.Arrival_Date,
                                     stageArrival.Arrival_Time,
                                     connection.Connection_Type,
                                     totalConnectionDistance,
                                     connection.Connection_Id));
            }
            return results;
        }

        private void NewSearchButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.searchConditions.Reset();
        }

        public void Update()
        {
            FromStationLabel.Content = Application.Current.Resources["SearchResults_FromStationLabel"] + " " + MainWindow.searchConditions.FromStationTextbox.Text;
            ToStationLabel.Content = Application.Current.Resources["SearchResults_ToStationLabel"] + " " + MainWindow.searchConditions.ToStationTextbox.Text;
            
            if(MainWindow.searchConditions.DepartureRadioButton.IsChecked == true) ResultsTable.ItemsSource = Results().OrderBy(p => p.Departure);
            else ResultsTable.ItemsSource = Results().OrderBy(result => result.Arrival);
        }
        void RowSelected(object sender, RoutedEventArgs e)
        {
            var cell = sender as DataGridCell;
            if (cell.Column.DisplayIndex == 0)
            {
                var selectedRow = (e.OriginalSource as FrameworkElement).DataContext as Result;
                var selectedConnection = (from connection 
                                          in MainWindow.connections 
                                          where connection.Connection_Id == selectedRow.resultId 
                                          select connection).First();
                ConnectionDetails details = new ConnectionDetails((Database.Connection)selectedConnection, 
                                                                    MainWindow.searchConditions.FromStationTextbox.Text.ToString(),
                                                                    MainWindow.searchConditions.ToStationTextbox.Text.ToString());
                details.Show();
            }
        }
        public void ChangeLanguage()
        {
            FromStationLabel.Content = Application.Current.Resources["SearchResults_FromStationLabel"] + " " + MainWindow.searchConditions.FromStationTextbox.Text;
            ToStationLabel.Content = Application.Current.Resources["SearchResults_ToStationLabel"] + " " + MainWindow.searchConditions.ToStationTextbox.Text;
            List<Result> results = ResultsTable.ItemsSource.Cast<Result>().ToList();

            if (CultureInfo.CurrentCulture.Name == "en-EN")
            {
                foreach (Result result in results)
                {
                    result.Details = Application.Current.Resources["SearchResults_Click"].ToString();
                    if (result.Type == "Regionalny") result.Type = Application.Current.Resources["SearchResults_Regional"].ToString();
                    if (result.Type == "Pospieszny") result.Type = Application.Current.Resources["SearchResults_Intercity"].ToString();
                    if (result.Type == "Ekspresowy") result.Type = Application.Current.Resources["SearchResults_Express"].ToString();
                }
            }
            if (CultureInfo.CurrentCulture.Name == "pl-PL")
            {
                foreach (Result result in results)
                {
                    result.Details = Application.Current.Resources["SearchResults_Click"].ToString();
                    if (result.Type == "Regional") result.Type = Application.Current.Resources["SearchResults_Regional"].ToString();
                    if (result.Type == "Intercity") result.Type = Application.Current.Resources["SearchResults_Intercity"].ToString();
                    if (result.Type == "Express") result.Type = Application.Current.Resources["SearchResults_Express"].ToString();
                }
            }

            ResultsTable.ItemsSource = results;
        }
    }

    public class Result
    {
        public Result(string DepartureDate, string DepartureTime,
                     string ArrivalDate, string ArrivalTime,
                     string ConnectionType, double Distance, int ConnectionId)
        {
            Details = Application.Current.Resources["SearchResults_Click"].ToString();
            Departure = DepartureDate + " " + DepartureTime;
            Arrival = ArrivalDate + " " + ArrivalTime;
            if (ConnectionType == "R") Type = Application.Current.Resources["SearchResults_Regional"].ToString();
            if (ConnectionType == "IC") Type = Application.Current.Resources["SearchResults_Intercity"].ToString();
            if (ConnectionType == "EP") Type = Application.Current.Resources["SearchResults_Express"].ToString();
            Time = (DateTime.Parse(Arrival) - DateTime.Parse(Departure)).ToString(@"hh\:mm");
            this.Distance = Distance.ToString() + " km";
            resultId = ConnectionId;
        }
        public int resultId { get; set; }
        public string Details { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Time { get; set; }
        public string Distance { get; set; }
        public string Type { get; set; }
    }
}
