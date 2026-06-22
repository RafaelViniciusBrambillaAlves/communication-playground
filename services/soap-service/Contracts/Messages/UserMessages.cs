using System.Data;
using System.Runtime.Serialization;

namespace SoapService.Contracts.Messages
{
    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class UserData
    {
        [DataMember(Order = 1)] public Guid Id { get; set; }
        [DataMember(Order = 2)] public string Name { get; set; } = string.Empty;
        [DataMember(Order = 3)] public string Email { get; set; } = string.Empty;
        [DataMember(Order = 4)] public int Age { get; set; }
        [DataMember(Order = 5)] public DateTime CreatedAt { get; set; }
        [DataMember(Order = 6)] public DateTime? UpdatedAt { get; set; }
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class CreateUserRequest
    {
        [DataMember(Order = 1)] public string Name { get; set; } = string.Empty;
        [DataMember(Order = 2)] public string Email { get; set; } = string.Empty;
        [DataMember(Order = 3)] public int Age { get; set; }  
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class CreateUserResponse
    {
        [DataMember(Order = 1)] public UserData User { get; set; } = null!;
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class GetUserRequest
    {
        [DataMember(Order = 1)] public Guid Id { get; set; }
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class GetUserResponse
    {
        [DataMember(Order = 1)] public UserData User { get; set; } = null!;
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class GetAllUsersRequest { }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class GetAllUsersResponse
    {
        [DataMember(Order = 1)] public List<UserData> Users { get; set; } = [];
    }
    
    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class UpdateUserRequest
    {
        [DataMember(Order = 1)] public Guid Id { get; set; }
        [DataMember(Order = 2)] public string? Name { get; set; }
        [DataMember(Order = 3)] public string? Email { get; set; }
        [DataMember(Order = 4)] public int? Age { get; set; }
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class UpdateUserResponse
    {
        [DataMember(Order = 1)] public UserData User { get; set; } = null!;
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class DeleteUserRequest
    {
        [DataMember(Order = 1)] public Guid Id { get; set; }
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user")]
    public sealed class DeleteUserResponse
    {
        [DataMember(Order = 1)] public bool Success { get; set; }
    }
}

