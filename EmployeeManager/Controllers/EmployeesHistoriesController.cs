using EmployeeManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    public class EmployeesHistoriesController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeesHistoriesController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: EmployeesHistories
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.EmployeesHistories.ToListAsync());
        //}
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

            var histories = from h in _context.EmployeesHistories
                            select h;

            if (!string.IsNullOrEmpty(searchString))
            {
                histories = histories.Where(emp => emp.LastName.Contains(searchString)
                    || emp.FirstName.Contains(searchString)
                    || emp.Remarks.Contains(searchString)
                    || emp.Address.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    histories = histories.OrderByDescending(h => h.LastName);
                    break;
                case "Date":
                    histories = histories.OrderBy(h => h.CreatedAt);
                    break;
                case "date_desc":
                    histories = histories.OrderByDescending(h => h.CreatedAt);
                    break;
                default:
                    histories = histories.OrderByDescending(h => h.Id);
                    break;
            }

            //return View(await histories.AsNoTracking().ToListAsync());
            int pageSize = 2;
            return View(await PaginatedList<EmployeeHistory>.CreateAsync(
                histories.AsNoTracking(),
                pageNumber ?? 1,
                pageSize));
        }

        // GET: EmployeesHistories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeHistory = await _context.EmployeesHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeHistory == null)
            {
                return NotFound();
            }

            return View(employeeHistory);
        }

        // GET: EmployeesHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmployeesHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Id,FirstName,LastName,MiddleName,DOB,SSN,Email,Address,City,State,PostalCode,Gender,Active,CreatedAt,Remarks")] EmployeeHistory employeeHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeHistory);
        }

        // GET: EmployeesHistories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeHistory = await _context.EmployeesHistories.FindAsync(id);
            if (employeeHistory == null)
            {
                return NotFound();
            }
            return View(employeeHistory);
        }

        // POST: EmployeesHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EmployeeId,Id,FirstName,LastName,MiddleName,DOB,SSN,Email,Address,City,State,PostalCode,Gender,Active,CreatedAt,Remarks")] EmployeeHistory employeeHistory)
        {
            if (id != employeeHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeHistoryExists(employeeHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeHistory);
        }

        // GET: EmployeesHistories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeHistory = await _context.EmployeesHistories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeHistory == null)
            {
                return NotFound();
            }

            return View(employeeHistory);
        }

        // POST: EmployeesHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employeeHistory = await _context.EmployeesHistories.FindAsync(id);
            _context.EmployeesHistories.Remove(employeeHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeHistoryExists(long id)
        {
            return _context.EmployeesHistories.Any(e => e.Id == id);
        }
    }
}
