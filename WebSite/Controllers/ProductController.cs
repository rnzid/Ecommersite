using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Data;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class ProductController : Controller
    {
        public readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<product> objList = _db.Product;
            foreach(var obj in objList)
            {
                obj.Category = _db.category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            return View(objList);
        }

        //Get-Upsert
        public IActionResult Upsert(int? id)
        {
            IEnumerable < SelectListItem > CategoryDropDown = _db.category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;  
            product Product = new product();
            if(id == null)
            {
                return View(Product);
            }
            else
            {
                Product = _db.Product.Find(id);
                if(Product == null)
                {
                    return NotFound();
                }
                return View(Product);
            }
            
        }

        //Post-Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(product obj)
        {
            
            if(ModelState.IsValid)
            {
                _db.Product.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }

        //Get-Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(product obj)
        {
            if (obj == null)
            {
                return NotFound();
            }

                _db.Product.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

        }
    }
}


