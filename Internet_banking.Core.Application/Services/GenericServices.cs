using AutoMapper;
using Internet_banking.Core.Application.Interfaces.Repositories;
using Internet_banking.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internet_banking.Core.Application.Services
{
    public class GenericServices<SaveViewModel, ViewModel, Model> : IGenericServices<SaveViewModel, ViewModel, Model>
         where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        private readonly IGenericRepository<Model> _repository;
        private readonly IMapper _mapper;

        public GenericServices(IGenericRepository<Model> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<bool> UpdateAsync(SaveViewModel vm, int id)
        {
            Model entity = _mapper.Map<Model>(vm);

            return await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task<SaveViewModel> CreateAsync(SaveViewModel vm)
        {
            Model entity = _mapper.Map<Model>(vm);

            entity = await _repository.createAsync(entity);

            SaveViewModel saveViewModel = _mapper.Map<SaveViewModel>(entity);
            return saveViewModel;
        }

        public virtual async Task<SaveViewModel> DeleteAsync(int id)
        {
            var model = await _repository.GetByIdAsync(id);
            bool value = await _repository.DeleteAsync(model);

            if (!value)
            {
                return _mapper.Map<SaveViewModel>(model);
            }
            return null;
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModelAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);
            return vm;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModelAsync()
        {
            var entityList = await _repository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entityList);
        }
    }
}
