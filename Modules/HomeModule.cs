using Nancy;
using Restaurants.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace BestRestaurants
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };
      Get["/cuisine/new"] = _ => {
        Dictionary<string, string> model = new Dictionary<string, string>{};
        string formType = Request.Query["form-type"];
        Console.WriteLine(formType);
        model.Add("form-type", formType);
        return View["form.cshtml", model];
      };
      Post["/"] = _ => {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-input"]);
        newCuisine.Save();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };
      Get["/cuisine/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        List<Restaurant> restaurantsByCuisine = selectedCuisine.GetRestaurants();
        model.Add("restaurants", restaurantsByCuisine);
        model.Add("selected-cuisine", selectedCuisine);
        return View["show_restaurants.cshtml", model];
      };
      Get["/cuisine/{id}/restaurant/new"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        string formType = Request.Query["form-type"];
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        model.Add("form-type", formType);
        model.Add("selected-cuisine", selectedCuisine);
        return View["form.cshtml", model];
      };
      Post["/cuisine/{id}/restaurants"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name-input"], Request.Form["restaurant-stars-input"], parameters.id);
        newRestaurant.Save();
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        List<Restaurant> allCuisineRestaurants = selectedCuisine.GetRestaurants();
        model.Add("restaurants", allCuisineRestaurants);
        model.Add("selected-cuisine", selectedCuisine);
        return View["show_restaurants.cshtml", model];
      };
      Delete["/"] = _ => {
        Cuisine.DeleteAll();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View["index.cshtml", allCuisines];
      };
    }
  }
}
