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

        public static string RFCPlaceHolder => Resource.RFCPlaceHolder;

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

        public static string NewPet => Resource.NewPet;

        public static string Delete => Resource.Delete;

        public static string EditPet => Resource.EditPet;

        public static string ChangeImage => Resource.ChangeImage;

        public static string Name => Resource.Name;

        public static string NameError => Resource.NameError;

        public static string NamePlaceHolder => Resource.NamePlaceHolder;

        public static string Race => Resource.Race;

        public static string RaceError => Resource.RaceError;

        public static string RacePlaceHolder => Resource.RacePlaceHolder;

        public static string PetType => Resource.PetType;

        public static string PetTypeError => Resource.PetTypeError;

        public static string PetTypePlaceHolder => Resource.PetTypePlaceHolder;

        public static string Born => Resource.Born;

        public static string Remarks => Resource.Remarks;

        public static string PictureSource => Resource.PictureSource;

        public static string Cancel => Resource.Cancel;

        public static string FromCamera => Resource.FromCamera;

        public static string FromGallery => Resource.FromGallery;

        public static string CreateEditPetConfirm => Resource.CreateEditPetConfirm;

        public static string Created => Resource.Created;

        public static string Edited => Resource.Edited;

        public static string Confirm => Resource.Confirm;

        public static string QuestionToDeletePet => Resource.QuestionToDeletePet;

        public static string Yes => Resource.Yes;

        public static string No => Resource.No;

        public static string AssignModifyAgenda => Resource.AssignModifyAgenda;

        public static string Assign => Resource.Assign;

        public static string CancelAgendaMessage => Resource.CancelAgendaMessage;
        
        public static string PetError => Resource.PetError;

        public static string Saving => Resource.Saving;

        public static string NotAddressFound => Resource.NotAddressFound;

        public static string NotLocationAvailable => Resource.NotLocationAvailable;

        public static string SelectAnAdrress => Resource.SelectAnAdrress;
        
        public static string Diary => Resource.Diary;
        
        public static string Histories => Resource.Histories;
        
        public static string History => Resource.History;
        
        public static string ErrorUser => Resource.ErrorUser;
        
        public static string Details => Resource.Details;
        
        public static string Pets => Resource.Pets;
        
        public static string PetsOf => Resource.PetsOf;
        
        public static string Pet => Resource.Pet;

        public static string ErrorRFC => Resource.ErrorRFC;

        public static string ErrorFirstName => Resource.ErrorFirstName;
        
        public static string ErrorLastName => Resource.ErrorLastName;
        
        public static string ErrorAddress => Resource.ErrorAddress;
        
        public static string ErrorEmail => Resource.ErrorEmail;
        
        public static string ErrorPhone => Resource.ErrorPhone;
        
        public static string ErrorPassword => Resource.ErrorPassword;
        
        public static string ErrorPasswordConfirm => Resource.ErrorPasswordConfirm;
        
        public static string ErrorPasswordConfirm2 => Resource.ErrorPasswordConfirm2;
        
        public static string Changing => Resource.Changing;
        
        public static string Deleting => Resource.Deleting;
        
        public static string Date => Resource.Date;
     
        public static string ServiceType => Resource.ServiceType;
        
        public static string Description => Resource.Description;
        public static string AgendaFor => Resource.AgendaFor;
        public static string PetPlaceHolder => Resource.PetPlaceHolder;

    }
}
