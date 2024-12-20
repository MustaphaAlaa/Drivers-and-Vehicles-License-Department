﻿
using AutoMapper;
using FluentAssertions;
using IRepository;
using IServices.IApplicationServices.Fees;
using Models.ApplicationModels;
using Moq;
using Services.ApplicationServices;
using Services.ApplicationServices.Fees;
using System.Linq.Expressions;

namespace DVLD_Tests.ApplicationServices;

public class GetApplicationFeesTest
{
    private readonly Mock<IMapper> _mapper;

    private readonly Mock<IGetRepository<ApplicationFees>> _getRepository;
    private readonly IGetApplicationFees _getApplicationFees;
    public GetApplicationFeesTest()
    {
        _mapper = new Mock<IMapper>();
        _getRepository = new Mock<IGetRepository<ApplicationFees>>();

        _getApplicationFees = new GetApplicationFeesService(_getRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetApplicationFees_ApplicationFeesDoesNotExist_ReturnNull()
    {
        //Arrange
        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<ApplicationFees, bool>>>()))
           .ReturnsAsync(null as ApplicationFees);

        //Act
        var result = await _getApplicationFees.GetByAsync(app => app.Fees > 500);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetApplicationFees_ApplicationFeesDoesExist_ReturnApplicationFeesObj()
    {
        //Arrange
        ApplicationFees applicationFees = new()
        {
            ApplicationForId = 1,
            ApplicationTypeId = 2,
            Fees = 100,
            LastUpdate = DateTime.Now,
        };

        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<ApplicationFees, bool>>>()))
           .ReturnsAsync(applicationFees);

        //Act
        var result = await _getApplicationFees.GetByAsync(app => app.Fees > 500);

        //Assert
        result.Should().BeEquivalentTo(applicationFees);
    }
}
