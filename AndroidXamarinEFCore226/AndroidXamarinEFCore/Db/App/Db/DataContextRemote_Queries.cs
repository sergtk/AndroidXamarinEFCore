using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using App.Db.Remote;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;

using ER = App.Db.Entities.Remote;

namespace App.Db
{
    /// <summary>
    /// This file contains direct queries to DB.
    /// So they are not under security check as it is implemented in Stored Procedures.
    /// So admin only can execute them.
    /// </summary>
    public partial class DataContextRemote
    {

        public ER.MachineModel GetMachineModel1(long id)
        {
            // Ref: https://stackoverflow.com/questions/46403949/select-specific-columns-from-table-entity-framework
            var all = this.MachineModels.Where(mm => mm.Id == id);
            var ret = all.FirstOrDefault();
            return ret;
        }

        public ER.MachineModel GetMachineModel2(long id)
        {
            List<SqlParameter> allParams = new List<SqlParameter> {
                new SqlParameter("id", id)
            };
            var all = this.MachineModels.FromSql($"SELECT * FROM [dbo].[MachineModels] WHERE Id = @id", allParams.ToArray());
            var ret = all.FirstOrDefault();
            return ret;
        }


    }
}
