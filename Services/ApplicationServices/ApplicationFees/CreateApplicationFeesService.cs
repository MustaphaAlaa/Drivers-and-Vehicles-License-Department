﻿using AutoMapper;
using IRepository;
using IServices.IApplicationServices.Fees;
using ModelDTO.ApplicationDTOs.Fees;
using Models.ApplicationModels;


namespace Services.ApplicationServices.Fees;

public class CreateApplicationFeesService : ICreateApplicationFees
{
    private readonly IMapper _mapper; //
    private readonly IGetRepository<ApplicationFees> _getRepository;
    private readonly ICreateRepository<ApplicationFees> _createRepository;

    public CreateApplicationFeesService(IMapper mapper,
        IGetRepository<ApplicationFees> getRepository,
        ICreateRepository<ApplicationFees> createRepository)
    {
        _mapper = mapper;
        _getRepository = getRepository;
        _createRepository = createRepository;
    }

    public async Task<ApplicationFeesDTO> CreateAsync(ApplicationFeesDTO entity)
    {
        if (entity == null)
            throw new ArgumentNullException($"ApplicationFeesDTo is null");

        if (entity.ApplicationTypeId <= 0)
            throw new ArgumentOutOfRangeException("Type id must be greater than 0");

        if (entity.ApplicationForId <= 0)
            throw new ArgumentOutOfRangeException("For id must be greater than 0");

        if (entity.Fees < 0)
            throw new ArgumentOutOfRangeException("For id must be greater than 0");


        var applicationFees = await _getRepository.GetAsync(appFees =>
              appFees.ApplicationTypeId == entity.ApplicationTypeId && appFees.ApplicationForId == entity.ApplicationTypeId);

        if (applicationFees != null)
            throw new Exception("ApplicationFees is already exist you cannot recreate it only update it");

        ApplicationFees createReq = _mapper.Map<ApplicationFees>(entity)
             ?? throw new AutoMapperMappingException();



        var createdAppFees = await _createRepository.CreateAsync(createReq);

        if (createdAppFees is null)
            throw new Exception();

        var AppFeesDTO = _mapper.Map<ApplicationFeesDTO>(createdAppFees);

        if (AppFeesDTO is null)
            throw new AutoMapperMappingException();

        return AppFeesDTO;
    }
}
