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
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange
      List<Restaurant> allRestaurants = new List<Restaurant>{};

      //Act
      List<Restaurant> testList = Restaurant.GetAll();

      //Assert
      Assert.Equal(allRestaurants, testList);
    }

    public void Dispose()
    {
      
    }
  }
}
