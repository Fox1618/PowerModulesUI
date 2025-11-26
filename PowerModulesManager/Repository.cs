using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProgrammingWeapons.EntityFramework;
using Telemetria;
using Telemetria.Persistence;
using Telemetria.Persistence.Entities;

namespace PowerModulesManager
{
    public class Repository : IBindRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event Action<string> BindRepositoryError;
        public void RollBackFromDb()
        {
            throw new NotImplementedException();
        }

        public void RollBackFromDb<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void RollBackEntity(object entity)
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IBindRepository GetNew()
        {
            throw new NotImplementedException();
        }

        public Exception ExecuteInTransaction(Action action)
        {
            throw new NotImplementedException();
        }

        public bool ChangeContext(string nameConnection)
        {
            throw new NotImplementedException();
        }

        public DbContext GlobalContext { get; }
        public dynamic ObjectContext { get; }
        public bool CheckConnection(string nameConnection)
        {
            throw new NotImplementedException();
        }

        public bool CheckUserRole(int userId, string role)
        {
            throw new NotImplementedException();
        }

        public DbSet<T> Query<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity, bool isAttach = true) where T : class
        {
            throw new NotImplementedException();
        }

        public void Insert<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void RollBackNavigationProperty<T, TRes>(T entity, Expression<Func<T, TRes>> navigationProperty) where T : class where TRes : class
        {
            throw new NotImplementedException();
        }

        public void RollBackNavigationProperties<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public void RollBackEntityFull<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public T GetEntityInCurrentContext<T>(T entity, BarcodeType barcodeType = BarcodeType.Unknown) where T : class, IEntity
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public IBindRepository GetNewRepositoryByUser(IUserInfo user)
        {
            throw new NotImplementedException();
        }
    }
}
