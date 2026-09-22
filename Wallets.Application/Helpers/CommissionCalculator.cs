using SharedModels.Enums;

namespace Wallets.Application.Helpers;

public static class CommissionCalculator
{
    public static decimal Calculate(SchemaType schemaType, int level, decimal profit)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        var factor = schemaType switch
        {
            SchemaType.Linear => level,
            SchemaType.Fibonacci => Fibonacci(level),
            _ => throw new ArgumentOutOfRangeException(nameof(schemaType)),
        };

        return Math.Round(factor * profit / 100m, 4, MidpointRounding.AwayFromZero);
    }

    public static int Fibonacci(int level)
    {
        if (level < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }

        if (level <= 2)
        {
            return 1;
        }

        var previous = 1;
        var current = 1;
        for (var i = 3; i <= level; i++)
        {
            var next = previous + current;
            previous = current;
            current = next;
        }

        return current;
    }
}
