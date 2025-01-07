using Microsoft.AspNetCore.Authorization;
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
        public async Task<dynamic> CreateProduct([FromBody]Product product, [FromHeader] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new
                {
                    message = "Houve um problema com o Token."
                });
            }
            if (product == null)
            {
                return BadRequest(new
                {
                    message = "O produto não pode estar vazio."
                });
            }

            try
            {
                var user = await TokenServices.GetTokenUserAsync(TokenServices.ValidateJwtToken(token), _context);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Nenhum usuário válido."
                    });
                }
                product.LastUpdaterId = user.Id;
                product.RecorderId = user.Id;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Produto cadastrado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet("{ProductId}")]
        public async Task<ActionResult<dynamic>> GetProduct([FromRoute] int ProductId)
        {
            if (ProductId == 0)
            {
                return BadRequest(new
                {
                    message = "Erro, o Id não pode ser vazio."
                });
            }
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                if (product == null)
                {
                    return NotFound(new
                    {
                        message = "Erro, o produto não foi encontrado."
                    });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromHeader]Product product, [FromHeader] string token)
        {
            if (product == null)
            {
                return BadRequest(new
                {
                    message = "O novo produto não pode ser nulo."
                });
            }

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new
                {
                    message = "O token não pode estar vazio."
                });
            }

            if (SWII6P2Verifications.Verifications.IsProductFull(product.Name, product.Price))
            {
                return BadRequest(new
                {
                    message = "O produto não está devidamente preenchido."
                });
            }

            try 
            {
                var user = await TokenServices.GetTokenUserAsync(TokenServices.ValidateJwtToken(token), _context);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Nenhum usuário válido."
                    });
                }

                var productToChange = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (productToChange == null)
                {
                    return NotFound(new
                    {
                        message = "Produto não encontrado."
                    });
                }

                productToChange.Price = product.Price;
                productToChange.Name = product.Name;
                productToChange.Status = product.Status;
                productToChange.LastUpdaterId = user.Id;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Produto atualizado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productId, [FromHeader] string token)
        {
            if (productId == 0)
            {
                return BadRequest(new
                {
                    message = "Erro, o id do produto não pode ser zero."
                });
            }

            try
            {
                var user = await TokenServices.GetTokenUserAsync(TokenServices.ValidateJwtToken(token), _context);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Nenhum usuário válido."
                    });
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    return NotFound(new
                    {
                        message = "O produto não foi encontrado."
                    });
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "O produto foi devidamente excluído."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Um erro inesperado ocorreu. Confira o erro: {ex.Message}" });
            }            
        }
    }
}
