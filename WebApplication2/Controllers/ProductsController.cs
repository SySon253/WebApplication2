using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.ViewModels;

namespace WebApplication2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebApplication2Context _context;
        public int PageSize = 9;
        public ProductsController(WebApplication2Context context)
        {
            _context = context;
        }
        public class PriceRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }
        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        {
            var filterdProducts = _context.Product.ToList();
            if (filter.PriceRanges != null && filter.PriceRanges.Count > 0 && !filter.PriceRanges.Contains("all"))
            {
                List<PriceRange> priceRanges = new List<PriceRange>();
                foreach (var range in filter.PriceRanges)
                {
                    var value = range.Split("-").ToArray();
                    PriceRange priceRange = new PriceRange();
                    priceRange.Min = Int16.Parse(value[0]);
                    priceRange.Max = Int16.Parse(value[1]);
                    priceRanges.Add(priceRange);
                }
                filterdProducts = filterdProducts.Where(p => priceRanges.Any(r => p.ProductPrice >= r.Min && p.ProductPrice <= r.Max)).ToList();
            }
            if (filter.Colors != null && filter.Colors.Count > 0 && !filter.Colors.Contains("all"))
            {
                filterdProducts = filterdProducts.Where(p => filter.Colors.Contains(p.Color.ColorName)).ToList();
            }
            if (filter.Sizes != null && filter.Sizes.Count > 0 && !filter.Sizes.Contains("all"))
            {
                filterdProducts = filterdProducts.Where(p => filter.Sizes.Contains(p.Size.SizeName)).ToList();
            }
            return PartialView("_ReturnProducts", filterdProducts);
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var webApplication2Context = _context.Product.Include(p => p.Category).Include(p => p.Color).Include(p => p.Size);
            return View(await webApplication2Context.ToListAsync());
        }
        //GET: Products/Shop------------
        public async Task<IActionResult> Shop(int productPage = 1)
        {


            return View(
                new ProductListViewModel
                {
                    Products = _context.Product
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Product.Count()
                    }
                }
                );
        }
        [HttpPost]
        public async Task<IActionResult> Search(string keywords, int productPage = 1)
        {


            return View("Index",
                new ProductListViewModel
                {
                    Products = _context.Product
                    .Where(p => p.ProductName.Contains(keywords))
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = _context.Product.Count()
                    }
                }
                );
        }
        //public async Task<IActionResult> ProductByCart(int categoryId)
        //{
        //    var webApplication2Context = _context.Product.Where(p => p.CategoryId == categoryId).Include(p => p.Category).Include(p => p.Color).Include(p => p.Size);
        //    return View("Index", await webApplication2Context.ToListAsync());
        //}


        //public async Task<IActionResult> ProductByCart(int categoryId)
        //{
        //    var webApplication2Context = _context.Product
        //        .Where(p => p.CategoryId == categoryId)
        //        .Include(p => p.Category)
        //        .Include(p => p.Color)
        //        .Include(p => p.Size);

        //    return View("ProductByCart", await webApplication2Context.ToListAsync());
        //}

        public async Task<IActionResult> ProductByCart(int categoryId, int productPage = 1)
        {
            int pageSize = 9; // số sản phẩm mỗi trang
            var productsQuery = _context.Product
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size);

            var count = await productsQuery.CountAsync();

            var products = await productsQuery
                .OrderBy(p => p.ProductId)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = count
                },
                CurrentCategory = categoryId
            };

            return View("ProductByCart", viewModel);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["ColorId"] = new SelectList(_context.Color, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductDiscount,ProductQuantity,ProductPhoto,CategoryId,SizeId,ColorId,IsTrandy,IsArrived,IsSportSwear,IsFeatured,IsOfficeClothes,IsWinterClothes,IsPoloShirt,IsShorts")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Color, "ColorId", "ColorName", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Color, "ColorId", "ColorName", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductDiscount,ProductQuantity,ProductPhoto,CategoryId,SizeId,ColorId,IsTrandy,IsArrived,IsSportSwear,IsFeatured,IsOfficeClothes,IsWinterClothes,IsPoloShirt,IsShorts")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Color, "ColorId", "ColorName", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Size, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }


    }
}
