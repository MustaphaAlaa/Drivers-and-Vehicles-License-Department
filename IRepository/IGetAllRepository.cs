﻿using System.Linq.Expressions;

namespace IRepository;

public interface IGetAllRepository<T >
{
    public Task<List<T >> GetAllAsync();
    public Task<IQueryable<T >> GetAllAsync(Expression<Func<T , bool>> predicate);
}