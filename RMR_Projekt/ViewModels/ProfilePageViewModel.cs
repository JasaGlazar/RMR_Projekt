using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt.ViewModels
{
    class ProfilePageViewModel
    {
        public INavigation _navigation;
        public Command LogoutBtn { get; }

        public ProfilePageViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            LogoutBtn = new Command(LogoutBtnTappedAsync);

        }

        private async void LogoutBtnTappedAsync(object obj)
        {
            await _navigation.PushAsync(new Prijava());
        }
    }
}
