using AutoFixture;
using FamilyBudget.Api.Controllers;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Fixture _fixture;

    public UsersControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateUser_ValidInput_ShouldReturnUserViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var input = _fixture.Create<UserInputModel>();
        
        // Act
        var result = await controller.CreateUser(input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<UserViewModel>(valueOfObjectResult);
        
        var userViewModel = valueOfObjectResult as UserViewModel;
        Assert.NotNull(userViewModel?.Id);
        Assert.True(await context.Users.AnyAsync(x=>x.Id == userViewModel.Id));
    }
    
    [Fact]
    public async Task CreateUser_InvalidInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var input = new UserInputModel();
        
        // Act
        controller.ModelState.AddValidationErrors(input);
        var result = await controller.CreateUser(input);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_AlreadyExistInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var user = await helper.CreateUser();
        var input = new UserInputModel
        {
            Login = user.Login,
            Password = _fixture.Create<string>()
        };
        
        // Act
        controller.ModelState.AddValidationErrors(input);
        var result = await controller.CreateUser(input);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateUser_InvalidInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var input = new UpdateUserInputModel();
        
        // Act
        controller.ModelState.AddValidationErrors(input);
        var result = await controller.UpdateUser(Guid.NewGuid(), input);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateUser_WrongUserInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var input = _fixture.Create<UpdateUserInputModel>();
        
        // Act
        controller.ModelState.AddValidationErrors(input);
        var result = await controller.UpdateUser(Guid.NewGuid(), input);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task UpdateUser_ValidInput_ShouldReturnUpdatedUserViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var input = _fixture.Create<UpdateUserInputModel>();
        var user = await helper.CreateUser();
        
        // Act
        var result = await controller.UpdateUser(user.Id, input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<UserViewModel>(valueOfObjectResult);
    }
    [Fact]
    
    public async Task DeleteUser_ValidInput_ShouldReturnDeletedObject()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var user = await helper.CreateUser();
        context.Entry(user).State = EntityState.Detached;
        
        // Act
        var result = await controller.DeleteUser(user.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<UserViewModel>(valueOfObjectResult);
        var userViewModel = valueOfObjectResult as UserViewModel;

        Assert.Equal(user.Id, userViewModel!.Id);
        Assert.False(await context.Users.AnyAsync(x=>x.Id == user.Id));
    }
    
    [Fact]
    public async Task DeleteUser_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);

        // Act
        var result = await controller.DeleteUser(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteUsers_ValidList_ShouldReturnListOfDeletedObjects()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var user1 = await helper.CreateUser();
        var user2 = await helper.CreateUser();

        context.Entry(user1).State = EntityState.Detached;
        context.Entry(user2).State = EntityState.Detached;
        
        var idList = new List<Guid> { user1.Id, user2.Id };

        // Act
        var result = await controller.DeleteUsers(idList);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<List<UserViewModel>>(valueOfObjectResult);
        var userViewModels = valueOfObjectResult as List<UserViewModel>;
        
        Assert.True(userViewModels!.All(x=>idList.Contains(x.Id)));
    }
    
    [Fact]
    public async Task DeleteUsers_InvalidList_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var list = _fixture.Create<List<Guid>>();
        
        // Act
        var result = await controller.DeleteUsers(list);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task GetUser_ValidInput_ShouldReturnUserViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var user = await helper.CreateUser();

        // Act
        var result = await controller.GetUser(user.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<UserViewModel>(valueOfObjectResult);
        
        var userViewModel = valueOfObjectResult as UserViewModel;
        Assert.Equal(user.Login, userViewModel!.Login);
    }
    
    [Fact]
    public async Task GetUser_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);

        // Act
        var result = await controller.GetUser(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetUsers_ValidInput_ShouldReturnBaseListViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new UsersController(context, mapper);
        var user = await helper.CreateUser();
        var input = new FilterInputModel();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;
        
        // Act
        var result = await controller.GetUsers(input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BaseListViewModel<UserViewModel>>(valueOfObjectResult);
        
        var baseListViewModel = valueOfObjectResult as BaseListViewModel<UserViewModel>;
        Assert.Equal(1, baseListViewModel!.Length);
    }
}