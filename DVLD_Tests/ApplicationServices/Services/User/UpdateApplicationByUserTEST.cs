﻿using AutoFixture;
using AutoMapper;
using FluentAssertions;
using IRepository;
using IServices.IApplicationServices.User;
using Microsoft.AspNetCore.Routing.Template;
using ModelDTO.ApplicationDTOs.User;
using Models.ApplicationModels;
using Models.Users;
using Moq;
using Services.ApplicationServices.Services.UserAppServices;
using Services.Execptions;
using System.Linq.Expressions;

namespace DVLD_Tests.ApplicationServices.Services.UserTests;

public class UpdateApplicationByUserTEST
{
    private readonly IFixture _fixture;
    private readonly Mock<IMapper> _mapper;

    private readonly Mock<IUpdateRepository<Application>> _updateRepository;
    private readonly Mock<IGetRepository<Application>> _getRepository;

    private readonly IUpdateApplicationByUser _updateApplicationByUser;

    public UpdateApplicationByUserTEST()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();

        _getRepository = new Mock<IGetRepository<Application>>();
        _updateRepository = new Mock<IUpdateRepository<Application>>();

        _updateApplicationByUser = new UpdateApplcationByUserService(_updateRepository.Object, _getRepository.Object, _mapper.Object);
    }


    [Fact]
    public async Task UpdateAsync_UpdateObj_ThrowArgumentNullException()
    {
        //Arrange
        UpdateApplicationByUser updateRequest = null;

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);
        //Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task UpdateAsync_InvaildApplicationId_ThrowsArgumentOutOfRangeException(int id)
    {
        //Arrange
        UpdateApplicationByUser updateRequest = new UpdateApplicationByUser() { Id = id };

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task UpdateAsync_InvaildApplicantUserId_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        UpdateApplicationByUser updateRequest = new UpdateApplicationByUser() { Id = 1 };

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task UpdateAsync_InvaildApplicationTypeId_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        UpdateApplicationByUser updateRequest = new UpdateApplicationByUser() { Id = 1, ApplicantUserId = Guid.NewGuid(), ApplicationTypeId = 0 };

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }


    [Theory]
    [InlineData(null)]
    [InlineData(0)]

    public async Task UpdateAsync_WhenApplicationForIdIsInvaild_ThrowsArgumentOutOfRangeException(short id)
    {
        //Arrange
        UpdateApplicationByUser updateRequest = _fixture.Build<UpdateApplicationByUser>()
                                                .With(app => app.ApplicationTypeId, id)
                                                .Create();

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task UpdateAsync_ApplicationDoesNotExist_ThrowsDoseNotExistException()
    {
        //Arrange
        UpdateApplicationByUser updateRequest = _fixture.Build<UpdateApplicationByUser>()
                                                        .Create();

        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<Application, bool>>>()))
            .ReturnsAsync(null as Application);

        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<DoesNotExistException>();
    }


    [Fact]
    public async Task UpdateAsync_WhenUpdateRepositoryFails_ThrowsFailedToUpdateException()
    {

        //Arrange
        UpdateApplicationByUser updateRequest = _fixture.Build<UpdateApplicationByUser>()
                                                        .Create();


        Application dummyApplication = new Application() { };

        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<Application, bool>>>()))
            .ReturnsAsync(dummyApplication);

        _mapper.Setup(temp => temp.Map<Application>(It.IsAny<UpdateApplicationByUser>()))
                        .Returns(dummyApplication);

        _updateRepository.Setup(temp => temp.UpdateAsync(It.IsAny<Application>()))
                    .ReturnsAsync(null as Application);
        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<FailedToUpdateException>();
    }


    [Fact]
    public async Task UpdateAsync_WhenFailToMappingFromApplicationlToApplicationDTO_ThrowsAutoMapperMappingException()
    {

        //Arrange
        UpdateApplicationByUser updateRequest = _fixture.Build<UpdateApplicationByUser>()
                                                        .Create();

        Application existsApplication = _fixture.Build<Application>()
                                 .With(app => app.ApplicationFees, null as ApplicationFees)
                                 .With(app => app.Employee, null as Employee)
                                 .With(app => app.User, null as Models.Users.User)
                              .Create();

        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<Application, bool>>>()))
            .ReturnsAsync(existsApplication);

        _updateRepository.Setup(temp => temp.UpdateAsync(It.IsAny<Application>()))
            .ReturnsAsync(existsApplication);

        _mapper.Setup(temp => temp.Map<Application>(It.IsAny<UpdateApplicationByUser>()))
                        .Returns(existsApplication);
        //Act
        Func<Task> action = async () => await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        await action.Should().ThrowAsync<AutoMapperMappingException>();
    }



    [Fact]
    public async Task UpdateAsync_WhenSuccessfullyUpdate_ReturnsApplicationDTOForuser()
    {

        //Arrange
        UpdateApplicationByUser updateRequest = _fixture.Build<UpdateApplicationByUser>()
                                                        .Create();

        Application updatedApplication = new Application()
        {
            Id = updateRequest.Id,
            ApplicantUserId = updateRequest.ApplicantUserId,
            ApplicationTypeId = updateRequest.ApplicationTypeId,
            ApplicationForId = updateRequest.ApplicationForId,
        };

        Application dummyApplication = new() { };

        ApplicationDTOForUser applicationDTOForUser = new ApplicationDTOForUser()
        {
            Id = updateRequest.Id,
            ApplicantUserId = updatedApplication.ApplicantUserId,
            ApplicationTypeId = updatedApplication.ApplicationTypeId,
            ApplicationForId = updatedApplication.ApplicationForId,
            LastStatusDate = updatedApplication.LastStatusDate,
            ApplicationStatus = updatedApplication.ApplicationStatus,
            ApplicationDate = updatedApplication.ApplicationDate,
            PaidFees = updatedApplication.PaidFees,
        };

        _getRepository.Setup(temp => temp.GetAsync(It.IsAny<Expression<Func<Application, bool>>>()))
            .ReturnsAsync(dummyApplication);

        _mapper.Setup(temp => temp.Map<Application>(It.IsAny<UpdateApplicationByUser>()))
                        .Returns(dummyApplication);

        _updateRepository.Setup(temp => temp.UpdateAsync(It.IsAny<Application>()))
                    .ReturnsAsync(updatedApplication);

        _mapper.Setup(temp => temp.Map<ApplicationDTOForUser>(It.IsAny<Application>()))
                     .Returns(applicationDTOForUser);

        //Act
        var result = await _updateApplicationByUser.UpdateAsync(updateRequest);

        //Assert
        result.Should().BeEquivalentTo(applicationDTOForUser);
    }
}