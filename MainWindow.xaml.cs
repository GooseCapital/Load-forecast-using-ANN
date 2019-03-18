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

        private List<string> _danSo = null;
        private List<string> _kinhTe = null;
        private List<string> _nhietDo = null;
        private List<string> _numNoron = null;
        private List<string> _phuTai = null;
        private List<string> _years = null;
        private List<DataImport> _dataImports = new List<DataImport>();

        private void BtnInfoAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Model.Constants.InfoButton, "Thông tin", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainWindows_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFileMain();
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

            DataGridTableValue.ItemsSource = _dataImports;
        }

        private void LoadFileMain()
        {
            LoadFile("danso.txt", ref _danSo);
            LoadFile("kinhTe.txt", ref _kinhTe);
            LoadFile("nhietDo.txt", ref _nhietDo);
            LoadFile("numNoron.txt", ref _numNoron);
            LoadFile("phuTai.txt", ref _phuTai);
            LoadFile("years.txt", ref _years);
        }

        private bool LoadFile(string fileName, ref List<string> objectSet)
        {
            if (File.Exists(StaticFunctions.PathDataCombine(fileName)))
            {
                string temp = File.ReadAllText(StaticFunctions.PathDataCombine(fileName));
                objectSet = temp.Split(' ').ToList();
                return true;
            }

            return false;
        }

        private bool CheckDataBeforeExport()
        {
            for (int i = 0; i < _dataImports.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(_dataImports[i].Population) || String.IsNullOrWhiteSpace(_dataImports[i].Economy) ||
                    String.IsNullOrWhiteSpace(_dataImports[i].Load) || String.IsNullOrWhiteSpace(_dataImports[i].Temperature) ||
                    String.IsNullOrWhiteSpace(_dataImports[i].Years))
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
                for (int i = 0; i < _dataImports.Count; i++)
                {
                    _danSo.Add(_dataImports[i].Population);
                    _kinhTe.Add(_dataImports[i].Economy);
                    _years.Add(_dataImports[i].Years);
                    _phuTai.Add(_dataImports[i].Load);
                    _nhietDo.Add(_dataImports[i].Temperature);
                }

                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("danso.txt"), _danSo);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("kinhTe.txt"), _kinhTe);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("nhietDo.txt"), _nhietDo);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("phuTai.txt"), _phuTai);
                StaticFunctions.ReplaceDataFromList(StaticFunctions.PathDataCombine("years.txt"), _years);

                return true;
            }
        }



        private void DeleteAllDataList()
        {
            _danSo = new List<string>();
            _kinhTe = new List<string>();
            _nhietDo = new List<string>();
            _phuTai = new List<string>();
            _years = new List<string>();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            
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
