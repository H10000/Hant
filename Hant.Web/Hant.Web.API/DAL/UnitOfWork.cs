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
        private GenericRepository<sys_user_authorization> sys_user_authorizationRepository;
        private GenericRepository<sys_code_number> sys_code_numberRepository;
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

        public GenericRepository<sys_user_authorization> Sys_user_authorizationRepository
        {
            get
            {
                if (sys_user_authorizationRepository == null)
                {
                    sys_user_authorizationRepository = new GenericRepository<sys_user_authorization>(context);
                }
                return sys_user_authorizationRepository;
            }
        }

        public GenericRepository<sys_code_number> Sys_code_numberRepository
        {
            get
            {
                if (sys_code_numberRepository == null)
                {
                    sys_code_numberRepository = new GenericRepository<sys_code_number>(context);
                }
                return sys_code_numberRepository;
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
        protected virtual void Dispose(bool disposing)
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