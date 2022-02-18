using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DeadlockWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Demo using Async/Await that "just works"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            AsyncResult.Text = "Loading...";
            var result = await DataClient.GetDataAsync();
            AsyncResult.Text = result;
        }

        /// <summary>
        /// Using "ContinueWith" without rescheduling back on the UI thread so nothing happens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinuationButton_Click(object sender, RoutedEventArgs e)
        {
            ContinuationResult.Text = "Loading...";
            DataClient.GetDataAsync().ContinueWith(t =>
            {
                ContinuationResult.Text = t.Result;
            });
        }

        /// <summary>
        /// Using "ContinueWith" and manually scheduling continuation on the Dispatcher so callback runs on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinuationDispatcherButton_Click(object sender, RoutedEventArgs e)
        {
            ContinuationDispatcherResult.Text = "Loading...";
            DataClient.GetDataAsync().ContinueWith(t =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ContinuationDispatcherResult.Text = t.Result;
                });
            });
        }

        /// <summary>
        /// Using ContinueWith and the SynchronizationContext to schedule continuation on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncContextButton_Click(object sender, RoutedEventArgs e)
        {
            SyncContextResult.Text = "Loading...";
            var syncContext = SynchronizationContext.Current!;
            DataClient.GetDataAsync().ContinueWith(t =>
            {
                syncContext.Post(delegate
                {
                    SyncContextResult.Text = t.Result;
                }, null);
            });
        }

        /// <summary>
        /// Using TaskScheduler.FromCurrentSyncContext to schedule continuation on UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskSchedulerButton_Click(object sender, RoutedEventArgs e)
        {
            TaskSchedulerResult.Text = "Loading...";
            DataClient.GetDataAsync().ContinueWith(t =>
            {
                TaskSchedulerResult.Text = t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Synchronously wait for result, blocking the main thread which prevents the continuation from being scheduled on the UI thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyncDeadlockButton_Click(object sender, RoutedEventArgs e)
        {
            SyncDeadlockResult.Text = DataClient.GetDataAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Use ConfigureAwait(false) to enable synchronously waiting on the main thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigureAwaitFalse_Click(object sender, RoutedEventArgs e)
        {
            ConfigureAwaitFalseResult.Text = DataClient.GetDataConfigureAwaitFalseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchronously attempt to load cached data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CachedAsyncData_Click(object sender, RoutedEventArgs e)
        {
            CachedAsyncDataResult.Text = DataClient.GetCachedDataAsync().GetAwaiter().GetResult();
        }
    }
}
