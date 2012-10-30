using System;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using SubSonic.DataProviders;
using SubSonic.Repository;
using System.Collections.Generic;
using System.Reflection;

namespace TripleTriadRefresh.Data
{
    public class DbRepository
    {
        public static void Initialize()
        {
            DbRepository.Current = new SimpleRepository(ProviderFactory.GetProvider("TTF"), SimpleRepositoryOptions.RunMigrations);

            if (ConfigurationManager.AppSettings["DropDb"] == "true")
            {
                DbRepository.DropTables();
            }

            DbRepository.InitializeTables();
        }

        public static SimpleRepository Current { get; set; }

        private static void InitializeTables()
        {
            var types = DbRepository.ScanFor<DbEntity>();

            foreach (var type in types)
            {
                var method = typeof(SimpleRepository).GetMethod("Single", new Type[] { typeof(object) }).MakeGenericMethod(type);
                method.Invoke(DbRepository.Current, new object[] { null });
            }
        }

        private static void DropTables()
        {
            using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["TTF"].ConnectionString))
            {
                var types = DbRepository.ScanFor<DbEntity>();

                conn.Open();
                foreach (var type in types)
                {
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = string.Format("drop table if exists {0}s", type.Name);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private static IEnumerable<Type> ScanFor<T>()
        {
            var baseType = typeof(T);
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => 
                t != baseType && 
                baseType.IsAssignableFrom(t) && 
                !t.GetCustomAttributes(false).Any(x => x.GetType() == typeof(IgnoreCreateTableAttribute)) 
                );
        }
    }
}