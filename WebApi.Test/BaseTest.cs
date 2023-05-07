using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test
{
  public abstract class BaseTest
  {
    public const string _mongoDbConnectionString = "Data source=mongodb://localhost:27017;Initial catalog=Test;";
  }
}
