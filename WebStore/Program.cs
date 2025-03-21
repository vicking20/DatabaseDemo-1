
using Microsoft.EntityFrameworkCore;
using WebStore.Data;

var optionsBuilder = new DbContextOptionsBuilder<WebStoreContext>();
optionsBuilder.UseNpgsql(
    "Host=localhost;Database=WebStore;Username=postgres;Password=mypassword"
);


using var context = new WebStoreContext(optionsBuilder.Options);



var orders = context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToList();

foreach (var order in orders)
{
    System.Console.WriteLine(
        $"Order {order.OrderId} by {order.Customer?.FirstName} " +
        $"{order.Customer?.LastName}, items: {order.OrderItems?.Count}"
    );
}

System.Console.WriteLine("Press any key to exit...");
System.Console.ReadKey();
