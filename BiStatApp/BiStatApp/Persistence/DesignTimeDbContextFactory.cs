using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore.Design;

namespace BiStatApp.Persistence
{
    class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BiStatContext>
    {
        public BiStatContext CreateDbContext(string[] args)
        {
            Debug.WriteLine(Directory.GetCurrentDirectory() + @"\BiStatApp.db3");

            return new BiStatContext(Directory.GetCurrentDirectory() + @"\BiStatApp.db3");
        }
    }
}
