using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.EntityModels;

namespace Northwind.Razor.Employees.MyFeature.Pages
{
    public class EmployeesListPageModel : PageModel
    {

        private NorthwindContext _db;

        public Employee[] Employees { get; set; } = null!;
        public EmployeesListPageModel(NorthwindContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            ViewData["Title"] = "Northwind B2B - Employees";
            Employees = _db.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToArray();
        }
    }
}
