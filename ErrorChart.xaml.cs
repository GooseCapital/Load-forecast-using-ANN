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
using LiveCharts;
using LiveCharts.Wpf;

namespace Load_forecast_using_ANN
{
    /// <summary>
    /// Interaction logic for ErrorChart.xaml
    /// </summary>
    public partial class ErrorChart : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ErrorChart(List<double> yearList, List<double> errorList)
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Sai số",
                    Values = new ChartValues<double>(errorList)
                }
            };
            Labels = yearList.Select(n => n.ToString()).ToArray();
            Formatter = value => value.ToString("N");
            DataContext = this;
        }
    }
}
