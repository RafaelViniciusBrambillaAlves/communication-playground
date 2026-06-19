namespace GrpcService.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public int Age { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }


    private User() {}

    public static User Create(string name, string email, int age)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidateAge(age);

        return new User
        {
            Name = name.Trim(),
            Email = email.Trim().ToLower(),
            Age = age,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string? name, string? email, int? age)
    {
        if (name is not null)
        {
            ValidateName(name);
            Name = name.Trim();
        }

        if (email is not null)
        {
            ValidateEmail(email);
            Email = email.Trim().ToLower();
        }

        if (age is not null)
        {
            ValidateAge(age.Value);
            Age = age.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }


    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 2 || name.Trim().Length > 100)
            throw new ArgumentException("Name must be between 2 and 100 characters", nameof(name));
    }
    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));
    }
    private static void ValidateAge(int age)
    {
        if (age < 18)
            throw new ArgumentException("User must be at least 18 years old", nameof(age));
    }
}