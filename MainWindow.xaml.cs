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
        public ObservableCollection<Income> Incomes { get; set; } = new();

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
  
            Show_calc.Visibility = Visibility.Collapsed;

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
            Show_calc.Visibility = Visibility.Collapsed;

            bool show_expenses = Show_expenses.Visibility != Visibility.Visible; 
            Show_expenses.Visibility = show_expenses ? Visibility.Visible : Visibility.Collapsed;

            bool show_incomes = Show_incomes.Visibility != Visibility.Visible;
            Show_incomes.Visibility = show_incomes ? Visibility.Visible : Visibility.Collapsed;

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

        public class Income
        {
            public string Name_income { get; set; }
            public string Betrag_income { get; set; }

        }


        private void Button_add_incomes(object sender, RoutedEventArgs e)
        {

            Incomes.Add(new Income
            {
                Name_income = "Einnahmen",
                Betrag_income = "0€"

            });

        }

        private void Button_open_calc (object sender, RoutedEventArgs e)
        {

            Show_Input.Visibility = Visibility.Collapsed;
            MyPlot.Visibility = Visibility.Collapsed;
            Show_expenses.Visibility = Visibility.Collapsed;
            Show_incomes.Visibility = Visibility.Collapsed;

            bool show_calc = Show_calc.Visibility != Visibility.Visible;
            Show_calc.Visibility = show_calc ? Visibility.Visible : Visibility.Collapsed;

        }


    }
}
