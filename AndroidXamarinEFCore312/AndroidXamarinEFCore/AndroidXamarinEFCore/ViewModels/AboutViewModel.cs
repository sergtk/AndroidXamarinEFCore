using System;
using System.Windows.Input;
//using Xamarin.Essentials;
using Xamarin.Forms;

namespace AndroidXamarinEFCore.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            //OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
            OpenWebCommand = new Command(() => { });
        }

        public ICommand OpenWebCommand { get; }
    }
}