using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AndroidXamarinEFCore.Services;
using AndroidXamarinEFCore.Views;
using App.Shared.Utils;
using App.Db.Remote;
using ER = App.Db.Entities.Remote;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Diagnostics;

namespace AndroidXamarinEFCore
{
    public partial class App1 : Application
    {

        public App1()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Cite: "On Android, the OnStart method will be called on rotation as well as when
            // the application first starts, if the main activity lacks
            // ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation in the [Activity()] attribute."
            // Ref: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/app-lifecycle

            try
            {
                App.Db.DataContextRemote.Init();

                var sqlParam1 = new Microsoft.Data.SqlClient.SqlParameter("Id", 555);
                string pName = sqlParam1.ParameterName;
                Debug.WriteLine(pName);

                using (App.Db.DataContextRemote dbRemote = new App.Db.DataContextRemote())
                {
                    var sqlParam2 = new SqlParameter("Id", 777);
                    string paramName = sqlParam2.ParameterName;
                    string paramValue = sqlParam2.SqlValue.ToString();
                    Debug.WriteLine($"{nameof(paramName)}: {paramName}");
                    Debug.WriteLine($"{nameof(paramValue)}: {paramValue}");

                    var machineModels = dbRemote.MachineModels.FromSqlRaw("SELECT * FROM MachineModels");
                    var machineModelsList = machineModels.ToList();
                    for (int i = 0; i < machineModelsList.Count; i++)
                    {
                        Debug.WriteLine($"{i}. MachineModel = {machineModelsList[i].Name}");
                    }

                    long mmId1 = 2;
                    ER.MachineModel mm1 = dbRemote.GetMachineModel1(mmId1);
                    Debug.WriteLine($"MachineModel1 = ({mm1.Id}, {mm1.Name})");

                    long mmId2 = 3;
                    ER.MachineModel mm2 = dbRemote.GetMachineModel2(mmId2);
                    Debug.WriteLine($"MachineModel2 = ({mm2.Id}, {mm2.Name})");
                }
            }
            catch (Exception ex)
            {
                Diagnostics.DebugWriteLineExtended(ex);
                throw;
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
