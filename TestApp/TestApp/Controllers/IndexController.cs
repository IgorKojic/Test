﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class IndexController : Controller
    {
        private readonly CategoriesService _categoriesService;
        private readonly ProductsService _productService;

        public IndexController(CategoriesService categoriesService, ProductsService productsService)
        {
            _categoriesService = categoriesService;
            _productService = productsService;
        }

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
                 return View();

            return RedirectToAction("Login", "User");
        }

        public ActionResult Categories_Read([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return Json(_categoriesService.Read().ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Create([DataSourceRequest] DataSourceRequest request, Category category)
        {
            try
            {
                if (category != null && ModelState.IsValid)
                {
                    _categoriesService.CreateCategory(category);
                }

                return Json(new[] { category }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });

            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Update([DataSourceRequest]DataSourceRequest request, Category categoryModel)
        {
            try
            {
                if (categoryModel != null && ModelState.IsValid)
                {
                    _categoriesService.Update(categoryModel);
                }

                return Json(new[] { categoryModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Destroy([DataSourceRequest] DataSourceRequest request, Category category)
        {
            try
            {
                if (category != null && ModelState.IsValid)
                {
                    _categoriesService.Delete(category.CategoryID);
                }

                return Json(new[] { category }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }

        public ActionResult DetailTemplate_Products_By_Category(int categoryID, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = _categoriesService.ReadProductsByCategory(categoryID);

                return Json(result.ToDataSourceResult(request));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Create(int categoryID, [DataSourceRequest] DataSourceRequest request, Product product)
        {
            try
            {
                if (product != null && ModelState.IsValid)
                {
                    _productService.CreateProduct(product, categoryID);
                }

                return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Update([DataSourceRequest]DataSourceRequest request, Product product)
        {
            try
            {
                if (product != null && ModelState.IsValid)
                {
                    _productService.Update(product);
                }

                return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Product_Destroy([DataSourceRequest] DataSourceRequest request, Product product)
        {
            try
            {
                if (product != null && ModelState.IsValid)
                {
                    _productService.Delete(product.ProductID);
                }

                return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new DataSourceResult
                {
                    Errors = "A server error has occurred!"
                });
            }
        }
        protected override void Dispose(bool disposing)
        {
            _productService.Dispose();
            _categoriesService.Dispose();

            base.Dispose(disposing);
        }
    }
}
