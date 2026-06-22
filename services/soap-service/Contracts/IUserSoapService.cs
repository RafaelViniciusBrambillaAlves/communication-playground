using System.ServiceModel;
using SoapService.Contracts.Faults;
using SoapService.Contracts.Messages;

namespace SoapService.Contracts
{
    [ServiceContract(Namespace = "http://communicationplayground.com/soap/user")]
    public interface IUserSoapService
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        Task<GetUserResponse> GetUser(GetUserRequest request);

        [OperationContract]
        Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        [FaultContract(typeof(ValidationFault))]
        Task<UpdateUserResponse> UpdateUser (UpdateUserRequest request);

        [OperationContract]
        [FaultContract(typeof(UserNotFoundFault))]
        Task<DeleteUserResponse> DeleteUser (DeleteUserRequest request);
    }

}