using FontaineApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FontaineApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using FontaineApp.Areas.Identity.Data;

namespace FontaineApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Articles(UploadedFileViewModel vm)
        {
            vm.SystemFiles = await _context.UploadedFiles.ToListAsync();
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> FileUpload(UploadedFileViewModel vm,IFormFile file)
        {
            var filename = DateTime.Now.ToString("yyyymmddhhmmss");
            filename = filename + "_" + file.FileName;
            var path = $"{_configuration.GetSection("FileManagement:SystemFileUploads").Value}";
            var filepath = Path.Combine(path, filename);

            var stream = new FileStream(filepath, FileMode.Create);
            await file.CopyToAsync(stream);

            var fileuploaded = new UploadedFile
            {
                UploadedAt = DateTime.Now,
                FileName = filename,
                Description = vm.Description,
                FilePath = filepath,
                Title = vm.Title,
            };
            await _context.AddAsync(fileuploaded);
            await _context.SaveChangesAsync();

            return RedirectToAction("Articles");
        }

        public IActionResult ViewPDF(int id)
        {
            var fileuploaded = _context.UploadedFiles.FirstOrDefault(f => f.Id == id);
            if (fileuploaded == null || string.IsNullOrEmpty(fileuploaded.FilePath))
            {
                return NotFound();
            }

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/uploads", fileuploaded.FilePath);
            byte[] filebytes = System.IO.File.ReadAllBytes(filepath);
            return File(filebytes, "application/pdf");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.UploadedFiles.FindAsync(id);
            if (file == null) return NotFound();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/uploads", file.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.UploadedFiles.Remove(file);
            await _context.SaveChangesAsync();

            return RedirectToAction("Articles");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
