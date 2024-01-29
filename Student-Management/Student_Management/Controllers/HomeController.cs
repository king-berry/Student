using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Student_Management.Models;
using System.Diagnostics;

namespace Student_Management.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly Student_ManagementContext _context;
        public HomeController(ILogger<HomeController> logger, Student_ManagementContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
        
        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
