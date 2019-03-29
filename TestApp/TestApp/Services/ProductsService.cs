using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TestApp.Models;

namespace TestApp.Services
{
    public class ProductsService
    {
        private TestDBContext testDBContext;

        public ProductsService()
        {
            testDBContext = new TestDBContext();
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

        public IEnumerable<Product> Read()
        {
            return GetAllProducts();
        }
        
        public void CreateProduct(Product product, int categoryId)
        {
            try
            {
                var newProduct = new Product();

                newProduct.Name = product.Name;
                newProduct.ValidFrom = product.ValidFrom;
                newProduct.Price = product.Price;
                newProduct.Quantity = product.Quantity;
                newProduct.ProductCategoryID = categoryId;

                testDBContext.Products.Add(newProduct);
                testDBContext.SaveChanges();

                product.ProductID = newProduct.ProductID;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Product product)
        {
            try
            {
                Product productToUpdate = GetAllProducts().Where(x => x.ProductCategoryID == product.ProductCategoryID).FirstOrDefault();

                if (productToUpdate != null)
                {
                    productToUpdate.Name = product.Name;
                    productToUpdate.ValidFrom = product.ValidFrom;
                    productToUpdate.Quantity = product.Quantity;
                    productToUpdate.Price = product.Price;

                    testDBContext.Products.Attach(productToUpdate);
                    testDBContext.Entry(productToUpdate).State = EntityState.Modified;
                    testDBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int productID)
        {
            try
            {
                Product productToDelete = GetAllProducts().Where(x => x.ProductID == productID).FirstOrDefault();

                if (productToDelete != null)
                {
                    testDBContext.Products.Attach(productToDelete);
                    testDBContext.Products.Remove(productToDelete);

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