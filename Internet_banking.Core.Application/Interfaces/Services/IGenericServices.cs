using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface IGenericServices<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        Task<bool> UpdateAsync(SaveViewModel vm, int id);

        Task<SaveViewModel> CreateAsync(SaveViewModel vm);

        Task<SaveViewModel> DeleteAsync(int id);

        Task<SaveViewModel> GetByIdSaveViewModelAsync(int id);

        Task<List<ViewModel>> GetAllViewModelAsync();
    }
}
