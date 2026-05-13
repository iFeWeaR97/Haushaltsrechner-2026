using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WPF_Test
{
    public class Expense : INotifyPropertyChanged
    {
        private string _name = "Neue Ausgabe";
        private string _betrag = "0";

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Betrag
        {
            get => _betrag;
            set
            {
                _betrag = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BetragValue));
            }
        }

        public double BetragValue =>
            double.TryParse(
                Betrag.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double v)
                ? v
                : 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class Income : INotifyPropertyChanged
    {
        private string _name = "Neue Einnahme";
        private string _betrag = "0";

        public string Name_income
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Betrag_income
        {
            get => _betrag;
            set
            {
                _betrag = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BetragValue));
            }
        }

        public double BetragValue =>
            double.TryParse(
                Betrag_income.Replace(',', '.'),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double v)
                ? v
                : 0;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<Expense> Expenses { get; set; } = new();
        public ObservableCollection<Income> Incomes { get; set; } = new();

        public PlotModel PlotModel { get; set; }
        public PlotModel OverviewModel { get; set; }

        private string _calcInput = "";
        private double _calcFirstNum = 0;
        private string _calcOperator = "";
        private bool _calcNewInput = true;

        public MainWindow()
        {
            InitializeComponent();

            BuildPlots();

            DataContext = this;

            Recalculate();
        }

        private void HideAllPages()
        {
            Page_Overview.Visibility = Visibility.Collapsed;
            Page_Expenses.Visibility = Visibility.Collapsed;
            Page_Incomes.Visibility = Visibility.Collapsed;
            Page_Chart.Visibility = Visibility.Collapsed;
            Page_Calc.Visibility = Visibility.Collapsed;
        }

        private void Nav_Overview(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            Page_Overview.Visibility = Visibility.Visible;
            Recalculate();
        }

        private void Nav_Expenses(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            Page_Expenses.Visibility = Visibility.Visible;
        }

        private void Nav_Incomes(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            Page_Incomes.Visibility = Visibility.Visible;
        }

        private void Nav_Chart(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            Page_Chart.Visibility = Visibility.Visible;
            RefreshMainChart();
        }

        private void Nav_Calc(object sender, RoutedEventArgs e)
        {
            HideAllPages();
            Page_Calc.Visibility = Visibility.Visible;
        }

        private void Recalculate(object sender = null, RoutedEventArgs e = null)
        {
            double totalIncome = Incomes.Sum(i => i.BetragValue);
            double totalExpense = Expenses.Sum(ex => ex.BetragValue);
            double saldo = totalIncome - totalExpense;

            var germanCulture = new CultureInfo("de-DE");

            LabelTotalIncome.Text = totalIncome.ToString("N2", germanCulture) + " €";
            LabelTotalExpense.Text = totalExpense.ToString("N2", germanCulture) + " €";
            LabelSaldo.Text = saldo.ToString("N2", germanCulture) + " €";

            LabelSaldo.Foreground = saldo >= 0
                ? new SolidColorBrush(Color.FromRgb(99, 179, 237))
                : new SolidColorBrush(Color.FromRgb(252, 129, 129));

            RefreshOverviewChart(totalIncome, totalExpense);
        }

        private void Button_add_expenses(object sender, RoutedEventArgs e)
        {
            Expenses.Add(new Expense());
            Recalculate();
        }

        private void Button_DeleteExpense(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Expense exp)
            {
                Expenses.Remove(exp);
                Recalculate();
            }
        }

        private void Button_add_incomes(object sender, RoutedEventArgs e)
        {
            Incomes.Add(new Income());
            Recalculate();
        }

        private void Button_DeleteIncome(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Income inc)
            {
                Incomes.Remove(inc);
                Recalculate();
            }
        }

        private void BuildPlots()
        {
            OverviewModel = new PlotModel
            {
                Background = OxyColor.FromArgb(0, 0, 0, 0),
                PlotAreaBackground = OxyColor.FromArgb(0, 0, 0, 0),
                TextColor = OxyColors.LightGray
            };

            OverviewModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "overviewCategories",
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColor.FromRgb(45, 55, 72)
            });

            OverviewModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                AbsoluteMinimum = 0,
                TextColor = OxyColors.LightGray,
                AxislineColor = OxyColor.FromRgb(45, 55, 72),
                TicklineColor = OxyColor.FromRgb(45, 55, 72),
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(45, 55, 72)
            });

            PlotModel = new PlotModel
            {
                Title = "Ausgaben & Einnahmen im Überblick",
                TitleColor = OxyColors.LightGray,
                Background = OxyColor.FromArgb(0, 0, 0, 0),
                PlotAreaBackground = OxyColor.FromArgb(0, 0, 0, 0),
                TextColor = OxyColors.LightGray
            };

            PlotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "mainCategories",
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColor.FromRgb(45, 55, 72)
            });

            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Betrag in €",
                MinimumPadding = 0,
                AbsoluteMinimum = 0,
                TextColor = OxyColors.LightGray,
                TitleColor = OxyColors.LightGray,
                AxislineColor = OxyColor.FromRgb(45, 55, 72),
                TicklineColor = OxyColor.FromRgb(45, 55, 72),
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromRgb(45, 55, 72)
            });
        }

        private void RefreshOverviewChart(double income, double expense)
        {
            if (OverviewModel == null)
                return;

            OverviewModel.Series.Clear();

            var catAxis = OverviewModel.Axes
                .OfType<CategoryAxis>()
                .FirstOrDefault();

            if (catAxis != null)
            {
                catAxis.Labels.Clear();
                catAxis.Labels.Add("Einnahmen");
                catAxis.Labels.Add("Ausgaben");
                catAxis.Labels.Add("Saldo");
            }

            double saldo = income - expense;

            var series = new BarSeries
            {
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:N2} €",
                TextColor = OxyColors.LightGray,
                StrokeThickness = 1
            };

            series.Items.Add(new BarItem(income)
            {
                Color = OxyColor.FromRgb(56, 161, 105)
            });

            series.Items.Add(new BarItem(expense)
            {
                Color = OxyColor.FromRgb(229, 62, 62)
            });

            series.Items.Add(new BarItem(Math.Abs(saldo))
            {
                Color = saldo >= 0
                    ? OxyColor.FromRgb(99, 179, 237)
                    : OxyColor.FromRgb(252, 129, 129)
            });

            OverviewModel.Series.Add(series);
            OverviewModel.InvalidatePlot(true);
        }

        private void RefreshMainChart()
        {
            if (PlotModel == null)
                return;

            PlotModel.Series.Clear();

            var catAxis = PlotModel.Axes
                .OfType<CategoryAxis>()
                .FirstOrDefault();

            if (catAxis != null)
                catAxis.Labels.Clear();

            var series = new BarSeries
            {
                Title = "Einnahmen / Ausgaben",
                LabelPlacement = LabelPlacement.Outside,
                LabelFormatString = "{0:N2} €",
                TextColor = OxyColors.LightGray,
                StrokeThickness = 1
            };

            foreach (var inc in Incomes)
            {
                series.Items.Add(new BarItem(inc.BetragValue)
                {
                    Color = OxyColor.FromRgb(56, 161, 105)
                });

                catAxis?.Labels.Add("Einnahme: " + inc.Name_income);
            }

            foreach (var ex in Expenses)
            {
                series.Items.Add(new BarItem(ex.BetragValue)
                {
                    Color = OxyColor.FromRgb(229, 62, 62)
                });

                catAxis?.Labels.Add("Ausgabe: " + ex.Name);
            }

            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);
        }

        private void Button_RefreshChart(object sender, RoutedEventArgs e)
        {
            Recalculate();
            RefreshMainChart();
        }

        private void Button_delete_diagramm(object sender, RoutedEventArgs e)
        {
            if (PlotModel == null)
                return;

            PlotModel.Series.Clear();

            var catAxis = PlotModel.Axes
                .OfType<CategoryAxis>()
                .FirstOrDefault();

            catAxis?.Labels.Clear();

            PlotModel.InvalidatePlot(true);
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            string val = btn.Content.ToString();

            switch (val)
            {
                case "AC":
                    _calcInput = "";
                    _calcFirstNum = 0;
                    _calcOperator = "";
                    _calcNewInput = true;
                    CalcDisplay.Text = "0";
                    CalcSubDisplay.Text = "";
                    break;

                case "DEL":
                    if (_calcInput.Length > 0)
                        _calcInput = _calcInput[..^1];

                    CalcDisplay.Text = string.IsNullOrEmpty(_calcInput)
                        ? "0"
                        : _calcInput.Replace('.', ',');
                    break;

                case "±":
                    if (double.TryParse(
                            _calcInput.Replace(',', '.'),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out double negVal))
                    {
                        _calcInput = (-negVal).ToString(CultureInfo.InvariantCulture);
                        CalcDisplay.Text = _calcInput.Replace('.', ',');
                    }
                    break;

                case "%":
                    if (double.TryParse(
                            _calcInput.Replace(',', '.'),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out double pctVal))
                    {
                        _calcInput = (pctVal / 100).ToString(CultureInfo.InvariantCulture);
                        CalcDisplay.Text = _calcInput.Replace('.', ',');
                    }
                    break;

                case "+":
                case "-":
                case "×":
                case "÷":
                    if (_calcInput != "")
                    {
                        double.TryParse(
                            _calcInput.Replace(',', '.'),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out _calcFirstNum);

                        CalcSubDisplay.Text = _calcInput.Replace('.', ',') + " " + val;
                    }

                    _calcOperator = val;
                    _calcNewInput = true;
                    _calcInput = "";
                    break;

                case "=":
                    if (_calcOperator != "" && _calcInput != "")
                    {
                        double.TryParse(
                            _calcInput.Replace(',', '.'),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out double secondNum);

                        double result = _calcOperator switch
                        {
                            "+" => _calcFirstNum + secondNum,
                            "-" => _calcFirstNum - secondNum,
                            "×" => _calcFirstNum * secondNum,
                            "÷" => secondNum != 0 ? _calcFirstNum / secondNum : double.NaN,
                            _ => secondNum
                        };

                        CalcSubDisplay.Text =
                            CalcSubDisplay.Text + " " + _calcInput.Replace('.', ',') + " =";

                        if (double.IsNaN(result))
                        {
                            _calcInput = "";
                            CalcDisplay.Text = "Fehler";
                        }
                        else
                        {
                            _calcInput = result.ToString(CultureInfo.InvariantCulture);
                            CalcDisplay.Text = result.ToString("N2", new CultureInfo("de-DE"));
                        }

                        _calcOperator = "";
                        _calcNewInput = true;
                    }
                    break;

                case ",":
                    if (!_calcInput.Contains('.') && !_calcInput.Contains(','))
                    {
                        if (_calcInput == "")
                            _calcInput = "0";

                        _calcInput += ",";
                        CalcDisplay.Text = _calcInput;
                    }
                    break;

                default:
                    if (_calcNewInput)
                    {
                        _calcInput = "";
                        _calcNewInput = false;
                    }

                    _calcInput += val;
                    CalcDisplay.Text = _calcInput.Replace('.', ',');
                    break;
            }
        }

        private void Button_SaveCSV(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "CSV-Datei|*.csv",
                FileName = "Haushalt_" + DateTime.Now.ToString("yyyy-MM-dd")
            };

            if (dlg.ShowDialog() != true)
                return;

            var sb = new StringBuilder();

            sb.AppendLine("Typ;Bezeichnung;Betrag");

            foreach (var inc in Incomes)
                sb.AppendLine($"Einnahme;{inc.Name_income};{inc.BetragValue:F2}");

            foreach (var ex in Expenses)
                sb.AppendLine($"Ausgabe;{ex.Name};{ex.BetragValue:F2}");

            double saldo =
                Incomes.Sum(i => i.BetragValue) -
                Expenses.Sum(ex => ex.BetragValue);

            sb.AppendLine(";;");
            sb.AppendLine($"Saldo;;{saldo:F2}");

            File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);

            MessageBox.Show(
                $"CSV gespeichert:\n{dlg.FileName}",
                "Gespeichert",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void Button_LoadCSV(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "CSV-Datei|*.csv|Alle Dateien|*.*",
                Title = "CSV-Datei laden"
            };

            if (dlg.ShowDialog() != true)
                return;

            try
            {
                string[] lines = File.ReadAllLines(dlg.FileName, Encoding.UTF8);

                Incomes.Clear();
                Expenses.Clear();

                foreach (string line in lines.Skip(1)) // Überschrift überspringen
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(';');

                    if (parts.Length < 3)
                        continue;

                    string typ = parts[0].Trim();
                    string bezeichnung = parts[1].Trim();
                    string betragText = parts[2].Trim();

                    if (typ == "Saldo")
                        continue;

                    if (string.IsNullOrWhiteSpace(typ))
                        continue;

                    if (!double.TryParse(
                            betragText.Replace(',', '.'),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out double betrag))
                    {
                        continue;
                    }

                    string betragAlsText = betrag.ToString(CultureInfo.InvariantCulture);

                    if (typ == "Einnahme")
                    {
                        Incomes.Add(new Income
                        {
                            Name_income = bezeichnung,
                            Betrag_income = betragAlsText
                        });
                    }
                    else if (typ == "Ausgabe")
                    {
                        Expenses.Add(new Expense
                        {
                            Name = bezeichnung,
                            Betrag = betragAlsText
                        });
                    }
                }

                Recalculate();
                RefreshMainChart();

                MessageBox.Show(
                    $"CSV geladen:\n{dlg.FileName}",
                    "Geladen",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fehler beim Laden der CSV-Datei:\n{ex.Message}",
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Button_Print(object sender, RoutedEventArgs e)
        {
            var dlg = new PrintDialog();

            if (dlg.ShowDialog() != true)
                return;

            var doc = new FlowDocument
            {
                FontFamily = new FontFamily("Cascadia Code"),
                FontSize = 12,
                PagePadding = new Thickness(40)
            };

            doc.Blocks.Add(new Paragraph(new Run(
                "Haushaltsplan — " + DateTime.Now.ToString("dd.MM.yyyy")))
            {
                FontSize = 18,
                FontWeight = System.Windows.FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 16)
            });

            doc.Blocks.Add(new Paragraph(new Run("EINNAHMEN"))
            {
                FontSize = 13,
                FontWeight = System.Windows.FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 6)
            });

            var incTable = new Table();
            incTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) });
            incTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var incGroup = new TableRowGroup();

            foreach (var inc in Incomes)
            {
                var row = new TableRow();

                row.Cells.Add(new TableCell(new Paragraph(new Run(inc.Name_income))));

                row.Cells.Add(new TableCell(new Paragraph(
                    new Run(inc.BetragValue.ToString("N2", new CultureInfo("de-DE")) + " €"))
                {
                    TextAlignment = TextAlignment.Right
                }));

                incGroup.Rows.Add(row);
            }

            incTable.RowGroups.Add(incGroup);
            doc.Blocks.Add(incTable);

            doc.Blocks.Add(new Paragraph(new Run("AUSGABEN"))
            {
                FontSize = 13,
                FontWeight = System.Windows.FontWeights.Bold,
                Margin = new Thickness(0, 16, 0, 6)
            });

            var expTable = new Table();
            expTable.Columns.Add(new TableColumn { Width = new GridLength(3, GridUnitType.Star) });
            expTable.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var expGroup = new TableRowGroup();

            foreach (var ex in Expenses)
            {
                var row = new TableRow();

                row.Cells.Add(new TableCell(new Paragraph(new Run(ex.Name))));

                row.Cells.Add(new TableCell(new Paragraph(
                    new Run(ex.BetragValue.ToString("N2", new CultureInfo("de-DE")) + " €"))
                {
                    TextAlignment = TextAlignment.Right
                }));

                expGroup.Rows.Add(row);
            }

            expTable.RowGroups.Add(expGroup);
            doc.Blocks.Add(expTable);

            double totalIncome = Incomes.Sum(i => i.BetragValue);
            double totalExpense = Expenses.Sum(ex => ex.BetragValue);
            double saldo = totalIncome - totalExpense;

            doc.Blocks.Add(new Paragraph(new Run(
                $"\nEinnahmen: {totalIncome.ToString("N2", new CultureInfo("de-DE"))} €   |   " +
                $"Ausgaben: {totalExpense.ToString("N2", new CultureInfo("de-DE"))} €   |   " +
                $"Saldo: {saldo.ToString("N2", new CultureInfo("de-DE"))} €"))
            {
                FontWeight = System.Windows.FontWeights.Bold,
                Margin = new Thickness(0, 16, 0, 0)
            });

            var docPaginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;
            docPaginator.PageSize = new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

            dlg.PrintDocument(docPaginator, "Haushaltsplan");
        }
    }
}