using MasterFloor.Data;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private PartnersImport _currentpartner;
        private bool FlagAddorEdit;

        public AddEditPage(PartnersImport partner)
        {
            InitializeComponent();
            _currentpartner = partner;
            FlagAddorEdit = _currentpartner == null;
            Init();
        }

        public void Init()
        {
            IdTextBox.Visibility = Visibility.Hidden;
            IdLabel.Visibility = Visibility.Hidden;
            TypeComboBox.ItemsSource = Data.MasterPolEntities.GetContext().TypeOfPartner.ToList();
            if (_currentpartner != null)
            {
                NameTextBox.Text = _currentpartner.PartnerName.Name;
                RatingTextBox.Text = _currentpartner.Reiting.ToString();
                AdressTextBox.Text = _currentpartner.IdAdress.ToString();
                FIOTextBox.Text = _currentpartner.Directors.FIO;
                PhoneTextBox.Text = _currentpartner.PhoneOfPartner;
                EmailTextBox.Text = _currentpartner.EmailOfPartner;
                IdTextBox.Text = _currentpartner.Id.ToString();
                TypeComboBox.SelectedItem = Data.MasterPolEntities.GetContext().TypeOfPartner
                    .FirstOrDefault(d => d.Id == _currentpartner.IdTypeOfParther);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.ListViewPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    errors.AppendLine("Заполните наименование!");
                }
                if (TypeComboBox.SelectedItem == null)
                {
                    errors.AppendLine("Выберите тип партнера!");
                }
                if (string.IsNullOrEmpty(RatingTextBox.Text))
                {
                    errors.AppendLine("Заполните рейтинг!");
                }
                if (string.IsNullOrEmpty(AdressTextBox.Text))
                {
                    errors.AppendLine("Заполните адрес!");
                }
                if (string.IsNullOrEmpty(FIOTextBox.Text))
                {
                    errors.AppendLine("Заполните ФИО!");
                }
                if (string.IsNullOrEmpty(PhoneTextBox.Text))
                {
                    errors.AppendLine("Заполните номер телефона!");
                }
                if (string.IsNullOrEmpty(EmailTextBox.Text))
                {
                    errors.AppendLine("Заполните Email!");
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var selectedCategory = TypeComboBox.SelectedItem as Data.TypeOfPartner;
                if (selectedCategory == null)
                {
                    MessageBox.Show("Выберите тип партнера!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!FlagAddorEdit)
                {
                    _currentpartner.IdTypeOfParther = selectedCategory.Id;
                    _currentpartner.Reiting = Convert.ToInt32(RatingTextBox.Text);
                    _currentpartner.PhoneOfPartner = PhoneTextBox.Text;
                    _currentpartner.EmailOfPartner = EmailTextBox.Text;

                    var searchDirector = (from item in Data.MasterPolEntities.GetContext().Directors
                                          where item.Id == _currentpartner.IdDirector
                                          select item).FirstOrDefault();
                    if (searchDirector != null)
                    {
                        searchDirector.FIO = FIOTextBox.Text;
                    }

                    var searchPartnerName = (from item in Data.MasterPolEntities.GetContext().PartnerName
                                             where item.Id == _currentpartner.IdPartnerName
                                             select item).FirstOrDefault();
                    if (searchPartnerName != null)
                    {
                        searchPartnerName.Name = NameTextBox.Text;
                    }

                    MessageBox.Show("Успешно изменено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var newPartner = new Data.PartnersImport
                    {
                        IdTypeOfParther = selectedCategory.Id,
                        Reiting = Convert.ToInt32(RatingTextBox.Text),
                        PhoneOfPartner = PhoneTextBox.Text,
                        EmailOfPartner = EmailTextBox.Text
                    };

                    var searchDirector = (from item in Data.MasterPolEntities.GetContext().Directors
                                          where item.FIO == FIOTextBox.Text
                                          select item).FirstOrDefault();
                    if (searchDirector != null)
                    {
                        newPartner.IdDirector = searchDirector.Id;
                    }
                    else
                    {
                        Data.Directors directors = new Data.Directors()
                        {
                            FIO = FIOTextBox.Text
                        };
                        Data.MasterPolEntities.GetContext().Directors.Add(directors);
                        Data.MasterPolEntities.GetContext().SaveChanges();
                        newPartner.IdDirector = directors.Id;
                    }

                    var searchPartnerName = (from item in Data.MasterPolEntities.GetContext().PartnerName
                                             where item.Name == NameTextBox.Text
                                             select item).FirstOrDefault();
                    if (searchPartnerName != null)
                    {
                        newPartner.IdPartnerName = searchPartnerName.Id;
                    }
                    else
                    {
                        Data.PartnerName partnerName = new Data.PartnerName()
                        {
                            Name = NameTextBox.Text
                        };
                        Data.MasterPolEntities.GetContext().PartnerName.Add(partnerName);
                        Data.MasterPolEntities.GetContext().SaveChanges();
                        newPartner.IdPartnerName = partnerName.Id;
                    }

                    Data.MasterPolEntities.GetContext().PartnersImport.Add(newPartner);
                    MessageBox.Show("Успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                Data.MasterPolEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
