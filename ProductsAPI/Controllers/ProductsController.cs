using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    // [Authorize]  aşağıda kullanılırsa sadece kullandığı alan yetkili olur burda tüm controller
    [ApiController]
    [Route("api/[controller]")] //  [Route("api/[controller]")] bu ile  [Route("api/Products")] bu aynı
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;



        public ProductsController(ProductsContext context)
        {

            _context = context;
            //iki şekil kullanım mevcut
            //_products.Add(new() { productId = 1, productName = "IPhone 14", Price = 40000, IsActive = true });
            //_products.Add(new() { productId = 2, productName = "IPhone 15", Price = 50000, IsActive = true });
            //_products.Add(new Product { productId = 3, productName = "IPhone 16", Price = 60000, IsActive = true });
            //_products.Add(new Product { productId = 4, productName = "IPhone 17", Price = 70000, IsActive = true });

            //bu veriler contexte modelbuildera eklendi dataseeds için
            //_products = new List<Product>()
            //{
            //   new() { productId = 1, productName = "IPhone 14", Price = 40000, IsActive = true },
            //    new() { productId = 2, productName = "IPhone 15", Price = 50000, IsActive = true },
            //    new() { productId = 3, productName = "IPhone 16", Price = 60000, IsActive = true },
            //    new() { productId = 4, productName = "IPhone 17", Price = 70000, IsActive = true }
            //};



        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            // bununla alttaki sorgu aynı alttaki yeni kısa version
            // return _products == null ? new List<Product>() : _products;

            //if (_products == null) { return NotFound(); }

            //return Ok(_products);

            //bu şekilde de yazılabilir veya bir fonksiyon yazıp aşağıdaki gibi yazılabilir
            //var products = await _context.Products
            //    .Where(x=>x.IsActive==true)
            //    .Select(p => new productDTO { productId = p.productId, productName = p.productName, Price = p.Price })

            //    .ToListAsync();

            var products = await _context.Products
             .Where(x => x.IsActive == true)
             .Select(p => productToDTO(p))

             .ToListAsync();





            return Ok(products);


        }
        [HttpGet("{id}")] // bu kullanım ile [Route("api/[controller]/{id}")] bu aynı
         //[Authorize]// burda yaparsan sadece bu alan yetkili sorgu olur yukarda olursa bütün hepsi
        public async Task<IActionResult> GetProduct(int? id)
        {
            if (id == null) { return NotFound(); }

            var product = await _context
                .Products
                .Where(x => x.productId == id)
                  .Select(p => productToDTO(p))
                .FirstOrDefaultAsync();

            if (product == null) { return NotFound(); };

            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {

            if (entity == null) { return NotFound(); }

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = entity.productId }, entity);

        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if (id != entity.productId) { return BadRequest(); }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.productId == id);

            if (product == null) { return NotFound(); }


            product.productName = entity.productName;
            product.Price = entity.Price;
            product.IsActive = entity.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return NotFound();
            }


            return NoContent(); // başarılı ancak geriye değer dönmez 200 lü statüs verir
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) { return NotFound(); }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.productId == id);

            if (product == null) { return NotFound(); }

            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return NoContent();

        }


        private static productDTO productToDTO(Product p)
        {

            var entity = new productDTO();

            if (p != null)
            {
                entity.productId = p.productId;
                entity.productName = p.productName;
                entity.Price = p.Price;

            }

            return entity;
        }

    }
}
