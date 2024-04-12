using Northwind.EntityModels;

namespace Northwind.UnitTests;

public class EntityModelTests
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = new NorthwindContext();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void CategoryCountTest()
    {
        using NorthwindContext db = new();

        int expected = 8;
        int actual;

        actual = db.Categories.Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ProductId1IsChaiTest()
    {
        using NorthwindContext db = new();

        string expected = "Chai";
        string actual;

        Product? product = db.Products.Find(keyValues: 1);

        actual = product?.ProductName ?? string.Empty;

        Assert.Equal(expected, actual);
    }

}