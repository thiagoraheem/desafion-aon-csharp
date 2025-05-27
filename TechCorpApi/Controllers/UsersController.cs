using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCorpApi.Data;
using TechCorpApi.Models;
using CsvHelper;
using System.Globalization;

namespace TechCorpApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Nome = updatedUser.Nome;
        user.Email = updatedUser.Email;
        user.Idade = updatedUser.Idade;

        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Arquivo inválido.");

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var users = csv.GetRecords<UserCsvDto>().ToList();

        foreach (var userDto in users)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (!exists)
                _context.Users.Add(new User
            {
                Nome = userDto.Nome,
                Email = userDto.Email,
                Idade = userDto.Idade
            });
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = $"Inseridos {users.Count} usuários com sucesso." });
    }
}
