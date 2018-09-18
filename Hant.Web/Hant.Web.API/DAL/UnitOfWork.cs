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
        private IGenericRepository<sys_user> sys_userRepository;
        private IGenericRepository<sys_user_authorization> sys_user_authorizationRepository;
        private IGenericRepository<sys_code_number> sys_code_numberRepository;
        private IGenericRepository<sys_role> sys_roleRepository;
        public IGenericRepository<sys_user> Sys_userRepository
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

        public IGenericRepository<sys_user_authorization> Sys_user_authorizationRepository
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

        public IGenericRepository<sys_code_number> Sys_code_numberRepository
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

        public IGenericRepository<sys_role> Sys_roleRepository
        {
            get
            {
                if (sys_roleRepository == null)
                {
                    sys_roleRepository = new GenericRepository<sys_role>(context);
                }
                return sys_roleRepository;
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