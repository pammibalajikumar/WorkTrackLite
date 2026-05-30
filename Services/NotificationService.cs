using System.Windows;

namespace WorkTrackLite.Services
{
    public class NotificationService
    {
        public void ShowNotification(string message)
        {
            MessageBox.Show(message,
                "WorkTrack Lite",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
