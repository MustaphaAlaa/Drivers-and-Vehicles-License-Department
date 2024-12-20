﻿using IServices.IApplicationServices.User;
using ModelDTO.ApplicationDTOs.User;
using Models.ApplicationModels;

namespace Services.ApplicationServices.Services.UserAppServices;

public abstract class CreateApplicationServiceValidator : ICreateApplicationServiceValidator
{
    public virtual void ValidateRequest(CreateApplicationRequest request)
    {
        if (request is null)
            throw new ArgumentNullException("Create Request is null.");

        if (request.UserId == Guid.Empty)
            throw new ArgumentException();

        if (!(Enum.IsDefined(typeof(EnServicePurpose), request.ServicePurposeId)))
            throw new ArgumentOutOfRangeException("ServicePurposeId must be contained in enum EnServicePurpose");

        if (!(Enum.IsDefined(typeof(EnServicePurpose), request.ServiceCategoryId)))
            throw new ArgumentOutOfRangeException("ServiceCategoryId must be contained in enum EnServicePurpose");

    }
}