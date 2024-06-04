using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    public class MonthCharController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthCharController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync(DateTime? SelectedDate, String type)
        {
            DateTime StartDate;
            DateTime EndDate;

            if (SelectedDate.HasValue)
            {
                StartDate = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                EndDate = StartDate.AddMonths(1).AddDays(-1);
            }

            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            if (type == "Expense" || type == "Income")
            {
                //Doughnut Chart -Expense By Category
                ViewBag.DoughnutChartData = SelectedTransactions
                    .Where(i => i.Category.Type == type)
                    .GroupBy(j => j.Category.CategoryId)
                    .Select(k => new
                    {
                        categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                        amount = k.Sum(j => j.Amount),
                        formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                    })
                    .OrderByDescending(l => l.amount)
                    .ToList();
            }

            return View();
        }

    }
}

