using SharedModels.Enums;
using Wallets.Application.Helpers;

namespace Wallets.Application.Tests.Helpers;

public class CommissionCalculatorTests
{
    [Theory]
    [InlineData(1, 1000, 10)]
    [InlineData(2, 1000, 20)]
    [InlineData(3, 1000, 30)]
    [InlineData(10, 1000, 100)]
    public void Calculate_LinearSchema_ReturnsLevelTimesProfitOver100(int level, decimal profit, decimal expected)
    {
        //Arrange
        //Act
        var actual = CommissionCalculator.Calculate(SchemaType.Linear, level, profit);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 1000, 10)]
    [InlineData(2, 1000, 10)]
    [InlineData(3, 1000, 20)]
    [InlineData(4, 1000, 30)]
    [InlineData(5, 1000, 50)]
    [InlineData(6, 1000, 80)]
    public void Calculate_FibonacciSchema_ReturnsFibonacciFactorTimesProfitOver100(int level, decimal profit, decimal expected)
    {
        //Arrange
        //Act
        var actual = CommissionCalculator.Calculate(SchemaType.Fibonacci, level, profit);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    [InlineData(7, 13)]
    public void Fibonacci_ValidLevel_ReturnsExpectedSequenceValue(int level, int expected)
    {
        //Arrange
        //Act
        var actual = CommissionCalculator.Fibonacci(level);

        //Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Calculate_LevelBelowOne_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        //Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CommissionCalculator.Calculate(SchemaType.Linear, 0, 1000m));

        //Assert
        Assert.Equal("level", exception.ParamName);
    }

    [Fact]
    public void Fibonacci_LevelBelowOne_ThrowsArgumentOutOfRangeException()
    {
        //Arrange
        //Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CommissionCalculator.Fibonacci(0));

        //Assert
        Assert.Equal("level", exception.ParamName);
    }

    [Fact]
    public void Calculate_RoundsAwayFromZero_ToFourDecimalPlaces()
    {
        //Arrange
        // 1 * 10.55555 / 100 = 0.1055555 -> 0.1056
        //Act
        var actual = CommissionCalculator.Calculate(SchemaType.Linear, 1, 10.55555m);

        //Assert
        Assert.Equal(0.1056m, actual);
    }
}
