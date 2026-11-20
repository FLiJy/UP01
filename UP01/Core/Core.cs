using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UP01;

namespace UP01.Core
{
    public class Core
{
    public static AutoServiceDBEntities Context =
        new AutoServiceDBEntities();

    public static users CurrentUSer { get; set; }

    public static bool TestConnection()
    {
        try
        {
            return Context.Database.Exists()
                && Context.users.Any();
        }
        catch (Exception)
        {
            return false;
        }
    }
}
}