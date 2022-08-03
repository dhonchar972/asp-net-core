﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers;

[Authorize]
public class AdminController : Controller
{
    private readonly IProductRepository repository;

    public AdminController(IProductRepository repository)
    {
        this.repository = repository;
    }

    public ViewResult Index()
    {
        return View(repository.Products);
    }

    public ViewResult Edit(int productId)
    {
        return View(repository.Products.FirstOrDefault(p => p.ProductID == productId));
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            repository.SaveProduct(product);
            TempData["message"] = $"{product.Name} has been saved";
            return RedirectToAction("Index");
        }
        else
        {
            // there is something wrong with the data values
            return View(product);
        }
    }

    public ViewResult Create()
    {
        return View("Edit", new Product());
    }

    [HttpPost]
    public IActionResult Delete(int productId)
    {
        Product deletedProduct = repository.DeleteProduct(productId);
        if (deletedProduct != null)
        {
            TempData["message"] = $"{deletedProduct.Name} was deleted";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SeedDatabase()
    {
        SeedData.EnsurePopulated(HttpContext.RequestServices);
        return RedirectToAction(nameof(Index));
    }
}