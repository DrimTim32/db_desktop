using System.Net;
using RestSharp;

namespace BarProject.DesktopApplication.Common.Utils
{
    using System.Threading.Tasks;
    using System.Windows;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    public static class MessageBoxesHelper
    {

        public static async void ShowProblemWithRequest(IRestResponse response)
        {
            var code = response.StatusCode;
            string header = "";
            string message = "";
            if (response.ResponseStatus == ResponseStatus.Aborted)
            {
                header = "Aborted";
                message = "This operation has been aborted";
            }
            else if (response.ResponseStatus == ResponseStatus.TimedOut)
            {
                header = "Request timed out";
                message = "Problem connecting to the server";
            }
            else if (response.ResponseStatus == ResponseStatus.Error)
            {
                header = "Server error";
                message = response.ErrorMessage;
            }
            else if (response.ResponseStatus == ResponseStatus.Completed)
            {
                if (code == HttpStatusCode.Unauthorized)
                {
                    header = "Unauthorized";
                    message = "You do not have permission to do this";
                }
                else if (code == HttpStatusCode.Conflict)
                {
                    header = "Value duplication";
                    message = response.Content;
                }
                else if (code == HttpStatusCode.InternalServerError)
                {
                    header = "Internal server error";
                    message = response.Content;
                }
            }
            await MessageAsync(header, message);
        }
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
