using Microsoft.AspNetCore.Mvc;
using Northwind.EntityModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Web.Pages
{
    public class CustomerOrdersModel : PageModel
    {
        private NorthwindContext _db;

        public CustomerOrdersModel(NorthwindContext db)
        {
            _db = db;
        }
        public Customer? Customer { get; set; } = null;
        public void OnGet(string customerId)
        {
            if(customerId == null)
            {
                ViewData["Title"] = $"customer-not-provided";
                ViewData["Placeholder"] = "No customer id provided";
                return;
            }

            Customer = _db.Customers.Include(c => c.Orders).Single(c=> c.CustomerId == customerId);
            ViewData["Title"] = $"Customer #{customerId}";
            
        }
    }
}
