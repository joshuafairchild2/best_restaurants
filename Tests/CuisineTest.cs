using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace BestRestaurants
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void TestCuisine_DatabaseEmptyAtFirst()
    {
      //Arrange
      List<Cuisine> allCuisines = new List<Cuisine>{};

      //Act
      List<Cuisine> testList = Cuisine.GetAll();

      //Assert
      Assert.Equal(allCuisines, testList);
    }

    [Fact]
    public void TestCuisine_Equal_ReturnEqualValues()
    {
      //Arrange
      Cuisine newCuisine = new Cuisine("Chinese");
      Cuisine testCuisine = new Cuisine("Chinese");

      //Act, Assert
      Assert.Equal(newCuisine, testCuisine);
    }

    public void Dispose()
    {
      // Cuisine.DeleteAll();
    }
  }
}
