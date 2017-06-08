using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BestRestaurants;

namespace Restaurants.Objects
{
  public class Client
  {
    private int _id;
    private string _name;

    public Client(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int id)
    {
      _id = id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string name)
    {
      _name = name;
    }


    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
        return false;
      }
      else
      {
        Client newClient = (Client) otherClient;

        bool idEquality = this.GetId() == newClient.GetId();
        bool nameEquality = this.GetName() == newClient.GetName();

        return (idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM clients;", conn);

      cmd.ExecuteNonQuery();
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static List<Client> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Client> allClients = new List<Client>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Client newClient = new Client(name, id);
        allClients.Add(newClient);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allClients;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO clients (name) OUTPUT INSERTED.id VALUES (@ClientName);", conn);

      SqlParameter nameParam = new SqlParameter("@ClientName", this.GetName());

      cmd.Parameters.Add(nameParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Client Find(int idToFind)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE id = @ClientId;", conn);
      SqlParameter idParam = new SqlParameter("@ClientId", idToFind);
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();


      int id =  0;
      string name = null;

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Client foundClient = new Client(name, id);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundClient;
    }

    public void SubscribeToRestaurant(Restaurant subscription)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO clients_restaurants (client_id, restaurant_id) VALUES (@ClientId, @RestaurantId);", conn);
      SqlParameter clientIdParam = new SqlParameter("@ClientId", this.GetId());
      SqlParameter restaurantIdParam = new SqlParameter("@RestaurantId", subscription.GetId());
      cmd.Parameters.Add(clientIdParam);
      cmd.Parameters.Add(restaurantIdParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Restaurant> GetSubscriptions()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT restaurants.* FROM clients JOIN clients_restaurants ON (clients.id = clients_restaurants.client_id) JOIN restaurants ON (clients_restaurants.restaurant_id = restaurants.id) WHERE clients.id = @ClientId;", conn);
      SqlParameter clientIdParam = new SqlParameter("@ClientId", this.GetId());
      cmd.Parameters.Add(clientIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Restaurant> subscriptions = new List<Restaurant>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int stars = rdr.GetInt32(2);
        int cuisineId = rdr.GetInt32(3);
        Restaurant newRestaurant = new Restaurant(name, stars, cuisineId, id);
        subscriptions.Add(newRestaurant);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return subscriptions;
    }

    public static List<Client> SearchByName(string nameToSearch)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE name = @SearchName;", conn);
      SqlParameter searchParam = new SqlParameter("@SearchName", nameToSearch);
      cmd.Parameters.Add(searchParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Client> matches = new List<Client>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Client newClient = new Client(name, id);
        matches.Add(newClient);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return matches;
    }
  }
}
