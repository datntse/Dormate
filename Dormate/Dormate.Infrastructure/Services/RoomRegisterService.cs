using Dormate.Core.Entities;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Services
{
    public interface IRoomRegisterService
    {
        #region CRUD
        Task<RoomRegister> FindAsync(string id);
        IQueryable<RoomRegister> GetAll();
        IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where);
        IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where, params Expression<Func<RoomRegister, object>>[] includes);
        IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where, Func<IQueryable<RoomRegister>, IIncludableQueryable<RoomRegister, object>> include = null);
        Task AddAsync(RoomRegister RoomRegister);
        Task AddRange(IEnumerable<RoomRegister> RoomRegisters);
        void Update(RoomRegister RoomRegister);
        Task<bool> Remove(string id);
        Task<bool> CheckExist(Expression<Func<RoomRegister, bool>> where);
        Task<bool> SaveChangeAsync();
        #endregion

    }
    public class RoomRegisterService : IRoomRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRegisterRepository _RoomRegisterRepository;
        public RoomRegisterService(IUnitOfWork unitOfWork, IRoomRegisterRepository RoomRegisterRepository)
        {
            _unitOfWork = unitOfWork;
            _RoomRegisterRepository = RoomRegisterRepository;
        }

        #region CRUD
        public async Task AddAsync(RoomRegister RoomRegister)
        {
            await _RoomRegisterRepository.AddAsync(RoomRegister);
        }

        public async Task AddRange(IEnumerable<RoomRegister> RoomRegisters)
        {
            await _RoomRegisterRepository.AddRangce(RoomRegisters);
        }

        public async Task<bool> CheckExist(Expression<Func<RoomRegister, bool>> where)
        {
            return await _RoomRegisterRepository.CheckExist(where);
        }

        public async Task<RoomRegister> FindAsync(string id)
        {
            return await _RoomRegisterRepository.FindAsync(id);
        }

        public IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where)
        {
            return _RoomRegisterRepository.Get(where);
        }

        public IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where, params Expression<Func<RoomRegister, object>>[] includes)
        {
            return _RoomRegisterRepository.Get(where, includes);
        }

        public IQueryable<RoomRegister> Get(Expression<Func<RoomRegister, bool>> where, Func<IQueryable<RoomRegister>, IIncludableQueryable<RoomRegister, object>> include = null)
        {
            return _RoomRegisterRepository.Get(where, include);
        }

        public IQueryable<RoomRegister> GetAll()
        {
            return _RoomRegisterRepository.GetAll();
        }

        public async Task<bool> Remove(string id)
        {
            return await _RoomRegisterRepository.Remove(id);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(RoomRegister RoomRegister)
        {
            _RoomRegisterRepository.Update(RoomRegister);
        }
        #endregion
    }
}
