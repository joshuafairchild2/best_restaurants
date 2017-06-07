using System.IO;
using System.Collections.Generic;
using Nancy;
using Nancy.Owin;
using Microsoft.AspNet.Builder;
using Nancy.ViewEngines.Razor;

namespace Restaurants
{
  public class Startup
  {
    public void Configure(IApplicationBuilder app)
    {
      app.UseOwin(x => x.UseNancy());
    }
  }
  public class CustomRootPathProvider : IRootPathProvider
  {
    public string GetRootPath()
    {
      return Directory.GetCurrentDirectory();
    }
  }
  public class RazorConfig : IRazorConfiguration
  {
    public IEnumerable<string> GetAssemblyNames()
    {
      return null;
    }
    public IEnumerable<string> GetDefaultNamespaces()
    {
      return null;
    }
    public bool AutoIncludeModelNamespace
    {
      get {return false;}
    }
  }
  public sttaic class DBConfiguration
  {
    public static string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=best_restaurants;Integrated Security=SSPI;";
  }
}
