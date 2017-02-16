namespace BarProject.DesktopApplication.Common.Utils
{
    using System.Threading.Tasks;
    using System.Windows;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public static class MessageBoxesHelper
    {
        public static async void ShowWindowInformationAsync(string header, string message)
        {
            await MessageAsync(header, message);
        }
        public static void ShowWindowInformation(string header, string message)
        {
            Message(header, message);
        }

        public static MessageDialogResult ShowYesNoMessage(string header, string message)
        {
            return Message(header, message, MessageDialogStyle.AffirmativeAndNegative);
        }
        public static Task<MessageDialogResult> ShowYesNoMessageAsync(string header, string message)
        {
            return MessageAsync(header, message, MessageDialogStyle.AffirmativeAndNegative);
        }
        private static MessageDialogResult Message(string header, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            MessageDialogResult data = default(MessageDialogResult);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var owner = Application.Current.MainWindow as MetroWindow;
                owner.Dispatcher.Invoke(() =>
                {
                    data = owner.ShowModalMessageExternal(header, message, style);
                });

            });

            return data;
        }
        private static async Task<MessageDialogResult> MessageAsync(string header, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            MessageDialogResult data = default(MessageDialogResult);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var owner = Application.Current.MainWindow as MetroWindow;
                owner.Dispatcher.InvokeAsync(() =>
                {
                    data = owner.ShowModalMessageExternal(header, message, style);
                });

            });

            return data;
        }
    }
}
