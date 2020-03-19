namespace App.Db
{
    public static class Config
    {
        public static string DbRemoteUserName = "USERNAME";
        public static string DbRemotePassword = "PASSWORD";

        public static string DbRemoteConnectionString = "Server=tcp:<SERVER>.database.windows.net,1433;Initial Catalog=<DATABASE>;" +
            "Persist Security Info=False;User ID={UserName};Password={Password};" +
            "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
