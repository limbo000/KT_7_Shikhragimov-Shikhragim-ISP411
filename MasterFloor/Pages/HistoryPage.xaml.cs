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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterFloor.Pages
{
    /// <summary>
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private readonly Data.PartnersImport _selectedPartner;

        public HistoryPage(Data.PartnersImport selectedPartner)
        {
            InitializeComponent();
            _selectedPartner = selectedPartner;

            LoadHistory();
        }

        private void LoadHistory()
        {
            var history = Data.MasterPolEntities.GetContext().PartnerProductsImport
                .Where(p => p.IdPartnerName == _selectedPartner.Id)
                .Select(p => new
                {
                    Production = p.Production,
                    CountOfProduction = p.CountOfProduction,
                    DateOfSale = p.DateOfSale
                })
                .ToList();

            HistoryDataGrid.ItemsSource = history;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.ListViewPage());
        }
    }
}
