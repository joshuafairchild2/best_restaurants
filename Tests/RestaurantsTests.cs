using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace BestRestaurants
{
  public class RestaurantsTest : IDisposable
  {
    public RestaurantsTest()
    {
       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void TestRestaurant_DatabaseEmptyAtFirst()
    {
      //Arrange
      List<Restaurant> allRestaurants = new List<Restaurant>{};

      //Act
      List<Restaurant> testList = Restaurant.GetAll();

      //Assert
      Assert.Equal(allRestaurants, testList);
    }

    [Fact]
    public void TestRestaurant_Equal_ReturnEqualValues()
    {
      //Arrange
      Restaurant newRestaurant = new Restaurant("Lucia", 3, 1);
      Restaurant testRestaurant = new Restaurant("Lucia", 3, 1);

      //Act, Assert
      Assert.Equal(newRestaurant, testRestaurant);
    }

    [Fact]
    public void Test_Save_SavesRestaurantToDatabase()
    {
      //arrange
      Restaurant newRestaurant = new Restaurant("Lucia", 3, 1);
      newRestaurant.Save();

      //Act
      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      //assert
      Assert.Equal(newRestaurant, savedRestaurant);
    }

    [Fact]
    public void Test_Find_FindsRestaurantInDatabase()
    {
      //arrange
      // int id = 1;
      Restaurant newRestaurant = new Restaurant("Lucia", 3, 1);
      newRestaurant.Save();

      //Act
      Restaurant foundRestaurant = Restaurant.Find(newRestaurant.GetId());

      //assert
      Assert.Equal(newRestaurant, foundRestaurant);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
    }
  }
}
