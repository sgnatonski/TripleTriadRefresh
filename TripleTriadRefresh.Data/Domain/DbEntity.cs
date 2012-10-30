using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SubSonic.SqlGeneration.Schema;

namespace TripleTriadRefresh.Data
{
    public class DbEntity
    {
        [SubSonicPrimaryKey]
        public int Id { get; set; }

        private readonly Hashtable foreignCache = new Hashtable();

        protected T GetForeign<T>(int key) where T : DbEntity, new()
        {
            string relation = typeof(T).Name;
            T foreign = this.foreignCache[relation] as T;
            if (foreign == null || foreign.Id != key)
            {
                foreign = DbRepository.Current.Single<T>(key);
                this.foreignCache[relation] = foreign;
            }
            return foreign;
        }

        protected int SetForeign<T>(T foreign) where T : DbEntity, new()
        {
            string relation = typeof(T).Name;
            this.foreignCache[relation] = foreign;
            return (foreign == null) ? 0 : foreign.Id;
        }

        protected List<T> GetForeignList<T>(Expression<Func<T, bool>> expression) where T : DbEntity, new()
        {
            return this.GetForeignList<T>(expression, false);
        }

        protected List<T> GetForeignList<T>(Expression<Func<T, bool>> expression, bool refresh) where T : DbEntity, new()
        {
            string relation = string.Format("l-{0}", typeof(T).Name);
            List<T> foreign = this.foreignCache[relation] as List<T>;
            if (foreign == null || refresh)
            {
                foreign = DbRepository.Current.Find<T>(expression).ToList();
                this.foreignCache[relation] = foreign;
            }
            return foreign;
        }
    }
}