using Internet_banking.Core.Application.ViewModels.Transational;
using Internet_banking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Interfaces.Services
{
    public interface ITrasantionalService : IGenericServices<SaveTransationalVM, TransationalVM, Transactional>
    {
        Task<SaveTransationalVM> GetByDateTrasations();
        Task<TransationalVM> GetTransationalToday();
    }
}
