using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Restaurants.Objects;

namespace BestRestaurants
{
  [Collection("BestRestaurants")]

  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void TestClient_DatabaseEmptyAtFirst()
    {
      List<Client> allClient = new List<Client>{};
      List<Client> testList = Client.GetAll();
      Assert.Equal(allClient, testList);
    }

    [Fact]
    public void TestClient_Equal_ReturnEqualValues()
    {
      //Arrange
      Client newClient = new Client("Sam");
      Client testClient = new Client("Sam");

      //Act, Assert
      Assert.Equal(newClient, testClient);
    }

    [Fact]
    public void TestClient_Save_SavesClientToDatabase()
    {
      //arrange
      Client newClient = new Client("Sam");
      newClient.Save();

      //Act
      Client savedClient = Client.GetAll()[0];

      //assert
      Assert.Equal(newClient, savedClient);
    }

    [Fact]
    public void TestClient_Find_FindsClientInDatabase()
    {
      //arrange
      Client newClient = new Client("Sam");
      newClient.Save();

      //Act
      Client foundClient = Client.Find(newClient.GetId());

      //assert
      Assert.Equal(newClient, foundClient);
    }

    [Fact]
    public void TestClient_GetSubscriptions_ReturnsListOfSubscriptions()
    {
      Client newClient = new Client("Sam");
      newClient.Save();
      Restaurant newRestaurant1 = new Restaurant("Tender Green", 3, 1);
      newRestaurant1.Save();
      Restaurant newRestaurant2 = new Restaurant("Golden CHina", 2, 1);
      newRestaurant2.Save();

      newClient.SubscribeToRestaurant(newRestaurant1);
      newClient.SubscribeToRestaurant(newRestaurant2);

      List<Restaurant> controlList = new List<Restaurant> {newRestaurant1, newRestaurant2};
      List<Restaurant> subscriptions = newClient.GetSubscriptions();

      Assert.Equal(controlList, subscriptions);
    }

    [Fact]
    public void TestClient_SubscribeToRestaurant_SavesRelationshipToDatabase()
    {
      Client newClient = new Client("Sam");
      newClient.Save();
      Restaurant newRestaurant = new Restaurant("Tender Green", 3, 1);
      newRestaurant.Save();

      newClient.SubscribeToRestaurant(newRestaurant);

      Restaurant subscribedRestaurant = newClient.GetSubscriptions()[0];
      Assert.Equal(newRestaurant, subscribedRestaurant);
    }

    [Fact]
    public void TestClient_SearchByName_ReturnsMatches()
    {
      Client client1 = new Client("Ed");
      client1.Save();
      Client client2 = new Client("Edd");
      client2.Save();
      Client client3 = new Client("Eddy");
      client3.Save();
      Client client4 = new Client("edd");
      client4.Save();

      List<Client> controlList = new List<Client>{client2, client4};
      List<Client> matches = Client.SearchByName("edd");

      Assert.Equal(controlList, matches);
    }

    public void Dispose()
    {
      Client.DeleteAll();
    }
  }
}
