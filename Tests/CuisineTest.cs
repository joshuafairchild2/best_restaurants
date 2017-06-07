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

    [Fact]
    public void TestCuisine_Save_SavesCuisineToDatabase()
    {
      //arrange
      Cuisine newCuisine = new Cuisine("Chinese");
      newCuisine.Save();

      //Act
      Cuisine savedCuisine = Cuisine.GetAll()[0];

      //assert
      Assert.Equal(newCuisine, savedCuisine);
    }

    [Fact]
    public void TestCuisine_Find_FindsCuisineInDatabase()
    {
      //arrange
      Cuisine newCuisine = new Cuisine("Chinese");
      newCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(newCuisine.GetId());

      //assert
      Assert.Equal(newCuisine, foundCuisine);
    }

    public void Dispose()
    {
      Cuisine.DeleteAll();
    }
  }
}
