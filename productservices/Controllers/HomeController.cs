using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using productservices.data.interfaces;
using productservices.data.models;
using productservices.data.repo;
using System.Data;
using userservice.helper;

namespace productservices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Icategory _categoryRepo;
        private readonly Iproduct _productRepo;

        public ProductController(Icategory categoryRepo, Iproduct productRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
        }


        [Authorize]
        [HttpGet("categories")]
        public IActionResult GetCategories(int? id)
        {
            DataTable dt = _categoryRepo.GetCategories(id);
            return Ok(DbHelper.ToJson(dt));
        }

        [Authorize]
        [HttpPost("categories")]
        public IActionResult SaveCategory([FromBody] CategoryModel model)
        {
            var result = _categoryRepo.SaveCategory(model.Id, model.Name);
            return Ok(result);
        }


        [Authorize]
        [HttpGet("products")]
        public IActionResult GetProducts(int? id)
        {
            DataTable dt = _productRepo.GetProduct(id);
            return Ok(DbHelper.ToJson(dt));
        }

        [Authorize]
        [HttpPost("products")]
        public IActionResult SaveProduct([FromBody] ProductModel model)
        {
            var result = _productRepo.SaveProduct(
                model.CategoryId,
                model.Name,
                model.Description,
                model.Price
            );
            return Ok(result);
        }
    }



}
