﻿using System.Linq.Expressions;
using AutoMapper;
using IRepository;
using IServices.IApplicationServices.For;
using Models.ApplicationModels;



namespace Services.ApplicationServices.For;

public class GetApplicationForService : IGetApplicationFor
{
    private readonly IGetRepository<ApplicationFor> _getRepository;
    private IMapper _mapper;

    public GetApplicationForService(IGetRepository<ApplicationFor> getRepository, IMapper mapper)
    {
        _getRepository = getRepository;
        _mapper = mapper;
    }


    public async Task<ApplicationFor> GetByAsync(Expression<Func<ApplicationFor, bool>> predicate)
    {
        var appFor = await _getRepository.GetAsync(predicate);
        return appFor;

    }
}