namespace Eclipseworks.Tests.Utils.ObjectMothers;

public static class UserMother
{
    public static User Create(string? name = null)
        => new(name: name ?? "Anonymous");
    
    public static User Admin()
        => new(name: "Admin");
}