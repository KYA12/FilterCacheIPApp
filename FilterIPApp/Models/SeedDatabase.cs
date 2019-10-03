using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterIPApp.Models
{
    /* Seed created database with initial values*/
    public class SeedDatabase
    {
        public static void Initialize(IPTableContext context)
        {
            if (!context.IPTables.Any())
            {
                context.IPTables.AddRange(
                    new IPTable
                    {
                        Name = "Admin",
                        IPStart = "127.0.0.1",
                        IPEnd = "127.0.0.1"
                    },

                    new IPTable
                    {
                        Name = "Client",
                        IPStart = "::1",
                        IPEnd = "::1"
                    },

                    new IPTable
                    {
                        Name = "Client",
                        IPStart = "192.168.0.1",
                        IPEnd = "192.168.0.78"
                    }

                ); 
                context.SaveChanges();
            }
        }
    }
}
