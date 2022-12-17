using AutoFixture;
using FamilyBudget.Api.Controllers;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Tests.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Tests.Controllers;

public class AuthenticationControllerTests
{
    private readonly Fixture _fixture;

    public AuthenticationControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Login_FirstLogin_ShouldCreateAnAccountAndReturnToken()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var utils = helper.GetJwtUtils();
        var mapper = helper.GetMapper();
        var cache = helper.GetCache();
        var controller = new AuthenticationController(context, utils, mapper, cache);
        var inputModel = _fixture.Create<UserInputModel>();
        
        // Act
        var result = await controller.Login(inputModel);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<JwtTokenViewModel>(valueOfObjectResult);
        var tokenViewModel = valueOfObjectResult as JwtTokenViewModel;
        
        Assert.NotNull(tokenViewModel!.Token);
    }

    [Fact]
    public async Task Login_WrongCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var utils = helper.GetJwtUtils();
        var mapper = helper.GetMapper();
        var cache = helper.GetCache();
        var controller = new AuthenticationController(context, utils, mapper, cache);
        
        var userInDatabase = _fixture.Build<User>().Without(x=>x.BudgetUsers).Create();
        await context.Users.AddAsync(userInDatabase);
        await context.SaveChangesAsync();

        var inputModel = _fixture.Create<UserInputModel>();
        
        // Act
        var result = await controller.Login(inputModel);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_EmptyPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var utils = helper.GetJwtUtils();
        var mapper = helper.GetMapper();
        var cache = helper.GetCache();
        var controller = new AuthenticationController(context, utils, mapper, cache);
        var inputModel = new UserInputModel();
        
        // Act
        controller.ModelState.AddValidationErrors(inputModel);
        var result = await controller.Login(inputModel);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}