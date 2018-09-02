using Hant.Web.API.DAL.Entity;
using Hant.Web.API.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hant.Web.API.DAL
{
    public class UnitOfWork : IDisposable
    {
        private AccountContext context = new AccountContext();
        private GenericRepository<sys_user> sys_userRepository;

        public GenericRepository<sys_user> Sys_userRepository
        {
            get
            {
                if (sys_userRepository == null)
                {
                    sys_userRepository = new GenericRepository<sys_user>(context);
                }
                return sys_userRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        private bool disposed = false;
        public void Dispose()
         {
             Dispose(true);
             GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {

            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}