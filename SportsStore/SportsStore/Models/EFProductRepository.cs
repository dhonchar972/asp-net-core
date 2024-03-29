﻿using System.Linq;

namespace SportsStore.Models;

public class EFProductRepository : IProductRepository
{
    private readonly ApplicationDbContext context;

    public EFProductRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IQueryable<Product> Products { get { return context.Products; } }

    public void SaveProduct(Product product)
    {
        if (product.ProductID == 0)
        {
            context.Products.Add(product);
        }
        else
        {
            Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (dbEntry != null)
            {
                dbEntry.Name = product.Name;
                dbEntry.Description = product.Description;
                dbEntry.Price = product.Price;
                dbEntry.Category = product.Category;
            }
        }
        context.SaveChanges();
    }

    public Product DeleteProduct(int productID)
    {
        Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == productID);
        if (dbEntry != null)
        {
            context.Products.Remove(dbEntry);
            context.SaveChanges();
        }
        return dbEntry;
    }
}