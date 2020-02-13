using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MazdaInventory.Model
{
    public class CarViewModel
    {
        public IList<Car> lstUsers { get; set; }
        public object SelectedItem { get; set; }
        public CarViewModel()
        {
            lstUsers = new List<Car>();
            GenerateCardModel();
        }

        private void GenerateCardModel()
        {
            string[] arrNames = {
                "David", "John", "Paul", "Mark", "James",
                "Andrew", "Scott", "Steven", "Robert", "Stephen",
                "William", "Craig", "Michael", "Stuart", "Christopher",
                "Alan", "Colin", "Brian"
            };

            Random rnd = new Random();

            for (var i = 0; i < arrNames.Length; i++)
            {
                var age = rnd.Next(10000, 30000);
                var user = new Car()
                {
                    Name = arrNames[i],
                    Amount = ("$ " + age),
                    HasVote = age % 2 == 0 ? Color.Green : Color.Red,
                };
                lstUsers.Add(user);
            }
        }
    }
}
