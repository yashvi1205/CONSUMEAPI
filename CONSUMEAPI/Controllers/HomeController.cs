using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CONSUMEAPI.Models;

// namespace Admin.Controllers;
// [CheckAccess]

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

    public IActionResult ChartsApexCharts()
    {
        return View("Charts/Charts-ApexCharts");
    }

    public IActionResult Chartjs()
    {
        return View("Charts/Chartjs");
    }
    
    public IActionResult Echarts()
    {
        return View("Charts/Echarts");
    }
    
    public IActionResult Accodians()
    {
        return View("Components/Accodians");
    }
    public IActionResult Alerts()
    {
        return View("Components/Alerts");
    }
    public IActionResult Badges()
    {
        return View("Components/Badges");
    }
    public IActionResult Breadcrumbs()
    {
        return View("Components/Breadcrumbs");
    }
    public IActionResult Buttons()
    {
        return View("Components/Buttons");
    }
    public IActionResult Cards()
    {
        return View("Components/Cards");
    }
    public IActionResult Carousels()
    {
        return View("Components/Carousel");
    }
    public IActionResult ListGroup()
    {
        return View("Components/List-Group");
    }
    public IActionResult Modal()
    {
        return View("Components/Modal");
    }
    public IActionResult Pagination()
    {
        return View("Components/Pagination");
    }
    public IActionResult Progress()
    {
        return View("Components/Progress");
    }
   
    public IActionResult Spinners()
    {
        return View("Components/Spinners");
    }
    public IActionResult Tabs()
    {
        return View("Components/Tabs");
    }
    public IActionResult ToolTips()
    {
        return View("Components/ToolTips");
    }
    
    public IActionResult Editors()
    {
        return View("Forms/Editors");
    }
    public IActionResult Elements()
    {
        return View("Forms/Elements");
    }
    public IActionResult Layouts()
    {
        return View("Forms/Layouts");
    }
    public IActionResult Validation()
    {
        return View("Forms/Validation");
    }
    public IActionResult TablesData()
    {
        return View("Tables/TablesData");
    }
    public IActionResult TablesGeneral()
    {
        return View("Tables/TablesGeneral");
    }
    public IActionResult Bootstrap()
    {
        return View("Icons/Bootstrap");
    }
    public IActionResult Boxicons()
    {
        return View("Icons/Boxicons");
    }
    public IActionResult Remix()
    {
        return View("Icons/Remix");
    }
    
    public IActionResult UserProfile()
    {
        return View("UserProfile");
    }
    public IActionResult Faq()
    {
        return View("Pages/Faq");
    }
    public IActionResult Contact()
    {
        return View("Pages/Contact");
    }
    public IActionResult Error404()
    {
        return View("Pages/Error404");
    }
    public IActionResult Login()
    {
        return View("Pages/Login");
    }
    public IActionResult Register()
    {
        return View("Pages/Register");
    }
    public IActionResult Blank()
    {
        return View("Pages/Blank");
    }

    public IActionResult DepartmentTable()
    {
        return View("Tables/DepartmentTable");
    }

    public IActionResult DepartmentForm()
    {
        return View("Forms/DepartmentForm");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}