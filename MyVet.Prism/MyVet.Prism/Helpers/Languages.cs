using MyVet.Prism.Interfaces;
using MyVet.Prism.Resources;
using Xamarin.Forms;

namespace MyVet.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;

        public static string Address => Resource.Address;

        public static string AddressError => Resource.AddressError;

        public static string AddressPlaceHolder => Resource.AddressPlaceHolder;

        public static string Document => Resource.Document;

        public static string DocumentError => Resource.DocumentError;

        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;

        public static string Email => Resource.Email;

        public static string EmailError => Resource.EmailError;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Error => Resource.Error;

        public static string FirstName => Resource.FirstName;

        public static string FirstNameError => Resource.FirstNameError;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string Forgot => Resource.Forgot;

        public static string LastName => Resource.LastName;

        public static string LastNameError => Resource.LastNameError;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Login => Resource.Login;

        public static string LoginError => Resource.LoginError;

        public static string Password => Resource.Password;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordError2 => Resource.PasswordError2;

        public static string PasswordError3 => Resource.PasswordError3;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string PasswordConfirmError => Resource.PasswordConfirmError;

        public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;

        public static string Phone => Resource.Phone;

        public static string PhoneError => Resource.PhoneError;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string Register => Resource.Register;

        public static string Rememberme => Resource.Rememberme;

        public static string Connection => Resource.Connection;
        
        public static string Loading => Resource.Loading;
        
        public static string Ok => Resource.Ok;
        
        public static string RegisterNewUser => Resource.RegisterNewUser;
        
        public static string PasswordRecover => Resource.PasswordRecover;

        public static string Logout => Resource.Logout;

        public static string Map => Resource.Map;

        public static string MyAgenda => Resource.MyAgenda;

        public static string MyPets => Resource.MyPets;

        public static string MyProfile => Resource.MyProfile;

        public static string Save => Resource.Save;

        public static string ChangePassword => Resource.ChangePassword;

        public static string UserUpdated => Resource.UserUpdated;

        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;

        public static string ConfirmNewPasswordError => Resource.ConfirmNewPasswordError;

        public static string ConfirmNewPasswordPlaceHolder => Resource.ConfirmNewPasswordPlaceHolder;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string CurrentPasswordError => Resource.CurrentPasswordError;

        public static string CurrentPasswordPlaceHolder => Resource.CurrentPasswordPlaceHolder;

        public static string NewPassword => Resource.NewPassword;

        public static string NewPasswordError => Resource.NewPasswordError;

        public static string NewPasswordPlaceHolder => Resource.NewPasswordPlaceHolder;

    }
}
