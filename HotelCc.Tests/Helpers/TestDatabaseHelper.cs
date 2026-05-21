using Microsoft.EntityFrameworkCore;
using HotelCc.Data;
using System;

namespace HotelCc.Tests.Helpers
{
    public static class TestDatabaseHelper
    {
        public static AppDbContext CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
