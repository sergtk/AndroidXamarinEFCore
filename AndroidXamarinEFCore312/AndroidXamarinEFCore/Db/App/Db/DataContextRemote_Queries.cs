using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;

using ER = App.Db.Entities.Remote;

namespace App.Db
{
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
            var all = this.MachineModels.FromSqlRaw($"SELECT * FROM [dbo].[MachineModels] WHERE Id = @id", allParams.ToArray());
            var ret = all.FirstOrDefault();
            return ret;
        }


    }
}
