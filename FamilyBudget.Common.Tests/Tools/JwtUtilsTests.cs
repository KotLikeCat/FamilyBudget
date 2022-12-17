using AutoFixture;
using FamilyBudget.Tests.Common;

namespace FamilyBudget.Common.Tests.Tools;

public class JwtUtilsTests
{
    private readonly IFixture _fixture;

    public JwtUtilsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ValidateJwtToken_ValidInput_ShouldReturnUserId()
    {
        // Arrange
        var helper = new TestHelper();
        var jwtUtils = helper.GetJwtUtils();
        var user = await helper.CreateUser();
        var token = jwtUtils.GenerateJwtToken(user);
        
        // Act
        var result = jwtUtils.ValidateJwtToken(token);
        
        // Assert
        Assert.Equal(user.Id, result);
    }
    
    [Fact]
    public void ValidateJwtToken_NullInput_ShouldReturnNull()
    {
        // Arrange
        var helper = new TestHelper();
        var jwtUtils = helper.GetJwtUtils();

        // Act
        var result = jwtUtils.ValidateJwtToken(null);
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void ValidateJwtToken_WrongInput_ShouldReturnNull()
    {
        // Arrange
        var helper = new TestHelper();
        var jwtUtils = helper.GetJwtUtils();
        var token = _fixture.Create<string>();
        
        // Act
        var result = jwtUtils.ValidateJwtToken(token);
        
        // Assert
        Assert.Null(result);
    }
}