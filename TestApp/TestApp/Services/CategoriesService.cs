using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TestApp.Models;

namespace TestApp.Services
{
    public class CategoriesService
    {
        TestDBContext testDBContext;

        public CategoriesService()
        {
            testDBContext = new TestDBContext();
        }

        private IList<Category> GetAllCategories()
        {
            try
            {
                List<Category> categories = testDBContext.Categories.ToList();
                return categories;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private IList<Product> GetAllProducts()
        {
            try
            {
                List<Product> products = testDBContext.Products.ToList();
                return products;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Category> Read()
        {
            return GetAllCategories();
        }

        public IEnumerable<Product> ReadProductsByCategory(int categoryID)
        {
            return GetAllProducts().Where(x => x.ProductCategoryID == categoryID);
        }

        public void CreateCategory(Category category)
        {
            try
            {
               
                Category newCategory = new Category();
                newCategory.Name = category.Name;
                newCategory.ValidFrom = category.ValidFrom;

                testDBContext.Categories.Add(newCategory);
                testDBContext.SaveChanges();

                category.CategoryID = newCategory.CategoryID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Category category)
        {
            try
            {
                Category categoryToUpdate = GetAllCategories().Where(x => x.CategoryID == category.CategoryID).FirstOrDefault();

                if (categoryToUpdate != null)
                {
                    categoryToUpdate.Name = category.Name;
                    categoryToUpdate.ValidFrom = category.ValidFrom;

                    testDBContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int categoryID)
        {
            try
            {
                Category categoryToDelete = GetAllCategories().Where(x => x.CategoryID == categoryID).FirstOrDefault();

                List<Product> productsToDelete = GetAllProducts().Where(x => x.ProductCategoryID == categoryID).ToList();

                if (productsToDelete.Count > 0)
                {
                    foreach (Product productToDelete in productsToDelete)
                    {
                        testDBContext.Products.Attach(productToDelete);
                        testDBContext.Products.Remove(productToDelete);
                    }
                    testDBContext.SaveChanges();
                }
                if (categoryToDelete != null)
                {
                    testDBContext.Categories.Attach(categoryToDelete);
                    testDBContext.Categories.Remove(categoryToDelete);
                    testDBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            testDBContext.Dispose();
        }
    }
}