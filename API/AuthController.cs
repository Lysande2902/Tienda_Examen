using Microsoft.AspNetCore.Mvc;
using System;

namespace API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string UsuarioFijo = "admin";
        private const string ContrasenaFija = "1234";
        private const string TokenFijo = "TOKEN_SUPER_SECRETO";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Usuario == UsuarioFijo && request.Contrasena == ContrasenaFija)
            {
                // Puedes usar un GUID o un token fijo
                return Ok(new { token = TokenFijo });
            }
            return Unauthorized("Usuario o contraseña incorrectos");
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
} 