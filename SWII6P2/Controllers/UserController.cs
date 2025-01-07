using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWII6P2.Models;
using SWII6P2.Services;
using SWII6P2Verifications;

namespace SWII6P2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest(new
                {
                    message = "O usuário não pode ser nulo"
                });
            }

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Usuário criado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            if (userId == 0)
            {
                return BadRequest(new
                {
                    message = "O id não pode ser zero."
                });
            }

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "O usuário não foi encontrado."
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado. Detalhes: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<List<User>> GetUser() => await _context.Users.ToListAsync();

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (user == null)
            {
                return BadRequest(new
                {
                    Message = "O usuário não pode retornar vazio."
                });
            }

            if (!SWII6P2Verifications.Verifications.IsUserFull(user.Name, user.Password))
            {
                return BadRequest(new
                {
                    message = "Erro, o usuário não está devidamente completo."
                });
            }

            try
            {
                var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                if (userToUpdate == null)
                {
                    return NotFound(new
                    {
                        message = "Usuário não encontrado."
                    });
                }

                userToUpdate.Name = user.Name;
                userToUpdate.Status = user.Status;
                userToUpdate.Password = user.Password;
                _context.SaveChanges();
                return Ok(new
                {
                    message = "Usuário atualizado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado, detalhes: {ex.Message}" });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest(new
                {
                    message = "O id não pode ser zero."
                });
            }

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "O usuário não foi encontrado."
                    });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Usuário deletado com sucesso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado, detalhes: {ex.Message}" });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]User user)
        {
            if (user == null)
            {
                return BadRequest(new
                {
                    message = "O usuário não pode ser nulo."
                });
            }


            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest(new
                {
                    message = "O usuário precisa estar com o nome preenchido corretamente."
                });
            }

            try
            {
                var loggingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
                if (loggingUser == null)
                {
                    return NotFound(new
                    {
                        message = $"O usuário {user.Name} não foi encontrado. Por favor, verifique se foi devidamente preenchido."
                    });
                }

                if (!loggingUser.Status)
                {
                    return Unauthorized(new
                    {
                        message = $"O usuário {user.Name} está inativo. Entre em contato com o suporte."
                    });
                }

                if (loggingUser.Password.Trim() != user.Password.Trim())
                {
                    return Unauthorized(new
                    {
                        message = $"O usuário ou a senha estão incorretos. Por favor, verifique as informações preenchidas."
                    });
                }

                var token = TokenServices.GenerateToken(loggingUser);

                return Ok(new { Token = token });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = $"Erro inesperado, detalhes: {ex.Message}" });
            }
        }
    }
}
