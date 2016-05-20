using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.CoreComponents;
using static System.Math;

namespace RNB_Analyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string Criteria { get; set; }
        public SeriesCollection PFSeries { get; set; }
        public SeriesCollection ACHSeries { get; set; }
        public SeriesCollection FCHSeries { get; set; }
        public SeriesCollection AFHSeries { get; set; }
        public Func<double, string> LogXFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public bool init { get; set; } = false;

        private readonly DispatcherTimer _timer;
        private double k = 1;
        private double T = 1;
        private double Psi = 1;
        public List<Zveno> typical;
        private LineSeries _PF = new LineSeries
        {
            Title = "ПФ",
            Values = new ChartValues<ViewModel>(),
            Fill = Brushes.Transparent,
            Stroke = Brushes.Blue,
            PointRadius = 0
        };

        private LineSeries _AFH = new LineSeries
        {
            Title = "АФХ",
            Values = new ChartValues<ViewModel>(),
            Fill = Brushes.Transparent,
            Stroke = Brushes.BlueViolet,
            PointRadius = 0
        };

        private LineSeries _ACH = new LineSeries
        {
            Title = "АЧХ",
            Values = new ChartValues<ViewModel>(),
            Fill = Brushes.Transparent,
            Stroke = Brushes.Red,
            PointRadius = 0
        };
        private LineSeries _FCH = new LineSeries
        {
            Title = "ФЧХ",
            Values = new ChartValues<ViewModel>(),
            Fill = Brushes.Transparent,
            Stroke = Brushes.BlueViolet,
            PointRadius = 0
        };

        private SeriesConfiguration<ViewModel> logConfig;
        private SeriesConfiguration<ViewModel> normalConfig;
        private bool loaded = false;

        public double upperValue { get; set; } = 10;
        public double lowerValue { get; set; } = 0;
        public double pointsCount { get; set; } = 20;

        public MainWindow()
        {
            log = false;
            XFormatter = x => x.ToString("g2");

            InitializeComponent();


            setupTypical();

            LogXFormatter = x => Pow(10, x).ToString();

            normalConfig = new SeriesConfiguration<ViewModel>()
               .X(point => point.XValue) // we use log10(point.X) as X
               .Y(point => point.YValue); // we use point.Y as the Y of our chart :)

            logConfig = new SeriesConfiguration<ViewModel>()
               .X(point => Math.Log(point.XValue, 10)) // we use log10(point.X) as X
               .Y(point => point.YValue); // we use point.Y as the Y of our chart :)

            PFSeries = new SeriesCollection
            {
                _PF
            }.Setup(normalConfig);

            ACHSeries = new SeriesCollection
            {
                _ACH
            }.Setup(normalConfig);

            FCHSeries = new SeriesCollection
            {
                _FCH
            }.Setup(normalConfig);

            AFHSeries = new SeriesCollection
            {
                _AFH
            }.Setup(normalConfig);

            _PF.Values.Clear();
            _ACH.Values.Clear();
            _FCH.Values.Clear();

            for (var i = lowerValue; i <= upperValue; i += (upperValue - lowerValue) / pointsCount)
            {
                _PF.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                _ACH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                _FCH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
            }

            DataContext = this;
            init = true;
        }

        private double one(double x) => (x <= 0) ? 0 : 1;
        private double sig(double x) => (x == 0) ? double.PositiveInfinity : 0;

        private void setupTypical()
        {
            typical = new List<Zveno>()
            {
                new Zveno() {
                    Name = "Пропорциональное",
                    PF = t => k*one(t),
                    IF = t => k*sig(t),
                    ACH = w => k,
                    FCH = w => 0
                },
                new Zveno()
                {
                    Name = "Дифференцирующее",
                    //Тут возможно не минус. Узнай что такое сигма с точкой
                    PF = t => k*(sig(t)),
                    IF = t => k*(-sig(t)),
                    ACH = w => k*w,
                    FCH = w => PI/2
                },
                new Zveno()
                {
                    Name = "Интегрирующее",
                    PF = t => k*t,
                    IF = t => k,
                    ACH = w => k/w,
                    FCH = w => -PI/2
                },
                new Zveno()
                {
                    Name = "Форсирующее 1 порядка",
                    PF = t => k*(sig(t)+one(t)),
                    IF = t => k*(-T*sig(t)+sig(t)),
                    ACH = w => k*Pow(Pow(T*w,2)+1,0.5),
                    FCH = w => Math.Atan(T*w)
                },

                new Zveno()
                {
                    Name = "Апериодическое",
                    PF = t => k*(1-Pow(E,-t/T)),
                    IF = t => k/T*(Pow(E,-t/T)),
                    ACH = w => k/Pow(Pow(T*w,2)+1,0.5),
                    FCH = w => -Atan(T*w)
                },
                new Zveno()
                {
                    Name = "Форсирующее 2 порядка",
                    PF = t => 0, //не используется
                    IF = t => 0, //не используется
                    ACH = w => k*Pow(Pow(1-Pow(T*w,2)+Pow(2*Psi*T*w,2),2),0.5),
                    FCH = w => ((w <= 1/T) ? PI : 0) + Atan((2*Psi*T*w)/(1 - Pow(T*w, 2)))
                },
                new Zveno()
                {
                    Name = "Колебательное",
                    PF = t =>
                    {
                        double al = Psi/T;
                        double beta = Pow(1 - Psi*Psi, 0.5)/T;
                        double phi0 = Atan(Pow(1 - Psi*Psi, 0.5)/Psi);
                        return k*( 1 - Pow(al*al+beta*beta,0.5)/beta * Pow(E,-al*t) * Sin(beta*t + phi0) );
                    },
                    IF = t =>
                    {
                        double al = Psi/T;
                        double beta = Pow(1 - Psi*Psi, 0.5)/T;
                        return k*(al*al+beta*beta)/beta * Pow(E,-al*t) * Sin(beta*t) ;
                    },
                    ACH = w => k/Pow( Pow(1-Pow(T*w,2),2) + Pow(2*Psi*T*w,2) ,0.5),
                    FCH = w => ((w <= 1/T)?(-PI):0) -Atan((2*Psi*T*w)/(1 - Pow(T*w, 2)))
                },

            };

            foreach (
                var item in
                from Zveno z in typical
                select new ListBoxItem { Content = z.Name }
                )
            {
                comboBox.Items.Add(item);
            }
        }

        private void updateValues(LineSeries series, Func<double, double> y)
        {
            foreach (ViewModel point in series.Values)
            {
                double val = y(point.XValue);
                if (double.IsPositiveInfinity(val))
                {


                    point.YValue = 1000;

                }
                else if (double.IsNegativeInfinity(val))
                {

                    point.YValue = -1000;

                }
                else if (double.IsNaN(val))
                {
                    point.YValue = 0;
                }
                else
                {
                    point.YValue = val;

                }
            }
        }



        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            updateValues();
        }

        private void updateValues()
        {
            if (comboBox.SelectedIndex == -1) return;
            Zveno cur = typical[comboBox.SelectedIndex];

            switch (bottomGraphComboBox.SelectedIndex)
            {
                case 0:
                    TransformChart.AxisY.Title = "h(t)";
                    _PF.Title = "ПФ";
                    _PF.Stroke = Brushes.GreenYellow;
                    updateValues(_PF, cur.PF);
                    break;
                case 1:
                    TransformChart.AxisY.Title = "w(t)";
                    _PF.Title = "ИФ";
                    _PF.Stroke = Brushes.Red;
                    updateValues(_PF, cur.IF);
                    break;
            }


            switch (rightGraphComboBox.SelectedIndex)
            {
                case 0:
                    updateValues(_ACH, cur.ACH);
                    updateValues(_FCH, cur.FCH);
                    break;

                case 1:
                    var LACH = new Func<double, double>(w => 20 * Log(cur.ACH(w), 10));
                    var LFCH = new Func<double, double>(w => 20 * Log(cur.FCH(w), 10));

                    updateValues(_ACH, LACH);
                    updateValues(_FCH, LFCH);
                    break;


                case 2:
                    _AFH.Values.Clear();
                    
                    for (var w = .0; w < 2.0; w+=0.1)
                    {
                        double ach = cur.ACH(w)* Sin(cur.FCH(w));
                        double fch = cur.ACH(w) * Cos(cur.FCH(w));
                        if (!double.IsNaN(ach) && !double.IsInfinity(ach) && !double.IsNegativeInfinity(ach) &&
                            !double.IsPositiveInfinity(ach)
                            && !double.IsNaN(fch) && !double.IsInfinity(fch) && !double.IsNegativeInfinity(fch) &&
                            !double.IsPositiveInfinity(fch))
                        {
                            _AFH.Values.Add(new ViewModel()
                            {
                                XValue = ach,
                                YValue = fch
                            });
                        }
                        else
                        {
                        }
                    }

                    break;

            }


        }

        public bool log { get; set; }

        private void kSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            k = kSlider.Value;
            kText.Text = k.ToString();
            updateValues();
        }

        private void tSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            T = tSlider.Value;
            tText.Text = T.ToString();
            updateValues();

        }

        private void slider_Copy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Psi = psiSlider.Value;
            psiText.Text = Psi.ToString();
            updateValues();
        }


        private void kText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            try
            {
                k = double.Parse(kText.Text);
                kSlider.Value = k;
                updateValues();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void psiText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            try
            {
                Psi = double.Parse(psiText.Text);
                psiSlider.Value = Psi;
                updateValues();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void tText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            try
            {
                T = double.Parse(tText.Text);
                tSlider.Value = T;
                updateValues();
            }
            catch (Exception)
            {
                // ignored
            }
        }



        private void pointsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!loaded) return;
            this.pointsCount = Math.Round(pointsSlider.Value);
            if (pointsText != null) pointsText.Text = Math.Round(pointsSlider.Value).ToString();
            rebuildValues();

        }

        private void rebuildValues()
        {
            if (!init) return;


            switch (bottomGraphComboBox.SelectedIndex)
            {
                case 0:
                    TransformChart.AxisY.Title = "h(t)";
                    break;

                case 1:
                    TransformChart.AxisY.Title = "w(t)";
                    break;
            }

            PchChart.AxisX.Separator.Step = (upperValue - lowerValue) / pointsCount;


            switch (rightGraphComboBox.SelectedIndex)
            {
                case 0:
                    AchChart.Visibility = Visibility.Visible;
                    PchChart.Visibility = Visibility.Visible;
                    AFH_chart.Visibility = Visibility.Hidden;

                    _ACH.Values.Clear();
                    _FCH.Values.Clear();

                    ACHSeries.Setup(normalConfig);
                    FCHSeries.Setup(normalConfig);

                    PchChart.AxisX.LabelFormatter = XFormatter;
                    PchChart.AxisY.Title = "Ф(w), рад.";
                    _FCH.Title = "ФЧХ";
                    AchChart.AxisX.LabelFormatter = XFormatter;
                    AchChart.AxisY.Title = "A(w)";
                    _ACH.Title = "АЧХ";
                    
                    PchChart.AxisX.Separator.Step = (upperValue - lowerValue) / pointsCount;
                    
                    PchChart.AxisX.Separator.Step = (upperValue - lowerValue) / pointsCount;

                    for (var i = lowerValue; i < upperValue; i += (upperValue - lowerValue) / pointsCount)
                    {
                        _ACH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                        _FCH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                    }

                    break;

                case 1:
                    AchChart.Visibility = Visibility.Visible;
                    PchChart.Visibility = Visibility.Visible;
                    AFH_chart.Visibility = Visibility.Hidden;

                    _ACH.Values.Clear();
                    _FCH.Values.Clear();

                    ACHSeries.Setup(logConfig);
                    FCHSeries.Setup(logConfig);

                    PchChart.AxisX.LabelFormatter = LogXFormatter;
                    _FCH.Title = "ЛФЧХ";

                    PchChart.AxisY.Title = "Ф(w), дБ";
                    AchChart.AxisX.LabelFormatter = LogXFormatter;
                    AchChart.AxisY.Title = "L(w), дБ";
                    _ACH.Title = "ЛАЧХ";


                    for (var i = 1e-5; i < 1e3; i *= 10)
                    {
                        _ACH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                        _FCH.Values.Add(new ViewModel { XValue = i, YValue = 0 });
                    }

                    break;

                case 2:
                    AchChart.Visibility = Visibility.Hidden;
                    PchChart.Visibility = Visibility.Hidden;
                    AFH_chart.Visibility = Visibility.Visible;

                    break;

            }

            updateValues();
        }


        private void pointsText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            try
            {
                pointsCount = double.Parse(pointsText.Text);
                pointsSlider.Value = pointsCount;
            }
            catch (Exception exception)
            {
                // ignored
            }

            rebuildValues();
        }

        private void leftBorder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            try
            {
                lowerValue = double.Parse(leftBorder.Text);
            }
            catch (Exception exception)
            {
                // ignored
            }

            rebuildValues();
        }

        private void rightBorder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            try
            {
                upperValue = double.Parse(rightBorder.Text);

            }
            catch (Exception exception)
            {
                // ignored
            }

            rebuildValues();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rebuildValues();
        }

        private void comboBox1_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rebuildValues();

        }
    }

}
