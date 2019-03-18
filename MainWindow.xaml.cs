using System;
using System.Collections.Generic;
using System.IO;
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
using ControlzEx.Native;
using Forecast_Load_ANN;
using Load_forecast_using_ANN.Model;
using MahApps.Metro.Controls;


namespace Load_forecast_using_ANN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<double> _danSo = new List<double>();
        private List<double> _kinhTe = new List<double>();
        private List<double> _nhietDo = new List<double>();
        private List<double> _numNoron = new List<double>();
        private List<double> _phuTai = new List<double>();
        private List<double> _years = new List<double>();

        private List<double> _danSoDubao = new List<double>();
        private List<double> _kinhTeDubao = new List<double>();
        private List<double> _nhietDoDubao = new List<double>();
        private List<double> _yearsDubao = new List<double>();

        private List<DataImport> _dataImports = new List<DataImport>();
        private List<ForecastData> _forecastDatas = new List<ForecastData>();

        private void BtnInfoAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Model.Constants.InfoButton, "Thông tin", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainWindows_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFileMain();
            if (_numNoron.Count > 0)
            {
                TbNumberNoron.Text = _numNoron[0].ToString();
            }
            ConvertListDataToClassObject();
        }

        private void ConvertListDataToClassObject()
        {
            for (int i = 0; i < _years.Count; i++)
            {
                _dataImports.Add(new DataImport()
                {
                    Economy = _kinhTe[i],
                    Load = _phuTai[i],
                    Population = _danSo[i],
                    Temperature = _nhietDo[i],
                    Years = _years[i]
                });
            }

            for (int i = 0; i < _yearsDubao.Count; i++)
            {
                _forecastDatas.Add(new ForecastData()
                {
                    Economy = _kinhTeDubao[i],
                    Population = _danSoDubao[i],
                    Temperature = _nhietDoDubao[i],
                    Years = _yearsDubao[i]
                });
            }

            DataGridTableValue.ItemsSource = _dataImports;
            DataGridForecast.ItemsSource = _forecastDatas;
        }

        private void LoadFileMain()
        {
            LoadFile("danso.txt", ref _danSo);
            LoadFile("kinhTe.txt", ref _kinhTe);
            LoadFile("nhietDo.txt", ref _nhietDo);
            LoadFile("numNoron.txt", ref _numNoron);
            LoadFile("phuTai.txt", ref _phuTai);
            LoadFile("years.txt", ref _years);

            LoadFile("danso_dubao.txt", ref _danSoDubao);
            LoadFile("kinhTe_dubao.txt", ref _kinhTeDubao);
            LoadFile("nhietDo_dubao.txt", ref _nhietDoDubao);
            LoadFile("years_dubao.txt", ref _yearsDubao);
        }

        private bool LoadFile(string fileName, ref List<double> objectSet)
        {
            if (File.Exists(StaticFunctions.PathDataCombine(fileName)))
            {
                string temp = File.ReadAllText(StaticFunctions.PathDataCombine(fileName));
                for (int i = 0; i < temp.Split(' ').Length; i++)
                {
                    objectSet.Add(Convert.ToDouble(temp.Split(' ')[i]));
                }
                return true;
            }

            return false;
        }

        private bool CheckDataBeforeExport()
        {
            for (int i = 0; i < _dataImports.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(_dataImports[i].Population.ToString()) || String.IsNullOrWhiteSpace(_dataImports[i].Economy.ToString()) ||
                    String.IsNullOrWhiteSpace(_dataImports[i].Load.ToString()) || String.IsNullOrWhiteSpace(_dataImports[i].Temperature.ToString()) ||
                    String.IsNullOrWhiteSpace(_dataImports[i].Years.ToString()))
                {
                    return false;
                }
            }

            for (int i = 0; i < _forecastDatas.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(_forecastDatas[i].Population.ToString()) ||
                    String.IsNullOrWhiteSpace(_forecastDatas[i].Economy.ToString()) ||
                    String.IsNullOrWhiteSpace(_forecastDatas[i].Temperature.ToString()) ||
                    String.IsNullOrWhiteSpace(_forecastDatas[i].Years.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SaveData()
        {
            if (!CheckDataBeforeExport())
            {
                return false;
            }
            else
            {
                DeleteAllDataList();
                _numNoron.Add(Convert.ToDouble(TbNumberNoron.Text));
                for (int i = 0; i < _dataImports.Count; i++)
                {
                    _danSo.Add(_dataImports[i].Population);
                    _kinhTe.Add(_dataImports[i].Economy);
                    _years.Add(_dataImports[i].Years);
                    _phuTai.Add(_dataImports[i].Load);
                    _nhietDo.Add(_dataImports[i].Temperature);
                }

                for (int i = 0; i < _forecastDatas.Count; i++)
                {
                    _danSoDubao.Add(_forecastDatas[i].Population);
                    _kinhTeDubao.Add(_forecastDatas[i].Economy);
                    _yearsDubao.Add(_forecastDatas[i].Years);
                    _nhietDoDubao.Add(_forecastDatas[i].Temperature);
                }

                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("danso.txt"), _danSo);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("kinhTe.txt"), _kinhTe);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("nhietDo.txt"), _nhietDo);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("phuTai.txt"), _phuTai);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("years.txt"), _years);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("numNoron.txt"), _numNoron);

                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("danso_dubao.txt"), _danSoDubao);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("kinhTe_dubao.txt"), _kinhTeDubao);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("nhietDo_dubao.txt"), _nhietDoDubao);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("years_dubao.txt"), _yearsDubao);

                return true;
            }
        }

        private void DeleteAllDataList()
        {
            _danSo = new List<double>();
            _kinhTe = new List<double>();
            _nhietDo = new List<double>();
            _phuTai = new List<double>();
            _years = new List<double>();
            _numNoron = new List<double>();

            _nhietDoDubao = new List<double>();
            _danSoDubao = new List<double>();
           _kinhTeDubao = new List<double>();
           _yearsDubao = new List<double>();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ANN_Class annClass = new ANN_Class();
            annClass.Forecast_Load_ANN();
            annClass.WaitForFiguresToDie();
        }

        private void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            if (SaveData())
            {
                MessageBox.Show("Lưu dữ liệu thành công", "Thông Báo", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Dữ liệu điền vào chưa đủ", "Lỗi", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
