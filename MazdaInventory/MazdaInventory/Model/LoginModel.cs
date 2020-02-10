using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.Model
{
    public class LoginModel
    {
        public String UserName;
        /// The password.
        public String Password;
        /// The shouldRememberMe value.
        public Boolean ShouldRemember;
        /// cookies.
        public String Cookies;
    }
}
