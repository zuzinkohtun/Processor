using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProcessorApp.Models;

namespace ProcessorApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Compute(string arrayList, string compute)
    {
        string result = "Invalid input.";

        if (!string.IsNullOrWhiteSpace(arrayList))
        {
            //Split to list of string by comma from input
            var stringList = arrayList.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // Change from list of string to list of integer
            // Will remove other strings except digits
            var numberList = stringList.Where(s => int.TryParse(s.Trim(), out _)).Select(s => int.Parse(s.Trim())).ToList();

            if (numberList is not null && numberList.Any())
            {
                switch (compute)
                {
                    case "ListOdd":
                        var odds = numberList.Where(n => n % 2 != 0).ToList();
                        if (odds.Any())
                            result = "Odd Numbers: " + string.Join(", ", odds);
                        else
                            result = "No odd number found.";
                        break;
                    case "Sum":
                        result = "Sum: " + numberList.Sum();
                        break;
                    case "CountDuplicate":
                        var duplicates = numberList.GroupBy(n => n).Where(g => g.Count() > 1);
                        if (duplicates.Any())
                            result = "Duplicates: " + string.Join(", ", duplicates.Select(d => $"{d.Key} ({d.Count()} times)"));
                        else
                            result = "No duplicates found.";
                        break;
                    default:
                        result = "System Error!";
                        break;
                }
            }
        }

        TempData["ResultMessage"] = result;
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
