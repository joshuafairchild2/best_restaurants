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

    public void Dispose()
    {
      Client.DeleteAll();
    }
  }
}
