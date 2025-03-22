using WebStore.Data;

namespace WebStore.Console
{
    /// <summary>
    /// This class demonstrates various LINQ query tasks
    /// that students can implement to practice querying
    /// an EF Core database.
    /// 
    /// ASSIGNMENT INSTRUCTIONS:
    ///   1. For each method labeled "TODO", write the necessary
    ///      LINQ query to return or display the required data.
    ///   2. Print meaningful output to the console (or return
    ///      collections, as needed).
    ///   3. Test each method by calling it from your Program.cs
    ///      or test harness.
    /// </summary>
    public static class LinqQueriesAssignment
    {
        /// <summary>
        /// 1. List all customers in the database:
        ///    - Print each customer's full name (First + Last) and Email.
        /// </summary>
        public static async Task Task01_ListAllCustomers(WebStoreContext context)
        {
            // TODO: Write a LINQ query that fetches all customers
            //       and prints their names + emails to the console.
            // HINT: context.Customers
        }

        /// <summary>
        /// 2. Fetch all orders along with:
        ///    - Customer Name
        ///    - Order ID
        ///    - Order Status
        ///    - Number of items in each order (the sum of OrderItems.Quantity)
        /// </summary>
        public static async Task Task02_ListOrdersWithItemCount(WebStoreContext context)
        {
            // TODO: Write a query to return all orders,
            //       along with the associated customer name, order status,
            //       and the total quantity of items in that order.

            // HINT: Use Include/ThenInclude or projection with .Select(...).
            //       Summing the quantities: order.OrderItems.Sum(oi => oi.Quantity).
        }

        /// <summary>
        /// 3. List all products (ProductName, Price),
        ///    sorted by price descending (highest first).
        /// </summary>
        public static async Task Task03_ListProductsByDescendingPrice(WebStoreContext context)
        {
            // TODO: Write a query to fetch all products and sort them
            //       by descending price.
            // HINT: context.Products.OrderByDescending(p => p.Price)
        }

        /// <summary>
        /// 4. Find all "Pending" orders (order status = "Pending")
        ///    and display:
        ///      - Customer Name
        ///      - Order ID
        ///      - Order Date
        ///      - Total price (sum of unit_price * quantity - discount) for each order
        /// </summary>
        public static async Task Task04_ListPendingOrdersWithTotalPrice(WebStoreContext context)
        {
            // TODO: Write a query to fetch only PENDING orders,
            //       and calculate their total price.
            // HINT: The total can be computed from each OrderItem:
            //       (oi.UnitPrice * oi.Quantity) - oi.Discount
        }

        /// <summary>
        /// 5. List the total number of orders each customer has placed.
        ///    Output should show:
        ///      - Customer Full Name
        ///      - Number of Orders
        /// </summary>
        public static async Task Task05_OrderCountPerCustomer(WebStoreContext context)
        {
            // TODO: Write a query that groups by Customer,
            //       counting the number of orders each has.

            // HINT: 
            //  1) Join Orders and Customers, or
            //  2) Use the navigation (context.Orders or context.Customers),
            //     then group by customer ID or by the customer entity.
        }

        /// <summary>
        /// 6. Show the top 3 customers who have placed the highest total order value overall.
        ///    - For each customer, calculate SUM of (OrderItems * Price).
        ///      Then pick the top 3.
        /// </summary>
        public static async Task Task06_Top3CustomersByOrderValue(WebStoreContext context)
        {
            // TODO: Calculate each customer's total order value 
            //       using their Orders -> OrderItems -> (UnitPrice * Quantity - Discount).
            //       Sort descending and take top 3.

            // HINT: You can do this in a single query or multiple steps.
            //       One approach:
            //         1) Summarize each Order's total
            //         2) Summarize for each Customer
            //         3) Order by descending total
            //         4) Take(3)
        }

        /// <summary>
        /// 7. Show all orders placed in the last 30 days (relative to now).
        ///    - Display order ID, date, and customer name.
        /// </summary>
        public static async Task Task07_RecentOrders(WebStoreContext context)
        {
            // TODO: Filter orders to only those with OrderDate >= (DateTime.Now - 30 days).
            //       Output ID, date, and the customer's name.
        }

        /// <summary>
        /// 8. For each product, display how many total items have been sold
        ///    across all orders.
        ///    - Product name, total sold quantity.
        ///    - Sort by total sold descending.
        /// </summary>
        public static async Task Task08_TotalSoldPerProduct(WebStoreContext context)
        {
            // TODO: Group or join OrdersItems by Product.
            //       Summation of quantity.
        }

        /// <summary>
        /// 9. List any orders that have at least one OrderItem with a Discount > 0.
        ///    - Show Order ID, Customer name, and which products were discounted.
        /// </summary>
        public static async Task Task09_DiscountedOrders(WebStoreContext context)
        {
            // TODO: Identify orders with any OrderItem having (Discount > 0).
            //       Display order details, plus the discounted products.
        }

        /// <summary>
        /// 10. (Open-ended) Combine multiple joins or navigation properties
        ///     to retrieve a more complex set of data. For example:
        ///     - All orders that contain products in a certain category
        ///       (e.g., "Electronics"), including the store where each product
        ///       is stocked most. (Requires `Stocks`, `Store`, `ProductCategory`, etc.)
        ///     - Or any custom scenario that spans multiple tables.
        /// </summary>
        public static async Task Task10_AdvancedQueryExample(WebStoreContext context)
        {
            // TODO: Design your own complex query that demonstrates
            //       multiple joins or navigation paths. For example:
            //       - Orders that contain any product from "Electronics" category.
            //       - Then, find which store has the highest stock of that product.
            //       - Print results.

            // Here's an outline you could explore:
            // 1) Filter products by category name "Electronics"
            // 2) Find any orders that contain these products
            // 3) For each of those products, find the store with the max stock
            //    (requires .MaxBy(...) in .NET 6+ or custom code in older versions)
            // 4) Print a combined result

            // (Implementation is left as an exercise.)

            System.Console.WriteLine("\n=== TASK 10: Advanced Query (Exercise) ===");
            System.Console.WriteLine("TODO: Implement a custom query that combines multiple joins.");
            await Task.CompletedTask; // no-op for demonstration
        }
    }
}
