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
    public interface IRoomImageService
    {
        #region CRUD
        Task<RoomImage> FindAsync(string id);
        IQueryable<RoomImage> GetAll();
        IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where);
        IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where, params Expression<Func<RoomImage, object>>[] includes);
        IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where, Func<IQueryable<RoomImage>, IIncludableQueryable<RoomImage, object>> include = null);
        Task AddAsync(RoomImage RoomImage);
        Task AddRange(IEnumerable<RoomImage> RoomImages);
        void Update(RoomImage RoomImage);
        Task<bool> Remove(string id);
        Task<bool> CheckExist(Expression<Func<RoomImage, bool>> where);
        Task<bool> SaveChangeAsync();
        #endregion
    }
    public class RoomImageService : IRoomImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomImageRepository _roomImageRepository;
        public RoomImageService(IUnitOfWork unitOfWork, IRoomImageRepository RoomImageRepository)
        {
            _unitOfWork = unitOfWork;
            _roomImageRepository = RoomImageRepository;
        }

        #region CRUD
        public async Task AddAsync(RoomImage RoomImage)
        {
            await _roomImageRepository.AddAsync(RoomImage);
        }

        public async Task AddRange(IEnumerable<RoomImage> RoomImages)
        {
            await _roomImageRepository.AddRangce(RoomImages);
        }

        public async Task<bool> CheckExist(Expression<Func<RoomImage, bool>> where)
        {
            return await _roomImageRepository.CheckExist(where);
        }

        public async Task<RoomImage> FindAsync(string id)
        {
            return await _roomImageRepository.FindAsync(id);
        }

        public IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where)
        {
            return _roomImageRepository.Get(where);
        }

        public IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where, params Expression<Func<RoomImage, object>>[] includes)
        {
            return _roomImageRepository.Get(where, includes);
        }

        public IQueryable<RoomImage> Get(Expression<Func<RoomImage, bool>> where, Func<IQueryable<RoomImage>, IIncludableQueryable<RoomImage, object>> include = null)
        {
            return _roomImageRepository.Get(where, include);
        }

        public IQueryable<RoomImage> GetAll()
        {
            return _roomImageRepository.GetAll();
        }

        public async Task<bool> Remove(string id)
        {
            return await _roomImageRepository.Remove(id);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(RoomImage RoomImage)
        {
            _roomImageRepository.Update(RoomImage);
        }
        #endregion
    }
}
