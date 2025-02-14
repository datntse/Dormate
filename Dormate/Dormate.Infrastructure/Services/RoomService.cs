using Dormate.Core.Constant;
using Dormate.Core.Entities;
using Dormate.Core.Models;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dormate.Infrastructure.Services
{

    public interface IRoomService
    {
        #region CRUD
        Task<Room> FindAsync(string id);
        IQueryable<Room> GetAll();
        IQueryable<Room> Get(Expression<Func<Room, bool>> where);
        IQueryable<Room> Get(Expression<Func<Room, bool>> where, params Expression<Func<Room, object>>[] includes);
        IQueryable<Room> Get(Expression<Func<Room, bool>> where, Func<IQueryable<Room>, IIncludableQueryable<Room, object>> include = null);
        Task AddAsync(Room room);
        Task AddRange(IEnumerable<Room> rooms);
        void Update(Room room);
        Task<bool> Remove(string id);
        Task<bool> CheckExist(Expression<Func<Room, bool>> where);
        Task<bool> SaveChangeAsync();
        #endregion
    }
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRepository _roomRepository;
        public RoomService(IUnitOfWork unitOfWork, IRoomRepository roomRepository)
        {
            _unitOfWork = unitOfWork;
            _roomRepository = roomRepository;
        }

        #region CRUD
        public async Task AddAsync(Room Room)
        {
            await _roomRepository.AddAsync(Room);
        }

        public async Task AddRange(IEnumerable<Room> Rooms)
        {
            await _roomRepository.AddRangce(Rooms);
        }

        public async Task<bool> CheckExist(Expression<Func<Room, bool>> where)
        {
            return await _roomRepository.CheckExist(where);
        }

        public async Task<Room> FindAsync(string id)
        {
            return await _roomRepository.FindAsync(id);
        }

        public IQueryable<Room> Get(Expression<Func<Room, bool>> where)
        {
            return _roomRepository.Get(where);
        }

        public IQueryable<Room> Get(Expression<Func<Room, bool>> where, params Expression<Func<Room, object>>[] includes)
        {
            return _roomRepository.Get(where, includes);
        }

        public IQueryable<Room> Get(Expression<Func<Room, bool>> where, Func<IQueryable<Room>, IIncludableQueryable<Room, object>> include = null)
        {
            return _roomRepository.Get(where, include);
        }

        public IQueryable<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public async Task<bool> Remove(string id)
        {
            return await _roomRepository.Remove(id);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(Room Room)
        {
            _roomRepository.Update(Room);
        }
        #endregion
    }
}
