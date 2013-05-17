using System;
using System.Windows;

namespace Beast.MapMaker.Services
{
    public interface IDialogService
    {
        bool? ShowDialog<T>() where T : Window;
        bool? ShowDialog<T>(object dataContext) where T : Window;

        void PerformWaitOperation(Action callback);

        MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton button, MessageBoxImage image);
    }
}
