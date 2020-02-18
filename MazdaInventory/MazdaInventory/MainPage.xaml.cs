using MazdaInventory.Commons;
using MazdaInventory.ConnectionManager;
using MazdaInventory.Model;
using MazdaInventory.SQLite;
using MazdaInventory.SQLLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace MazdaInventory
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, IConnectionCallbacks
    {

        String UserName;
        String Password;
        Boolean ShouldRememberMe;
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        public MainPage()
        {
            InitializeComponent();

            // Define the binding context
            BindingContext = this;

            // Set the IsBusy property
            IsBusy = false;

            var forgetPassword_tap = new TapGestureRecognizer();
            forgetPassword_tap.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new ForgotPasswordPage());
            };
            forgotPassword.GestureRecognizers.Add(forgetPassword_tap);

        }

        private void RememberMeChecked(object sender, EventArgs e)
        {
            if (rememberMe.IsChecked)
            {
                ShouldRememberMe = true;
                rememberMe.BoxBackgroundColor = Color.Black;
            }
            else
            {
                ShouldRememberMe = false;
                rememberMe.BoxBackgroundColor = Color.White;
            }
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            UserName = userName.Text;
            Password = passwordHidden.Text;
            ShouldRememberMe = rememberMe.IsChecked;
            HitLogin();
        }

        void HitLogin()
        {
            try
            {
                MainController ms = new MainController();
                ms.LoginRequest(UserName, Password, this, Defines.LoginRequestServerHit);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.Out.WriteLine("Lgin Screen: \r {0}", ex.Message);
            }
        }

        public void ConnectionWasSuccessFullWithResult(object result, string RequestID)
        {
            try
            {
                IsBusy = false;
                if (RequestID == Defines.LoginRequestServerHit)
                {
                    Dictionary<String, Object> lDictionary = (Dictionary<String, Object>)result;
                    String lConnectionResponseMessage = (String)lDictionary[Defines.KeyConnectionResponseMessage];
                    String lLoginCookies = (String)lDictionary[Defines.KeyLoginCookies];
                    LoginModel lLoginModel = new LoginModel();
                    lLoginModel.UserName = UserName;
                    lLoginModel.Password = Password;
                    lLoginModel.Cookies = lLoginCookies;
                    lLoginModel.ShouldRemember = ShouldRememberMe;
                    var qaActivated = Utilities.getEnvironmentValue();
                    Console.Out.WriteLine("Login Screen Success: \r {0}", lConnectionResponseMessage);

                    SQLiteRowData dataTime = new SQLiteRowData();
                    SQLiteRowData dataView = new SQLiteRowData();

                    dataTime = Utilities.getTimePeriod();
                    dataView = Utilities.getDefaultView();

                    Utilities.deleteAllDataFromSQlite();
                    Utilities.saveEnvironment(qaActivated);
                    if (dataTime != null)
                    {
                        Utilities.saveTimePeriod(dataTime.Value);
                    }
                    if (dataView != null)
                    {
                        Utilities.saveDefaultView(dataView.Value);
                    }
                    SQLiteManager.SharedInstance().SaveUserPreferences(lLoginModel);
                    Navigation.PushAsync(new DealerSearchPage(false));
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Login Screen Success Exception: \r {0}", ex.Message);
                DisplayAlert("Login Alert", "Could not Login. Please try again!!", "OK");
            }
        }

        public void ConnectionFailedWithError(object error, string RequestID)
        {
            try
            {
                IsBusy = false;
                Dictionary<String, Object> lErrorDictionary = (Dictionary<String, Object>)error;
                string lErrorMessage = (String)lErrorDictionary[Defines.KeyConnectionResponseMessage];
                Console.Out.WriteLine("Login Screen Error: \r {0}", lErrorMessage);
                DisplayAlert("Login Alert", lErrorMessage, "OK");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Login Screen Error Exception: \r {0}", ex.Message);
                DisplayAlert("Login Alert", "Something went Wrong. Please try again!!", "OK");
            }
        }
    }
}
