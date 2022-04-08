using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.EntityFramwork
{
    public static class Database
    {

        public static DbContextOptions GetOptions()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(Constants.SportsConnectionString);

            return builder.Options;
        }
    }
}
