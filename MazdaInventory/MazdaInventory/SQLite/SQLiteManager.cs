using MazdaInventory.Commons;
using MazdaInventory.Model;
using MazdaInventory.SQLite;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MazdaInventory.SQLLite
{
    class SQLiteManager
    {
        const String cSQLiteFileName = "MazdaMBA.sqlite";
        const String cDatabaseDirectoryName = "Databases";
        const String cLibraryDirectoryName = "Library";

        readonly SQLiteAsyncConnection sqliteConnection;
        readonly Object lockObject = new object();

        static SQLiteManager sSharedInstance = null;

        /// Shared instance of SQLiteManager.
		public static SQLiteManager SharedInstance()
        {
            if (sSharedInstance == null)
            {
                sSharedInstance = new SQLiteManager(cSQLiteFileName);
            }

            return sSharedInstance;
        }

        /// Initializes a new instance of the <see cref="T:MazdaMBA.SQLite.SQLiteManager"/> class.
		public SQLiteManager(String sqliteFileName)
        {
            String lDocumentsDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents Directory in iOS
            String lDatabaseDirectoryPath = Path.Combine(lDocumentsDirectoryPath, cDatabaseDirectoryName);
            if (!Directory.Exists(lDatabaseDirectoryPath))
            {
                Directory.CreateDirectory(lDatabaseDirectoryPath);
            }
            String lSQLiteFilePath = Path.Combine(lDatabaseDirectoryPath, sqliteFileName);
            //CreateDatabase(lSQLiteFilePath);
            sqliteConnection = new SQLiteAsyncConnection(lSQLiteFilePath);
            sqliteConnection.CreateTableAsync<SQLiteRowData>(CreateFlags.ImplicitPK).Wait();
        }

        /// Gets the user preferences.
        public LoginModel GetUserPreferences()
        {
            LoginModel lLoginModel = null;

            Task<SQLiteRowData> tempSQLiteRowData = GetItemForKey(Defines.KeyUserPreferences);

            SQLiteRowData lSQLiteRowData = null;
            if (tempSQLiteRowData.Result != null)
            {
                lSQLiteRowData = tempSQLiteRowData.Result;
            }

            if (lSQLiteRowData != null)
            {
                String lValue = lSQLiteRowData.Value;
                Dictionary<String, Object> lDictionary = JsonConvert.DeserializeObject<Dictionary<String, Object>>(lValue);
                if (lDictionary != null)
                {
                    lLoginModel = new LoginModel();
                    lLoginModel.UserName = (String)lDictionary[Defines.KeyUserName];
                    lLoginModel.Password = (String)lDictionary[Defines.KeyPassword];
                    lLoginModel.ShouldRemember = (Boolean)lDictionary[Defines.KeyShouldRemember];
                    lLoginModel.Cookies = (String)lDictionary[Defines.KeyLoginCookies];
                }
            }
            return lLoginModel;
        }

        /// Saves the user preferences.
        public Boolean SaveUserPreferences(LoginModel loginModel)
        {
            Boolean lReturnValue = false;

            if (loginModel == null)
            {
                return lReturnValue;
            }

            if ((loginModel.UserName == null) || (loginModel.Password == null))
            {
                return lReturnValue;
            }

            Dictionary<String, Object> lDictionary = new Dictionary<String, Object>();
            lDictionary.Add(Defines.KeyUserName, loginModel.UserName);
            lDictionary.Add(Defines.KeyPassword, loginModel.Password);
            lDictionary.Add(Defines.KeyShouldRemember, loginModel.ShouldRemember);
            lDictionary.Add(Defines.KeyLoginCookies, loginModel.Cookies);

            String lValue = JsonConvert.SerializeObject(lDictionary);

            SQLiteRowData lSQLiteRowData = new SQLiteRowData(Defines.KeyUserPreferences, lValue);
            int tempValue = SaveItem(lSQLiteRowData).Result;
            if (tempValue > 0)
            {
                lReturnValue = true;
            }

            return lReturnValue;
        }

        /// Deletes the user preferences.
        public Boolean DeleteUserPreferences()
        {
            Boolean lReturnValue = false;

            SQLiteRowData lSQLiteRowData = new SQLiteRowData(Defines.KeyUserPreferences, "");
            int tempValue = DeleteItemAsync(lSQLiteRowData).Result;
            if (tempValue > 0)
            {
                lReturnValue = true;
            }
            return lReturnValue;

        }

        /// Deletes all items.
        public void DeleteAllItems()
        {
            // sqliteConnection.ExecuteAsync("DELETE FROM MazdaMBA.sqlite");
            sqliteConnection.DropTableAsync<SQLiteRowData>().Wait();
            sSharedInstance = null;
            /*var value = 0;
            Task<List<SQLiteRowData>> lTaskList = GetItemsAsync();
            List<SQLiteRowData> lList = lTaskList.Result;
            foreach (SQLiteRowData data in lList) {
                Console.WriteLine("Deleting data");
                value = DeleteItemAsync(data).Result;
               // await DeleteItemAsync(data);
            }

            return ; */
        }

        /*  Async Methods  */

        /// <summary>
        /// Gets the items async.
        /// </summary>
        /// <returns>The items async.</returns>
        public Task<List<SQLiteRowData>> GetItemsAsync()
        {
            lock (lockObject)
            {
                return sqliteConnection.Table<SQLiteRowData>().ToListAsync();
            }
        }

        /// <summary>
        /// Gets the items not done async.
        /// </summary>
        /// <returns>The items not done async.</returns>
        public Task<List<SQLiteRowData>> GetItemsNotDoneAsync()
        {
            lock (lockObject)
            {
                return sqliteConnection.QueryAsync<SQLiteRowData>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
            }

        }

        /* public Task<SQLiteRowData> GetItemAsync(int id)
         {
             return sqliteConnection.Table<SQLiteRowData>().Where(i => i.ID == id).FirstOrDefaultAsync();
         } */

        /// <summary>
        /// Gets the item for key.
        /// </summary>
        /// <returns>The item for key.</returns>
        /// <param name="key">Key.</param>
        public Task<SQLiteRowData> GetItemForKey(string key)
        {
            lock (lockObject)
            {
                return sqliteConnection.Table<SQLiteRowData>().Where(rowData => rowData.Key.ToLower().Equals(key.ToLower())).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <returns>The item.</returns>
        /// <param name="item">Item.</param>
        public Task<int> SaveItem(SQLiteRowData item)
        {
            /* if (item.ID != 0)
             {
                 return sqliteConnection.UpdateAsync(item);
             }
             else
             {
                 return sqliteConnection.InsertAsync(item);
             } */
            lock (lockObject)
            {
                SQLiteRowData lRowData = GetItemForKey(item.Key).Result;
                if ((lRowData != null) && (lRowData.Key.ToLower().Equals(item.Key.ToLower())))
                {
                    item.ID = lRowData.ID;
                    return sqliteConnection.UpdateAsync(item);
                }
                else
                {
                    return sqliteConnection.InsertAsync(item);
                }
            }
        }

        /// <summary>
        /// Deletes the item async.
        /// </summary>
        /// <returns>The item async.</returns>
        /// <param name="item">Item.</param>
        public Task<int> DeleteItemAsync(SQLiteRowData item)
        {
            lock (lockObject)
            {
                return sqliteConnection.DeleteAsync(item);
            }
        }
    }
}
