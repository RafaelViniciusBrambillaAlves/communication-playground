using System.Runtime.Serialization;

namespace SoapService.Contracts.Faults
{
    [DataContract(Namespace = "http://communicationplayground.com/soap/user/faults")]
    public sealed class UserNotFoundFault
    {
        [DataMember(Order = 1)] public string Message { get; set; } = string.Empty;
        [DataMember(Order = 2)] public Guid UserId { get; set; }   
    }

    [DataContract(Namespace = "http://communicationplayground.com/soap/user/faults")]
    public sealed class ValidationFault
    {
        [DataMember(Order = 1)] public string Message { get; set; } = string.Empty;   
    }
}