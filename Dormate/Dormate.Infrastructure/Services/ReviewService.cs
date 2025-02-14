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
    public interface IReviewService
    {
        #region CRUD
        Task<Review> FindAsync(string id);
        IQueryable<Review> GetAll();
        IQueryable<Review> Get(Expression<Func<Review, bool>> where);
        IQueryable<Review> Get(Expression<Func<Review, bool>> where, params Expression<Func<Review, object>>[] includes);
        IQueryable<Review> Get(Expression<Func<Review, bool>> where, Func<IQueryable<Review>, IIncludableQueryable<Review, object>> include = null);
        Task AddAsync(Review Review);
        Task AddRange(IEnumerable<Review> Reviews);
        void Update(Review Review);
        Task<bool> Remove(string id);
        Task<bool> CheckExist(Expression<Func<Review, bool>> where);
        Task<bool> SaveChangeAsync();
        #endregion
    }
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReviewRepository _ReviewRepository;
        public ReviewService(IUnitOfWork unitOfWork, IReviewRepository ReviewRepository)
        {
            _unitOfWork = unitOfWork;
            _ReviewRepository = ReviewRepository;
        }

        #region CRUD
        public async Task AddAsync(Review Review)
        {
            await _ReviewRepository.AddAsync(Review);
        }

        public async Task AddRange(IEnumerable<Review> Reviews)
        {
            await _ReviewRepository.AddRangce(Reviews);
        }

        public async Task<bool> CheckExist(Expression<Func<Review, bool>> where)
        {
            return await _ReviewRepository.CheckExist(where);
        }

        public async Task<Review> FindAsync(string id)
        {
            return await _ReviewRepository.FindAsync(id);
        }

        public IQueryable<Review> Get(Expression<Func<Review, bool>> where)
        {
            return _ReviewRepository.Get(where);
        }

        public IQueryable<Review> Get(Expression<Func<Review, bool>> where, params Expression<Func<Review, object>>[] includes)
        {
            return _ReviewRepository.Get(where, includes);
        }

        public IQueryable<Review> Get(Expression<Func<Review, bool>> where, Func<IQueryable<Review>, IIncludableQueryable<Review, object>> include = null)
        {
            return _ReviewRepository.Get(where, include);
        }

        public IQueryable<Review> GetAll()
        {
            return _ReviewRepository.GetAll();
        }

        public async Task<bool> Remove(string id)
        {
            return await _ReviewRepository.Remove(id);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(Review Review)
        {
            _ReviewRepository.Update(Review);
        }
        #endregion
    }
}
