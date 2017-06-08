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
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
      };
      Get["/cuisine/new"] = _ => {
        Dictionary<string, string> model = new Dictionary<string, string>{};
        string formType = Request.Query["form-type"];
        model.Add("form-type", formType);
        return View["form.cshtml", model];
      };
      Post["/"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine-input"]);
        newCuisine.Save();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
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
      Post["/cuisine/{id}/restaurant"] = parameters => {
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
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
      };
      Delete["/cuisine/{id}/delete"] = parameters => {
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        selectedCuisine.DeleteSingleCuisine();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
      };
      Delete["/cuisine/{id}/restaurant"] = parameters => {
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        selectedCuisine.DeleteRestaurants();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
      };
      Get["/cuisine/{id}/update"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        string formType = Request.Query["form-type"];
        model.Add("selected-cuisine", selectedCuisine);
        model.Add("form-type", formType);
        return View["form.cshtml", model];
      };
      Patch["/cuisine/{id}/update"] = parameters => {
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        selectedCuisine.Update(Request.Form["cuisine-input"]);
        List<Cuisine> allCuisines = Cuisine.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>{};
        model.Add("user-name", Request.Form["user-name"]);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", allCuisines];
      };
      Get["/cuisine/{cuisId}/restaurant/{restId}/update"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine selectedCuisine = Cuisine.Find(parameters.cuisId);
        Restaurant selectedRestaurant = Restaurant.Find(parameters.restId);
        string formType = Request.Query["form-type"];
        model.Add("selected-cuisine", selectedCuisine);
        model.Add("form-type", formType);
        model.Add("selected-restaurant", selectedRestaurant);
        return View["form.cshtml", model];
      };
      Patch["/cuisine/{id}/restaurant/{restId}/update"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine selectedCuisine = Cuisine.Find(parameters.id);
        Restaurant selectedRestaurant = Restaurant.Find(parameters.restId);
        selectedRestaurant.Update(Request.Form["restaurant-name-input"], Request.Form["restaurant-stars-input"]);
        List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants();
        model.Add("selected-cuisine", selectedCuisine);
        model.Add("restaurants", cuisineRestaurants);
        return View["show_restaurants", model];
      };
      Get["/cuisine/{cuisId}/restaurant/{restId}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Cuisine selectedCuisine = Cuisine.Find(parameters.cuisId);
        Restaurant selectedRestaurant = Restaurant.Find(parameters.restId);
        List<Client> subscribers = selectedRestaurant.GetSubscribers();
        model.Add("selected-cuisine", selectedCuisine);
        model.Add("restaurant", selectedRestaurant);
        model.Add("subscribers", subscribers);
        return View["restaurant.cshtml", model];
      };
      Post["/cuisine/profile/new"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>{};
        Client newClient = new Client(Request.Form["user-name"]);
        newClient.Save();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        model.Add("user-name", newClient);
        model.Add("cuisines", allCuisines);
        return View["index.cshtml", model];
      };
    }
  }
}
