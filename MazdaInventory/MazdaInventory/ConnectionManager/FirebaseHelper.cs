using Firebase.Database;
using MazdaInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazdaInventory.ConnectionManager
{
    class FirebaseHelper
    {

        FirebaseClient firebase = new FirebaseClient("https://mazdainventorytracker.firebaseio.com/");

        public async Task<List<AndroidVersionModel>> GetAllVersions()
        {

            return (await firebase
              .Child("android")
              .OnceAsync<AndroidVersionModel>()).Select(item => new AndroidVersionModel
              {
                  updated_version = item.Object.updated_version,
                  forced_update = item.Object.forced_update
              }).ToList();
        }

        public async Task<AndroidVersionModel> GetVersion()
        {
            var allPersons = await GetAllVersions();
            await firebase
              .Child("android")
              .OnceAsync<AndroidVersionModel>();
            return allPersons.FirstOrDefault();
        }
    }
}
