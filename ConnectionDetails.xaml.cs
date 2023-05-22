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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for SzczegolyPolaczenia.xaml
    /// </summary>
    public partial class ConnectionDetails : Window
    {
        Database.Connection connection;
        string departureStation;
        string arrivalStation;
        public ConnectionDetails(Database.Connection connection, string departureStation, string arrivalStation)
        {
            InitializeComponent();
            this.connection = connection;
            this.departureStation = departureStation;
            this.arrivalStation = arrivalStation;
            Details();
            IntermediateTable.ItemsSource = findIntermediateStations();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void Details()
        {
            Database.Stage stageDeparture = connection.stages.Find(stage => stage.Departure_Station == departureStation);
            Database.Stage stageArrival = connection.stages.Find(stage => stage.Arrival_Station == arrivalStation);
            DateTime departure = DateTime.Parse(stageDeparture.Departure_Date + " " + stageDeparture.Departure_Time);
            DateTime arrival = DateTime.Parse(stageArrival.Arrival_Date + " " + stageArrival.Arrival_Time);
            double distance = 0;

            foreach(Database.Stage stage in connection.stages)
                if (stage.Sequence >= stageDeparture.Sequence && stage.Sequence <= stageArrival.Sequence) distance += Double.Parse(stage.Distance);

            ConnectionNameLabel.Content = connection.Train_Name;
            if (connection.Connection_Type == "R") ConnectionTypeLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_Regional");
            if (connection.Connection_Type == "IC") ConnectionTypeLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_Intercity");
            if (connection.Connection_Type == "EP") ConnectionTypeLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_Express");

            ConnectionFromLabel.Content = departureStation;
            ConnectionDepartureDateLabel.Content = departure.ToString("g");
            ConnectionToLabel.Content = arrivalStation;
            ConnectionArrivalDateLabel.Content = arrival.ToString("g");
            ConnectionTimeLabel.Content = (arrival - departure).ToString(@"hh\:mm");
            ConnectionDistanceLabel.Content = distance.ToString() + " km";

            if (connection.Is_WiFi == true) ConnectionWiFiLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_Yes");
            else ConnectionWiFiLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_No");

            if (connection.Is_Bicycle_Carriage == true) ConnectionBicycleCarriageLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_Yes");
            else ConnectionBicycleCarriageLabel.SetResourceReference(ContentControl.ContentProperty, "ConnectionDetails_No");
        }
        public List<Intermediate> findIntermediateStations()
        {
            List<Intermediate> intermediate = new List<Intermediate>();

            Database.Stage firstStage = connection.stages.Find(stage => stage.Departure_Station == departureStation);
            Database.Stage lastStage = connection.stages.Find(stage => stage.Arrival_Station == arrivalStation);
            foreach (Database.Stage stage in connection.stages)
            {
                if (stage.Sequence >= firstStage.Sequence && stage.Sequence <= lastStage.Sequence)
                    intermediate.Add(new Intermediate(stage));
            }

            return intermediate;
        }

        public class Intermediate
        {
            public Intermediate(Database.Stage stage)
            {
                Departure_Station = stage.Departure_Station;
                Departure_Date = stage.Departure_Date + " " + stage.Departure_Time;
                Arrival_Station = stage.Arrival_Station;
                Arrival_Date = stage.Arrival_Date + " " + stage.Arrival_Time;
                Time = (DateTime.Parse(Arrival_Date) - DateTime.Parse(Departure_Date)).ToString(@"hh\:mm");
                Distance = stage.Distance + " km";
            }

            public string Departure_Station { get; set; }
            public string Departure_Date { get; set; }
            public string Arrival_Station { get; set; }
            public string Arrival_Date { get; set; }
            public string Time { get; set; }
            public string Distance { get; set; }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
