namespace Ecommerce.User.Contract
{
    public record UserCreated
    (
        Guid Id,
        string Name
    );

    public record UserUpdated
    (
        Guid Id,
        string Name
    );

    public record UserDeleted
    (
        Guid Id
    );
}