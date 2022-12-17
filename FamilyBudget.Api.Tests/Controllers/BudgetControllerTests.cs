using AutoFixture;
using FamilyBudget.Api.Controllers;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Tests.Controllers;

public class BudgetControllerTests
{
    private readonly Fixture _fixture;

    public BudgetControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateBudget_ValidInput_ShouldReturnCreatedObject()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;

        var request = _fixture.Build<BudgetInputModel>().Without(x => x.UserIds).Create();
        
        // Act
        var result = await controller.CreateBudget(request);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetViewModel>(valueOfObjectResult);
        var budgetViewModel = valueOfObjectResult as BudgetViewModel;
        
        Assert.NotNull(budgetViewModel!.Name);
        Assert.True(await context.Budgets.AnyAsync(x=>x.Id == budgetViewModel.Id));
    }
    
    [Fact]
    public async Task CreateBudget_InputWithExistingName_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;

        var request = _fixture.Build<BudgetInputModel>().Without(x => x.UserIds).Create();
        request.Name = budget.Name;
        
        // Act
        var result = await controller.CreateBudget(request);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateBudget_ValidInput_ShouldReturnUpdatedBudget()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;

        var request = _fixture.Build<UpdateBudgetInputModel>().Without(x => x.UserIds).Create();
        request.UserIds = new List<Guid>()
        {
            user.Id
        };

        // Act
        var result = await controller.UpdateBudget(budget.Id, request);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetViewModel>(valueOfObjectResult);
        var budgetViewModel = valueOfObjectResult as BudgetViewModel;
        
        Assert.Equal(budgetViewModel!.Name, request.Name);
        Assert.True(await context.Budgets.AnyAsync(x=>x.Name == budgetViewModel.Name));
    }
    
    [Fact]
    public async Task UpdateBudget_InvalidInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);

        var request = _fixture.Build<UpdateBudgetInputModel>()
            .Without(x=>x.Name)
            .Without(x => x.UserIds).Create();
        request.UserIds = new List<Guid>();

        // Act
        controller.ModelState.AddValidationErrors(request);
        var result = await controller.UpdateBudget(Guid.NewGuid(), request);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateBudget_NoExistBudgetInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);

        var request = _fixture.Build<UpdateBudgetInputModel>()
            .Without(x => x.UserIds).Create();
        request.UserIds = new List<Guid>();

        // Act
        var result = await controller.UpdateBudget(Guid.NewGuid(), request);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetBudget_ValidInput_ShouldReturnBudgetObject()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var controller = new BudgetsController(context, mapper);

        // Act
        var result = await controller.GetBudget(budget.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetViewModel>(valueOfObjectResult);
        var budgetViewModel = valueOfObjectResult as BudgetViewModel;
        
        Assert.Equal(budgetViewModel!.Id, budget.Id);
    }
    
    [Fact]
    public async Task GetBudget_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);

        // Act
        var result = await controller.GetBudget(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetBudgets_EmptyRequest_ShouldReturnAllObjects()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var request = new FilterInputModel();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;

        // Act
        var result = await controller.GetBudgets(request);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BaseListViewModel<BudgetViewModel>>(valueOfObjectResult);
        var baseListViewModel = valueOfObjectResult as BaseListViewModel<BudgetViewModel>;
        
        Assert.Equal(baseListViewModel!.Length, context.Budgets.Count());
    }
    
    [Fact]
    public async Task DeleteBudget_ValidInput_ShouldReturnDeletedObject()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        
        // Act
        var result = await controller.DeleteBudget(budget.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetViewModel>(valueOfObjectResult);
        var budgetViewModel = valueOfObjectResult as BudgetViewModel;

        Assert.Equal(budgetViewModel!.Id, budget.Id);
        Assert.False(await context.Budgets.AnyAsync(x=>x.Id == budget.Id));
    }
    
    [Fact]
    public async Task DeleteBudget_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);

        // Act
        var result = await controller.DeleteBudget(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteBudget_ValidList_ShouldReturnListOfDeletedObjects()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var user = await helper.CreateUser();
        var budget1 = await helper.CreateBudget();
        var budget2 = await helper.CreateBudget();
        var idList = new List<Guid> { budget1.Id, budget2.Id };

        // Act
        var result = await controller.DeleteBudget(idList);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<List<BudgetViewModel>>(valueOfObjectResult);
        var budgetViewModelList = valueOfObjectResult as List<BudgetViewModel>;
        
        Assert.True(budgetViewModelList!.All(x=>idList.Contains(x.Id)));
    }
    
    [Fact]
    public async Task DeleteBudget_InvalidList_ShouldReturnBadRequestd()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetsController(context, mapper);
        var list = _fixture.Create<List<Guid>>();
        
        // Act
        var result = await controller.DeleteBudget(list);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}