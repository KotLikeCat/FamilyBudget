using AutoFixture;
using FamilyBudget.Api.Controllers;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Tests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudget.Api.Tests.Controllers;

public class BudgetDetailsControllerTests
{
    private readonly Fixture _fixture;

    public BudgetDetailsControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateBudgetDetail_ValidInput_ShouldReturnCreatedObject()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.ControllerContext.HttpContext.Items["User"] = user;

        var inputModel = _fixture.Create<BudgetDetailInputModel>();
        inputModel.CategoryId = category.Id;
        
        // Act
        var result = await controller.CreateBudgetDetail(budget.Id, inputModel);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetDetailViewModel>(valueOfObjectResult);
        var budgetDetailView = valueOfObjectResult as BudgetDetailViewModel;
        
        Assert.NotNull(budgetDetailView!.Category);
        Assert.True(await context.BudgetDetails.AnyAsync(x=>x.Id == budgetDetailView.Id));
    }
    
    [Fact]
    public async Task CreateBudgetDetail_InvalidBudgetId_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        
        var inputModel = _fixture.Create<BudgetDetailInputModel>();
        
        // Act
        var result = await controller.CreateBudgetDetail(budget.Id, inputModel);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateBudgetDetail_InvalidCategoryId_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        
        var inputModel = _fixture.Create<BudgetDetailInputModel>();
        
        // Act
        var result = await controller.CreateBudgetDetail(Guid.NewGuid(), inputModel);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetBudgetDetail_ValidInput_ShouldReturnBudgetDetailViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        var budgetDetail = await helper.CreateBudgetDetail();
        
        // Act
        var result = await controller.GetBudgetDetail(budget.Id, budgetDetail.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetDetailViewModel>(valueOfObjectResult);
        var budgetDetailView = valueOfObjectResult as BudgetDetailViewModel;
        
        Assert.NotNull(budgetDetailView!.Category);
    }
    
    [Fact]
    public async Task GetBudgetDetail_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);

        // Act
        var result = await controller.GetBudgetDetail(Guid.NewGuid(), Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetBudgetDetails_ValidInput_ShouldReturnBaseListViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        var budgetDetail = await helper.CreateBudgetDetail();
        var filterInput = new FilterInputModel();
        
        // Act
        var result = await controller.GetBudgetDetails(budget.Id, filterInput);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BaseListViewModel<BudgetDetailViewModel>>(valueOfObjectResult);
        var budgetDetailView = valueOfObjectResult as BaseListViewModel<BudgetDetailViewModel>;
        
        Assert.Equal(1, budgetDetailView!.Length);
    }
    
    [Fact]
    public async Task UpdateBudgetDetail_ValidInput_ReturnBudgetDetailViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        var budgetDetail = await helper.CreateBudgetDetail();
        var request = _fixture.Create<BudgetDetailInputModel>();
        request.CategoryId = category.Id;
        
        // Act
        var result = await controller.UpdateBudgetDetail(budget.Id, budgetDetail.Id, request);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetDetailViewModel>(valueOfObjectResult);
        var budgetDetailView = valueOfObjectResult as BudgetDetailViewModel;
        
        Assert.Equal(request.Description, budgetDetailView!.Description);
    }
    
    [Fact]
    public async Task UpdateBudgetDetail_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var request = _fixture.Create<BudgetDetailInputModel>();
        
        // Act
        var result = await controller.UpdateBudgetDetail(Guid.NewGuid(), Guid.NewGuid(), request);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteBudgetDetail_ValidInput_ReturnBudgetDetailViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        var budgetDetail = await helper.CreateBudgetDetail();

        // Act
        var result = await controller.DeleteBudgetDetail(budget.Id, budgetDetail.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BudgetDetailViewModel>(valueOfObjectResult);
        var budgetDetailView = valueOfObjectResult as BudgetDetailViewModel;
        
        Assert.Equal(budgetDetail.Id, budgetDetailView!.Id);
        Assert.False(await context.BudgetDetails.AnyAsync(x=>x.Id == budgetDetail.Id));
    }
    
    [Fact]
    public async Task DeleteBudgetDetail_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);

        // Act
        var result = await controller.DeleteBudgetDetail(Guid.NewGuid(), Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteBudgetDetails_ValidInput_ReturnBudgetDetailViewModelList()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var user = await helper.CreateUser();
        var budget = await helper.CreateBudget();
        var category = await helper.CreateCategory();
        var budgetDetail = await helper.CreateBudgetDetail();
        var list = new List<Guid> { budgetDetail.Id };

        // Act
        var result = await controller.DeleteBudgetDetails(budget.Id, list);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<List<BudgetDetailViewModel>>(valueOfObjectResult);
        var budgetDetailViewList = valueOfObjectResult as List<BudgetDetailViewModel>;
        
        Assert.Equal(list.Count, budgetDetailViewList!.Count);
    }
    
    [Fact]
    public async Task DeleteBudgetDetails_InvalidInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new BudgetDetailsController(context, mapper);
        var list = _fixture.Create<List<Guid>>();

        // Act
        var result = await controller.DeleteBudgetDetails(Guid.NewGuid(), list);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}