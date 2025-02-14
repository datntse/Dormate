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
    public interface IFavouriteRoomService
    {
        #region CRUD
        Task<FavouriteRoom> FindAsync(string id);
        IQueryable<FavouriteRoom> GetAll();
        IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where);
        IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where, params Expression<Func<FavouriteRoom, object>>[] includes);
        IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where, Func<IQueryable<FavouriteRoom>, IIncludableQueryable<FavouriteRoom, object>> include = null);
        Task AddAsync(FavouriteRoom FavouriteRoom);
        Task AddRange(IEnumerable<FavouriteRoom> FavouriteRooms);
        void Update(FavouriteRoom FavouriteRoom);
        Task<bool> Remove(string id);
        Task<bool> CheckExist(Expression<Func<FavouriteRoom, bool>> where);
        Task<bool> SaveChangeAsync();
        #endregion
    }
    public class FavouriteRoomService : IFavouriteRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFavouriteRoomRepository _FavouriteRoomRepository;
        public FavouriteRoomService(IUnitOfWork unitOfWork, IFavouriteRoomRepository FavouriteRoomRepository)
        {
            _unitOfWork = unitOfWork;
            _FavouriteRoomRepository = FavouriteRoomRepository;
        }

        #region CRUD
        public async Task AddAsync(FavouriteRoom FavouriteRoom)
        {
            await _FavouriteRoomRepository.AddAsync(FavouriteRoom);
        }

        public async Task AddRange(IEnumerable<FavouriteRoom> FavouriteRooms)
        {
            await _FavouriteRoomRepository.AddRangce(FavouriteRooms);
        }

        public async Task<bool> CheckExist(Expression<Func<FavouriteRoom, bool>> where)
        {
            return await _FavouriteRoomRepository.CheckExist(where);
        }

        public async Task<FavouriteRoom> FindAsync(string id)
        {
            return await _FavouriteRoomRepository.FindAsync(id);
        }

        public IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where)
        {
            return _FavouriteRoomRepository.Get(where);
        }

        public IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where, params Expression<Func<FavouriteRoom, object>>[] includes)
        {
            return _FavouriteRoomRepository.Get(where, includes);
        }

        public IQueryable<FavouriteRoom> Get(Expression<Func<FavouriteRoom, bool>> where, Func<IQueryable<FavouriteRoom>, IIncludableQueryable<FavouriteRoom, object>> include = null)
        {
            return _FavouriteRoomRepository.Get(where, include);
        }

        public IQueryable<FavouriteRoom> GetAll()
        {
            return _FavouriteRoomRepository.GetAll();
        }

        public async Task<bool> Remove(string id)
        {
            return await _FavouriteRoomRepository.Remove(id);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(FavouriteRoom FavouriteRoom)
        {
            _FavouriteRoomRepository.Update(FavouriteRoom);
        }
        #endregion
    }
}
