namespace BlackJackGame.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public string? HashedPassword { get; set; }
    public string? Email { get; set; }
    public decimal Balance { get; set; }
}