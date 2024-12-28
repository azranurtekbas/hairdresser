﻿using Microsoft.AspNetCore.Mvc;
using haircaredeneme.Models;
using System.Linq;

namespace haircaredeneme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { error = "Geçersiz giriş bilgileri." });
            }

            var musteri = _context.Musteriler
                .FirstOrDefault(m => m.mail == loginRequest.Email && m.sifre == loginRequest.Password);

            if (musteri == null)
            {
                return Unauthorized(new { error = "Hatalı e-posta veya şifre." });
            }

            // Kullanıcıyı session'a kaydet
            HttpContext.Session.SetString("UserEmail", musteri.mail);
            HttpContext.Session.SetInt32("UserId", musteri.musteriId);

            return Ok(new { message = "Giriş başarılı." });
        }

     


        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}