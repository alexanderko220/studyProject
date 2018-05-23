using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using studyProject.DataLayer;
using studyProject.Models;
using TreeUtility;
using studyProject.ViewModels;
using System.Data.Entity.Infrastructure;

namespace studyProject.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


       private List<Category> GetListOfNodes()
        {
            List<Category> courceCategories = _applicationDbContext.Categories.ToList();
            List<Category> categories = new List<Category>();
            foreach (var sourceCategory in courceCategories)
            {
                Category c = new Category();

                c.Id = sourceCategory.Id;
                c.CategoryName = sourceCategory.CategoryName;
                if(sourceCategory.ParentCategoryId != null)
                {
                    c.Parent = new Category();
                    c.Parent.Id = (int)sourceCategory.ParentCategoryId;
                }
                categories.Add(c);
            }
            return categories;
        }

        private string EnumerateNodes(Category parent)
        {
            //init an empty string
            string content = String.Empty;

            // Add <li> category name
            content += "<li class=\"list-group-item treenode\">";
            content += parent.CategoryName;
            content += String.Format("<span><a href=\"/Categories/Details/{0}\" class=\"btn btn-primary treenodedetailsbutton\">Details</a></span>", parent.Id);
            content += String.Format("<span><a href=\"/Categories/Edit/{0}\" class=\"btn btn-primary treenodeeditbutton\">Edit</a></span>", parent.Id);
            content += String.Format("<span><a href=\"/Categories/Delete/{0}\" class=\"btn btn-danger treenodedeletebutton\">Delete</a></span>", parent.Id);
            
            //if there are no children, end the </li>
            if (parent.Children.Count == 0)
                content += "</li>";
            else // if there are children, start a <ul>
                content += "<ul class=\"list-group\">";
            // Loop one past the number of children
            int numberOfChildren = parent.Children.Count;
            for (int i = 0; i <= numberOfChildren; i++)
            {
                // if this iteration's idex points to a child,
                //call this function recursively
                if(numberOfChildren > 0 && i < numberOfChildren)
                {
                    Category child = parent.Children[i];
                    content += EnumerateNodes(child);
                }
                //if this iteration's inex point past the children. end the </ul>
                if (numberOfChildren > 0 && i == numberOfChildren)
                    content += "</ul>";
            }
            return content;
        }

        private void ValidateParentsAreParentless(Category category)
        {
            // there in no parent
            if (category.ParentCategoryId == null)
                return;
            //the parent has a parent
            Category parentCategory = _applicationDbContext.Categories.Find(category.ParentCategoryId);
            if (parentCategory.ParentCategoryId != null)
            {
                throw new InvalidOperationException("You cannot nest this category more than two levels deep.");
            }
            //the parent does Not have a parent, but the category being nested has children
            int numberOfChildren = _applicationDbContext.Categories.Count(c => c.ParentCategoryId == category.Id);
            if (numberOfChildren > 0)
                throw new InvalidOperationException("You cannot nest this category's more than two levels deep.");
        }

        private SelectList PopulateParentCategorySelectList(int? id)
        {
            SelectList selectList;

            if (id == null)
                selectList = new SelectList(
                    _applicationDbContext
                    .Categories
                    .Where(c => c.ParentCategoryId == null), "Id", "CategoryName");
            else if (_applicationDbContext.Categories.Count(c => c.ParentCategoryId == id) == 0)
                selectList = new SelectList(
                    _applicationDbContext
                    .Categories
                    .Where(c => c.ParentCategoryId == null && c.Id != id), "Id", "CategoryName");
            else
                selectList = new SelectList(
                    _applicationDbContext
                    .Categories
                    .Where(c => false), "Id", "CategoryName");

            return selectList;

        }
        public ActionResult Index()
        {
            //var categories = _applicationDbContext.Categories.Include(c => c.Parent);
            //return View(await categories.ToListAsync());

            //Start the outermost list
            string fullString = "<ul>";
            IList<Category> listOfNodes = GetListOfNodes();
            IList<Category> topLevelCategories = TreeHelper.ConvertToForest(listOfNodes);

            foreach (var category in topLevelCategories)
            {
                fullString += EnumerateNodes(category);
            }
            //End the outermost list
            fullString += "</ul>";
            return View((object)fullString);
        }
        
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ParentCategoryId = PopulateParentCategorySelectList(null);
            return View();
        }

        // POST: Categories/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ParentCategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidateParentsAreParentless(category);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.ParentCategoryId = PopulateParentCategorySelectList(null);
                    return View(category);
                }

                _applicationDbContext.Categories.Add(category);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ParentCategoryId = PopulateParentCategorySelectList(null);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            //Wind-up Category viewmodel
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.Id = category.Id;
            categoryViewModel.ParentCategoryId = category.ParentCategoryId;
            categoryViewModel.CategoryName = category.CategoryName;

            ViewBag.ParentCategoryId = PopulateParentCategorySelectList(categoryViewModel.Id);
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ParentCategoryId,CategoryName")] CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                // Unwind back to a Category
                Category editedCategory = new Category();
                try
                {
                    editedCategory.Id = categoryViewModel.Id;
                    editedCategory.ParentCategoryId = categoryViewModel.ParentCategoryId;
                    editedCategory.CategoryName = categoryViewModel.CategoryName;
                    ValidateParentsAreParentless(editedCategory);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.ParentCategoryId = PopulateParentCategorySelectList(categoryViewModel.Id);
                    return View("Edit", categoryViewModel);
                }

                _applicationDbContext.Entry(editedCategory).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryId = PopulateParentCategorySelectList(categoryViewModel.Id);
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _applicationDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await _applicationDbContext.Categories.FindAsync(id);

            try
            {
                _applicationDbContext.Categories.Remove(category);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "You attempted to delete a category that had child categories associated with it.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Delete", category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
