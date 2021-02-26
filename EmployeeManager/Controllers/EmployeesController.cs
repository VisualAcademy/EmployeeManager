using EmployeeManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _employeeContext;

        public EmployeesController(EmployeeDbContext context)
        {
            _employeeContext = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder; 
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter; 
            }            
            
            ViewData["CurrentFilter"] = searchString; 

            var employees = from emp in _employeeContext.Employees
                            select emp;

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(emp => emp.LastName.Contains(searchString)
                    || emp.FirstName.Contains(searchString)
                    || emp.Address.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(emp => emp.LastName);
                    break;
                case "Date":
                    employees = employees.OrderBy(emp => emp.CreatedAt);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(emp => emp.CreatedAt);
                    break; 
                default:
                    employees = employees.OrderByDescending(emp => emp.Id); 
                    break;
            }

            //return View(await employees.AsNoTracking().ToListAsync());
            int pageSize = 2;
            return View(await PaginatedList<Employee>.CreateAsync(
                employees.AsNoTracking(),
                pageNumber ?? 1,
                pageSize));
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeContext.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,DOB,SSN,Email,Address,City,State,PostalCode,Gender,Active,CreatedAt")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeContext.Add(employee);
                await _employeeContext.SaveChangesAsync();

                await AddEmployeeHistory(employee);

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        /// <summary>
        /// 변경 내역 남기기 
        /// </summary>
        private async Task AddEmployeeHistory(Employee employee)
        {
            try
            {
                var history = new EmployeeHistory()
                {
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address = employee.Address,
                    CreatedAt = DateTime.Now,
                    Email = employee.Email,
                    Gender = employee.Gender,
                };
                _employeeContext.Add(history);
                await _employeeContext.SaveChangesAsync();
            }
            catch (System.Exception)
            {

            }
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,MiddleName,DOB,SSN,Email,Address,City,State,PostalCode,Gender,Active,CreatedAt")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeContext.Update(employee);
                    await _employeeContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


                await AddEmployeeHistory(employee);


                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeContext.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employee = await _employeeContext.Employees.FindAsync(id);
            _employeeContext.Employees.Remove(employee);
            await _employeeContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(long id)
        {
            return _employeeContext.Employees.Any(e => e.Id == id);
        }
    }
}
