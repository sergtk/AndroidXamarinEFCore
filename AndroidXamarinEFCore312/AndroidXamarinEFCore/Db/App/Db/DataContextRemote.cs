using System;
using System.Collections.Generic;
//using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;

using ER = App.Db.Entities.Remote;

namespace App.Db
{
    public class DisposableStub: IDisposable
    {
        public void Dispose() { }
    }

    public partial class DataContextRemote : DbContext
    {
        public DbSet<ER.MachineModel> MachineModels { get; set; }
        
        public static string ConnectionString { get; set; }

        public static string UserName { get; set; }

        public static string Password { get; set; }

        private string LogFileName { get; set; }

        public static void Init()
        {
            App.Db.DataContextRemote.ConnectionString = App.Db.Config.DbRemoteConnectionString;
            App.Db.DataContextRemote.UserName = Config.DbRemoteUserName;
            App.Db.DataContextRemote.Password = Config.DbRemotePassword;
        }

        public DataContextRemote()
        {

        }

        public DataContextRemote(string logFileName)
        {
            LogFileName = logFileName;
        }


        public Exception ErrorException{ get; private set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CanConnectToDatabaseAsync()
        {
            bool ret = await this.Database.CanConnectAsync();
            return ret;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString == null)
            {
                ConnectionString = ";";
            }

            string conStr = ConnectionString.Replace("{UserName}", UserName).Replace("{Password}", Password);

            optionsBuilder.UseSqlServer(conStr);
        }
    }
}
