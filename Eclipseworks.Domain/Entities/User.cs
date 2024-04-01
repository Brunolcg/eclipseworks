namespace Eclipseworks.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; private set; } = null!;

    protected User()
    {
    }
    
    public User(string name)
        => Name = name;
}