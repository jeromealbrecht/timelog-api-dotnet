using Microsoft.AspNetCore.Mvc;
using TimeLog.Models;
using TimeLog.Services;

namespace TimeLog.Controllers;

// contrôleur pour les utilisateurs
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // service pour les utilisateurs
    private readonly UserService _userService;

    // constructeur pour le contrôleur  
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // route pour récupérer tous les utilisateurs
    [HttpGet]
    public ActionResult<List<User>> Get() => _userService.Get();

    // route pour récupérer un utilisateur par son id
    [HttpGet("{id}")]
    public ActionResult<User> Get(string id)
    {
        var user = _userService.Get(id);
        if (user == null) return NotFound();
        return user;
    }

    // route pour créer un utilisateur
    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        _userService.Create(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    // route pour mettre à jour un utilisateur
    [HttpPut("{id}")]
    public IActionResult Update(string id, User userIn)
    {
        var user = _userService.Get(id);
        if (user == null) return NotFound();
        _userService.Update(id, userIn);
        return NoContent();
    }

    // route pour supprimer un utilisateur
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var user = _userService.Get(id);
        if (user == null) return NotFound();
        _userService.Remove(id);
        return NoContent();
    }

    [HttpPost("login")]
    public ActionResult<User> Login([FromBody] LoginRequest request)
    {
        var user = _userService.Get().FirstOrDefault(u => u.Email == request.Email);
        if (user == null)
            return Unauthorized("Utilisateur non trouvé");

        if (!_userService.VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Mot de passe incorrect");

        // Ici, tu pourrais générer un JWT et le retourner
        return Ok(user);
    }

    [HttpGet("me")]
    public ActionResult<User> Me()
    {
        // En vrai, il faudrait utiliser l'identité de l'utilisateur connecté (JWT)
        // Ici, on retourne le premier utilisateur pour l'exemple
        var user = _userService.Get().FirstOrDefault();
        if (user == null)
            return NotFound();
        return Ok(user);
    }
}