using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using dotnet_mvc_crud.Models;

namespace dotnet_mvc_crud.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=sahan_dilshan;Initial Catalog=dotnet-mvc-crud;Integrated Security=True;Encrypt=False;";
        
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Product", conn);
                adapter.Fill(dt);
            }
              
            return View(dt);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel productModel)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Product VALUES(@ProductName, @Price, @Count)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                cmd.Parameters.AddWithValue("@Price", productModel.Price);
                cmd.Parameters.AddWithValue("@Count", productModel.Count);
                cmd.ExecuteNonQuery();  

            }

            return RedirectToAction("Index");
           
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Product WHERE ProductId = @ProductId";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@ProductId", id);
                adapter.Fill(dt);
            }
            if(dt.Rows.Count == 1)
            {
                productModel.ProductId = Convert.ToInt32(dt.Rows[0][0].ToString());
                productModel.ProductName = dt.Rows[0][1].ToString();
                productModel.Price = Convert.ToDecimal(dt.Rows[0][2].ToString());
                productModel.Count = Convert.ToInt32(dt.Rows[0][3].ToString());
                return View(productModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductModel productModel)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Product SET ProductName= @ProductName ,Price = @Price ,Count = @Count WHERE ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue(@"ProductId", productModel.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", productModel.ProductName);
                    cmd.Parameters.AddWithValue("@Price", productModel.Price);
                    cmd.Parameters.AddWithValue("@Count", productModel.Count);
                    cmd.ExecuteNonQuery();

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Product WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue(@"ProductId",id);
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction("Index");
        }

  
    }
}
