using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWII6P2.Models;
using SWII6P2.Services;

namespace SWII6P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<dynamic> CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest(new
                {
                    Message = "O produto não pode estar vazio."
                });
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Produto cadastrado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet("{ProductId}")]
        public async Task<ActionResult<dynamic>> GetProduct([FromRoute] int ProductId)
        {
            if (ProductId == 0)
            {
                return BadRequest(new
                {
                    Message = "Erro, o Id não pode ser vazio."
                });
            }
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                if (product == null)
                {
                    return NotFound(new
                    {
                        Message = "Erro, o produto não foi encontrado."
                    });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product, int lastUpdaterId)
        {
            if (product == null)
            {
                return BadRequest(new
                {
                    Message = "O novo produto não pode ser nulo."
                });
            }

            if (SWII6P2Verifications.Verifications.IsProductFull(product.Name, product.Price))
            {
                return BadRequest(new
                {
                    Message = "O produto não está devidamente preenchido."
                });
            }

            try 
            {
                var productToChange = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (productToChange == null)
                {
                    return NotFound(new
                    {
                        Message = "Produto não encontrado."
                    });
                }

                productToChange.Price = product.Price;
                productToChange.Name = product.Name;
                productToChange.Status = product.Status;
                productToChange.LastUpdaterId = lastUpdaterId;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Produto atualizado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (productId == 0)
            {
                return BadRequest(new
                {
                    Message = "Erro, o id do produto não pode ser zero."
                });
            }

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    return NotFound(new
                    {
                        Message = "O produto não foi encontrado."
                    });
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "O produto foi devidamente excluído."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Um erro inesperado ocorreu. Confira o erro: {ex.Message}" });
            }            
        }
    }
}
