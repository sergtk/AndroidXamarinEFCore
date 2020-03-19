using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;
//using Xamarin.Essentials;

using ER = App.Db.Entities.Remote;

namespace App.Db
{
    public partial class DataContextRemote
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ER.MachineModel>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<ER.MachineModel>().HasData(
                new ER.MachineModel() { Id = 1, Name = "AAA" },
                new ER.MachineModel() { Id = 2, Name = "BBB" },
                new ER.MachineModel() { Id = 3, Name = "CCC" }
            );
        }
    }
}
