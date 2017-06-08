using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace BestRestaurants
{
  [Collection("BestRestaurants")]

  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
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

    [Fact]
    public void TestRestaurant_Update_UpdateRastaurantInfo()
    {
      Cuisine testCuisine = new Cuisine("Ukrainian");
      testCuisine.Save();

      Restaurant restaurant = new Restaurant("Tender Green", 3, testCuisine.GetId());
      restaurant.Save();

      restaurant.Update("Tender Blue", 4);

      Assert.Equal("Tender Blue", restaurant.GetName());
      Assert.Equal(4, restaurant.GetStars());
    }

    public void TestRestaurant_GetSubscribers_ReturnsListOfSubscribers()
    {
      Restaurant newRestaurant = new Restaurant("Tender Green", 3, 1);
      Client newClient1 = new Client("Sam");
      Client newClient2 = new Client("Tom");
      newRestaurant.Save();
      newClient1.Save();
      newClient2.Save();

      newClient1.SubscribeToRestaurant(newRestaurant);
      newClient2.SubscribeToRestaurant(newRestaurant);

      List<Client> controlList = new List<Client>{newClient1, newClient2};
      List<Client> testList = newRestaurant.GetClients();

      Assert.Equal(controlList, testList);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
    }
  }
}
