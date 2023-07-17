namespace Shared.Models;

public class Author
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsSystem { get; set; } = false;
    public bool IsVerified { get; set; } = false;
}
