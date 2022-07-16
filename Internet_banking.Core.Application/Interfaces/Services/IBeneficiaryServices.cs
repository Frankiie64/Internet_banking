using Internet_banking.Core.Application.ViewModels.Clients;
using Internet_banking.Core.Application.ViewModels.Clients.Beneficiary;
using Internet_banking.Core.Application.ViewModels.TypeAccount;
using Internet_banking.Core.Domain.Entities;


namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryServices : IGenericServices<SaveBeneficiaryVM, BeneficiaryVM, Beneficiary>
    {

    }
}
