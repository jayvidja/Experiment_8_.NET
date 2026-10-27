
using Experiment_8.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Experiment_8.Controllers
{
    public class ProductController : Controller
    {
        string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InventoryDB;Integrated Security=True;";

        // GET: Product/Index
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = dr["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(dr["Quantity"]),
                            Price = Convert.ToDecimal(dr["Price"])
                        });
                    }
                }
            }

            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (!ModelState.IsValid)
                return View(p);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                    (ProductId, ProductName, Quantity, Price)
                    VALUES (@ProductId, @ProductName, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product p = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        p = new Product
                        {
                            ProductId = Convert.ToInt32(dr["ProductId"]),
                            ProductName = dr["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(dr["Quantity"]),
                            Price = Convert.ToDecimal(dr["Price"])
                        };
                    }
                }
            }

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            if (!ModelState.IsValid)
                return View(p);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products SET
                    ProductName = @ProductName,
                    Quantity = @Quantity,
                    Price = @Price
                    WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProductId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
