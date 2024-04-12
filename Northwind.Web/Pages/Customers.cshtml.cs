using Microsoft.AspNetCore.Mvc;
using Northwind.EntityModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Web.Pages
{
    public class CustomersModel : PageModel
    {
        private NorthwindContext _db;

        public Customer[] Customers { get; set; } = null!;
        public IQueryable<IGrouping<string?, Customer>> CustomersByCountry { get; set; } = null!;


        public CustomersModel(NorthwindContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            ViewData["Title"] = "Customers";
            CustomersByCountry = _db.Customers.GroupBy(c => c.Country);
            foreach (var group in CustomersByCountry)
            {
                Console.WriteLine($"Country: {group.Key}");
                foreach (var customer in group)
                {
                    Console.WriteLine(customer.ContactName);
                }
                Console.WriteLine();
            }
        }
    }
}
