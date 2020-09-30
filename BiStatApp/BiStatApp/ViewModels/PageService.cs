using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BiStatApp.ViewModels
{
    public class PageService : IPageService
    {
        public async Task DisplayAlert(string title, string message, string ok)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, ok);
        }

        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        public async Task PushAsync(Page page)
        {
            await MainPage.PushAsync(page);
        }

        public async Task<Page> PopAsync()
        {
            return await MainPage.PopAsync();
        }

        public INavigation MainPage { get; set; }
    }
}
