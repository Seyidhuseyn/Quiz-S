using Microsoft.AspNetCore.Mvc;
using Quiz.DAL;
using Quiz.Models;
using Quiz.Utilies.Extension;
using Quiz.ViewModels;
using System.Drawing;

namespace Quiz.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Products.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid) return View();
            IFormFile file = productVM.Image;
            if (!file.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "Yuklediyiniz fal shekil deyil");
                return View();
            }
            if (file.Length > 200 * 1024)
            {
                ModelState.AddModelError("Image", "Shekilin olcusu 200 kb-dan artiq ola bilmez.");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            using (var stream = new FileStream(Path.Combine(_env.WebRootPath, "book-shop", "img", "product", fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            Product product = new Product { Name = productVM.Name, Price = productVM.Price, ImageUrl = fileName };

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        //IKINCI ÜSUL ---------------------------------

        //public IActionResult Create(CreateProductVM productVM)
        //{
        //    var imageP = productVM.Image;
        //    string result = imageP.CheckValidate("image/", 0);
        //    if (result.Length>0)
        //    {
        //        ModelState.AddModelError("Image", result);
        //    }
        //    Product newProduct = new Product
        //    {
        //        Name = productVM.Name,
        //        Image= productVM.Image,
        //        Price= productVM.Price
        //    };
        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0) return BadRequest();
            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            return View(product);
        }
        public IActionResult Update(int? id, Product product)
        {
            if (id == null || id == 0 || id != product.Id || product is null) return BadRequest();
            if (!ModelState.IsValid) return View();
            Product exist = _context.Products.Find(product.Id);
            exist.Name = product.Name;
            exist.Price = product.Price;
            exist.ImageUrl = product.ImageUrl;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();

            Product product = _context.Products.Find(id);
            if (product is null) return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
