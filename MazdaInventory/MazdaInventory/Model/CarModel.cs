using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MazdaInventory.Model
{
    public class Car
    {
        public string Name { get; set; }
        public string Amount { get; set; }

        // I assume if color is   
        // Green, then user has vote  
        // Red, then user is below 18  
        public Color HasVote { get; set; }
    }
}
