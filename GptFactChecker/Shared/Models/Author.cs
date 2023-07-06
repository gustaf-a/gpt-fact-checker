namespace Shared.Models;

public class Author
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsSystem { get; set; } = false;
    public bool IsVerified { get; set; }
}
