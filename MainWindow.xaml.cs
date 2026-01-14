
using System.Globalization;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WPF_Test
{
    public partial class MainWindow : Window
    {
        public PlotModel PlotModel { get; set; }
        private LineSeries series;

        double Ausgaben = 20.20;

        public MainWindow()
        {
            InitializeComponent();
            BuildPlot();
            DataContext = this;   // Binding aktivieren
        }

        private void Button_choose_date(object sender, RoutedEventArgs e)
        {
            string userInput = userInputAusgaben.Text;

            if (double.TryParse(userInput, NumberStyles.Any, new CultureInfo("de-DE"), out double result))
            {
                Ausgaben = result;

               
                if (series.Points.Count == 0)
                    series.Points.Add(new DataPoint(0, Ausgaben));
                else
                    series.Points[0] = new DataPoint(0, Ausgaben);

                PlotModel.InvalidatePlot(true);
            }
            else
            {
                MessageBox.Show("Bitte eine gültige Zahl eingeben.");
            }
        }


        private void Button_open_diagramm(object sender, RoutedEventArgs e)
        {
            bool show = Show_Input.Visibility != Visibility.Visible;

            Show_Input.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            MyPlot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;

        }

        private void BuildPlot()
        {
            DateTime dt = DateTime.Now;
            string outputDate = dt.ToString("dd.MM.yyyy");

            PlotModel = new PlotModel { Title = "Übersicht der Ausgaben" };

            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = outputDate });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Ausgaben in €" });

            
            series = new LineSeries { 
                Title = "Ausgabenhöhe",
                Color = OxyColors.Red,
                StrokeThickness = 2
            };

            // Startpunkt hinzufügen
            series.Points.Add(new DataPoint(0, 0));
            series.Points.Add(new DataPoint(Ausgaben, 0));
           
           

            PlotModel.Series.Add(series);
            MyPlot.Model = PlotModel;

        }



    }
}
