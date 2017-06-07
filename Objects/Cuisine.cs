using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BestRestaurants;

namespace Restaurants.Objects
{
  public class Cuisine
  {
    private int _id;
    private string _name;

    public Cuisine(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if (!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;

        bool idEquality = this.GetId() == newCuisine.GetId();
        bool nameEquality = this.GetName() == newCuisine.GetName();

        return (idEquality && nameEquality);
      }
    }

    public static List<Cuisine> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Cuisine> allCuisines = new List<Cuisine>{};
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);

        Cuisine newCuisine = new Cuisine(name, id);
        allCuisines.Add(newCuisine);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCuisines;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cuisines (name) OUTPUT INSERTED.id VALUES (@CuisineName);", conn);

      SqlParameter cuisineParam = new SqlParameter("@CuisineName", this.GetName());

      cmd.Parameters.Add(cuisineParam);

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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cuisines;", conn);

      cmd.ExecuteNonQuery();
      if(conn != null)
      {
        conn.Close();
      }
    }

  }
}
