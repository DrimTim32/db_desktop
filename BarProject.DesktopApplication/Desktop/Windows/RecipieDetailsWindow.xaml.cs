using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    public partial class RecipieDetailsWindow : MetroWindow
    {
        private ObservableCollection<ShowableRecipitDetails> _detailsList;
        private readonly object DetailsLock = new object();
        private readonly object IdLock = new object();
        private int _id;

        public ObservableCollection<ShowableRecipitDetails> DetailsList
        {
            get
            {
                lock (DetailsLock)
                {
                    if (_detailsList == null)
                        _detailsList =
                            new ObservableCollection<ShowableRecipitDetails>();
                    return _detailsList;
                }
            }
            set { lock (DetailsLock) { _detailsList = value; } }
        }

        private int Id
        {
            get { lock (IdLock) { return _id; } }
            set
            {
                lock (IdLock)
                {
                    _id = value;
                }
            }
        }

        public RecipieDetailsWindow(int id)
        {
            Id = id;
            InitializeComponent();
            Loaded += RecipieDetailsWindow_Loaded;
        }

        private void RecipieDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }

        private void RefreshData()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private void DoRefreshData()
        {
            ProgressBarStart();
            RestClient.Client().GetReceiptDetails(Id, (response) =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            DetailsList.Clear();
                            foreach (var showableCategory in response.Data)
                            {
                                DetailsList.Add(showableCategory);
                            }
                        }));
                    }
                    ProgressBarStop();
                }
            );
        }


    }
}
