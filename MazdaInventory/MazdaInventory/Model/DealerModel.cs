using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MazdaInventory.Model
{
    /// Timings.
    [XmlRoot(ElementName = "timings")]
    public class Timings
    {
        /// Gets or sets the service Day.
        [XmlElement(ElementName = "service")]
        public string Service { get; set; }
        /// Gets or sets Day = sunday.
        [XmlElement(ElementName = "sunday")]
        public string Sunday { get; set; }
        /// Gets or sets Day = monday.
        [XmlElement(ElementName = "monday")]
        public string Monday { get; set; }
        /// Gets or sets Day = tuesday.
        [XmlElement(ElementName = "tuesday")]
        public string Tuesday { get; set; }
        /// Gets or sets Day = wednesday.
        [XmlElement(ElementName = "wednesday")]
        public string Wednesday { get; set; }
        /// Gets or sets Day = thursday.
        [XmlElement(ElementName = "thursday")]
        public string Thursday { get; set; }
        /// Gets or sets Day = friday.
        [XmlElement(ElementName = "friday")]
        public string Friday { get; set; }
        /// Gets or sets Day = saturday.
        [XmlElement(ElementName = "saturday")]
        public string Saturday { get; set; }
    }

    /// Workinghour.
    [XmlRoot(ElementName = "workinghour")]
    public class Workinghour
    {
        /// Gets or sets the timings.
        [XmlElement(ElementName = "timings")]
        public List<Timings> Timings { get; set; }
    }

    /// Dealer.
    [XmlRoot(ElementName = "dealer")]
    public class Dealer
    {
        /// Gets or sets the identifier.
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        /// Gets or sets the person identifier.
        [XmlElement(ElementName = "personId")]
        public string PersonId { get; set; }
        /// Gets or sets the country code.
        [XmlElement(ElementName = "countryCode")]
        public string CountryCode { get; set; }
        /// Gets or sets the language code.
        [XmlElement(ElementName = "languageCode")]
        public string LanguageCode { get; set; }
        /// Gets or sets the person is dealer or not.
        [XmlElement(ElementName = "isdealer")]
        public string Isdealer
        { get; set; }
        /// Gets or sets the name.
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        /// Gets or sets the last name.
        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }
        /// Gets or sets the region code.
        [XmlElement(ElementName = "regionCode")]
        public string RegionCode { get; set; }
        /// Gets or sets the district code.
        [XmlElement(ElementName = "districtCode")]
        public string DistrictCode { get; set; }
        /// Gets or sets the location code.
        [XmlElement(ElementName = "locationCode")]
        public string LocationCode { get; set; }
        /// Gets or sets the dealer code.
        [XmlElement(ElementName = "dealerCode")]
        public string DealerCode { get; set; }
        /// Gets or sets the name of the dealer.
        [XmlElement(ElementName = "dealerName")]
        public string DealerName { get; set; }
        /// Gets or sets the dealer address.
        [XmlElement(ElementName = "dealerAddress")]
        public string DealerAddress { get; set; }
        /// Gets or sets the dealer phone no.
        [XmlElement(ElementName = "dealerPhoneNo")]
        public string DealerPhoneNo { get; set; }
        /// Gets or sets the dealer email.
        [XmlElement(ElementName = "dealerEmail")]
        public string DealerEmail { get; set; }
        /// Gets or sets the latitude.
        [XmlElement(ElementName = "latitude")]
        public string Latitude { get; set; }
        /// Gets or sets the longitude.
        [XmlElement(ElementName = "longitude")]
        public string Longitude { get; set; }
        /// Gets or sets the workinghour.
        [XmlElement(ElementName = "workinghour")]
        public Workinghour Workinghour { get; set; }
    }
}
