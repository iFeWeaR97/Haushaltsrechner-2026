using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
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
        private int timeIndex = 0;

        public ObservableCollection<Expense> Expenses { get; set; } = new();

        private List<double> Ausgaben = new();

        public MainWindow()
        {
            InitializeComponent();
            BuildPlot();
            DataContext = this;
        }

        private void Button_choose_date(object sender, RoutedEventArgs e)
        {
            string userInput = userInputAusgaben.Text;

            if (double.TryParse(userInput, NumberStyles.Any, new CultureInfo("de-DE"), out double result))
            {
                // Wert speichern
                Ausgaben.Add(result);

                // Punkt an Plot anhängen
                series.Points.Add(new DataPoint(timeIndex, result));
                timeIndex++;

                PlotModel.InvalidatePlot(true);
            }
            else
            {
                MessageBox.Show("Bitte eine gültige Zahl eingeben.");
            }
        }

        private void Button_open_diagramm(object sender, RoutedEventArgs e)
        {
            Show_expenses.Visibility = Visibility.Collapsed;
           
            bool show = Show_Input.Visibility != Visibility.Visible;

            Show_Input.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            MyPlot.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BuildPlot()
        {

            PlotModel = new PlotModel
            {
                Title = "Übersicht der Ausgaben"
            };

            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Zeit"
            });

            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Ausgaben in €"
            });

            series = new LineSeries
            {
                Title = "Ausgabenhöhe",
                Color = OxyColors.Red,
                StrokeThickness = 2
            };

            PlotModel.Series.Add(series);
            MyPlot.Model = PlotModel;
        }


        private void Button_delete_diagramm(object sender, RoutedEventArgs e)
        {
            PlotModel.Series.Remove(series);
            PlotModel.InvalidatePlot(true);
            userInputAusgaben.Clear();
            BuildPlot();
        }




        private void Button_open_finance(object sender, RoutedEventArgs e)
        {

            Show_Input.Visibility = Visibility.Collapsed;
            MyPlot.Visibility = Visibility.Collapsed;

            bool show_finance = Show_expenses.Visibility != Visibility.Visible;

            Show_expenses.Visibility = show_finance ? Visibility.Visible : Visibility.Collapsed;

        }

        public class Expense
        {
            public string Name { get; set; }
            public string Betrag { get; set; }

        }


        private void Button_add_expenses(object sender, RoutedEventArgs e)
        {

            Expenses.Add(new Expense
            {
                Name = "Ausgaben",
                Betrag = "0€"

            });

        }


    }
}
