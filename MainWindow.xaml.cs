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

        //Funktionen für die einezlenen Buttons (Rechner)
        private void Value_0(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "0";
        }
        private void Value_1(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "1";
        }
        private void Value_2(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "2";
        }
        private void Value_3(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "3";
        }
        private void Value_4(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "4";
        }
        private void Value_5(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "5";
        }
        private void Value_6(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "6";
        }
        private void Value_7(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "7";
        }
        private void Value_8(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "8";
        }
        private void Value_9(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "9";
        }

        private void Value_Plus(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "+";
        }
        private void Value_Minus(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "-";
        }
        private void Value_multiplikation(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += "x";
        }
        private void Value_division(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content += ":";
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content = "";
        }

        private void Delete_All(object sender, RoutedEventArgs e)
        {
            Calc_Result.Content = "";
        }


        private void Calculator_Operation()
        {

            


        }


    }
}
