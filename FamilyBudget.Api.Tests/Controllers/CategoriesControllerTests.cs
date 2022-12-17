using AutoFixture;
using FamilyBudget.Api.Controllers;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;
using FamilyBudget.Tests.Common;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudget.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Fixture _fixture;

    public CategoriesControllerTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateCategory_ValidInput_ShouldReturnCategoryViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var input = _fixture.Create<CategoryInputModel>();
        
        // Act
        var result = await controller.CreateCategory(input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<CategoryViewModel>(valueOfObjectResult);
        
        var categoryViewModel = valueOfObjectResult as CategoryViewModel;
        Assert.NotNull(categoryViewModel!.Id);
    }
    
    [Fact]
    public async Task CreateCategory_AlreadyExistInput_ShouldReturnBadRequest()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var category = await helper.CreateCategory();
        var input = new CategoryInputModel(category.Name);

        // Act
        var result = await controller.CreateCategory(input);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateCategory_ValidInput_ShouldReturnUpdatedCategoryViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var category = await helper.CreateCategory();
        var input = _fixture.Create<CategoryInputModel>();

        // Act
        var result = await controller.UpdateCategory(category.Id, input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<CategoryViewModel>(valueOfObjectResult);
        
        var categoryViewModel = valueOfObjectResult as CategoryViewModel;
        Assert.Equal(input.Name, categoryViewModel!.Name);
    }
    
    [Fact]
    public async Task UpdateCategory_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var input = _fixture.Create<CategoryInputModel>();

        // Act
        var result = await controller.UpdateCategory(Guid.NewGuid(), input);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetCategory_ValidInput_ShouldReturnCategoryViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var category = await helper.CreateCategory();

        // Act
        var result = await controller.GetCategory(category.Id);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<CategoryViewModel>(valueOfObjectResult);
        
        var categoryViewModel = valueOfObjectResult as CategoryViewModel;
        Assert.Equal(category.Name, categoryViewModel!.Name);
    }
    
    [Fact]
    public async Task GetCategory_InvalidInput_ShouldReturnNotFound()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var input = _fixture.Create<CategoryInputModel>();

        // Act
        var result = await controller.GetCategory(Guid.NewGuid());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetCategories_ValidInput_ShouldReturnBaseListViewModel()
    {
        // Arrange
        var helper = new TestHelper();
        var context = helper.GetContext();
        var mapper = helper.GetMapper();
        var controller = new CategoriesController(context, mapper);
        var category = await helper.CreateCategory();
        var input = new FilterInputModel();
        
        // Act
        var result = await controller.GetCategories(input);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
        var valueOfObjectResult = (result as OkObjectResult)!.Value;
        
        Assert.IsType<BaseListViewModel<CategoryViewModel>>(valueOfObjectResult);
        
        var baseListViewModel = valueOfObjectResult as BaseListViewModel<CategoryViewModel>;
        Assert.Equal(1, baseListViewModel!.Length);
    }
}