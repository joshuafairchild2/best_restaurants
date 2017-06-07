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

    [Fact]
    public void TestCuisine_GetRestaurants_GetsRestaurantsByCuisine()
    {
      Cuisine newCuisine = new Cuisine("Chinese");
      newCuisine.Save();

      Restaurant rest1 = new Restaurant("Tender Green", 3, newCuisine.GetId());
      rest1.Save();
      Restaurant rest2 = new Restaurant("Golden China", 4, newCuisine.GetId());
      rest2.Save();


      List<Restaurant> controlList = new List<Restaurant>{rest1, rest2};
      List<Restaurant> testList = newCuisine.GetRestaurants();

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void TestCuisineDelete_DeletesSingleCuisine()
    {
      Cuisine newCuisine1 = new Cuisine("Chinese");
      newCuisine1.Save();
      Cuisine newCuisine2 = new Cuisine("Thai");
      newCuisine2.Save();

      newCuisine1.DeleteSingleCuisine();

      List<Cuisine> controlList = new List<Cuisine>{newCuisine2};
      List<Cuisine> testList = Cuisine.GetAll();

      Assert.Equal(controlList, testList);
    }

    public void Dispose()
    {
      Cuisine.DeleteAll();
      Restaurant.DeleteAll();
    }
  }
}
