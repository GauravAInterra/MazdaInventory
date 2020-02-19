using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MazdaInventory.Model
{
    public class Dealer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool RowCheck { get; set; }
        public ICommand CheckedCommand { set; get; }
    }
}
