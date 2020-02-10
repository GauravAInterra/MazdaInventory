using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MazdaInventory.SQLite
{
    class SQLiteRowData
    {
        const String cDefaultKey = "DefaultKey";
        const String cDefaultValue = "DefaultValue";

        /// Initializes a new instance of the <see cref="T:MazdaMBA.SQLite.SQLiteRowData"/> class.
        public SQLiteRowData()
        {
        }

        /// Initializes a new instance of the <see cref="T:MazdaMBA.SQLite.SQLiteRowData"/> class.
        public SQLiteRowData(String key = cDefaultKey, String aValue = cDefaultValue)
        {
            Key = key;
            Value = aValue;
        }

        /// Gets or sets the identifier.
		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// Gets or sets the key.
        public String Key { get; set; } = cDefaultValue;

        /// Gets or sets the value.
        public String Value { get; set; } = cDefaultValue;

        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:MazdaMBA.SQLite.SQLiteRowData"/>.
		public override String ToString()
        {
            return String.Format("[SQLiteRowData: ID={0}, Key={1}, Value={2}]", ID, Key, Value);
        }
    }
}
