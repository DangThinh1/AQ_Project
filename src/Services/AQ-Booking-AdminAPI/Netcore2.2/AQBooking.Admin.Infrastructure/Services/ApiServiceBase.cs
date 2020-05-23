using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class ApiServiceBase<TViewModel, TCreateModel, TUpdateModel, TEntity> :
       IApiServiceBase<TViewModel, TCreateModel, TUpdateModel, TEntity>
       where TViewModel : class, new()
       where TCreateModel : class, new()
       where TUpdateModel : class, new()
       where TEntity : class, new()
    {
        protected readonly IApiRepository<TEntity> _repository;
        protected ControllerBase _controller;

        public ApiServiceBase(IApiRepository<TEntity> repository)
        {
            _repository = repository;
        }

        protected Guid GetUserGuidId()
        {
            var guid = _controller.User.Identities.FirstOrDefault().FindFirst(ClaimTypes.NameIdentifier).Value;
            return new Guid(guid);
        }

        public void InitController(ControllerBase controller)
        {
            _controller = controller;
        }

        public async Task<IQueryable<TEntity>> GetAllQueryableAsync(bool includeDeleted = false)
        {
            try
            {
                return await _repository.GetAllQueryableAsync(includeDeleted);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<(List<TViewModel> List, PagingInfo PagingInfo)> SearchAsync(SearchModel searchModel)
        {
            var dataResult = await _repository.SearchEntitiesAsync(searchModel);

            return await Task.Run(() =>
            {
                var listViewModels = new List<TViewModel>();
                foreach (var entity in dataResult.Data)
                {
                    TViewModel model = new TViewModel();
                    model.InjectFrom(entity);
                    listViewModels.Add(model);
                }
                return (listViewModels, dataResult.PagingInfo);
            });
        }

        public async Task<List<TViewModel>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var listEntities = await _repository.GetAllEntitiesAsync(includeDeleted);

                return listEntities.ConvertToList<TViewModel>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual async Task<TViewModel> FindByIdAsync(int id, bool includeDeleted = false)
        {
            try
            {
                var entity = await _repository.FindEntityByIdAsync(id: id, includeDeleted: includeDeleted);
                if (entity == null)
                    return null;
                TViewModel model = new TViewModel();
                model.InjectFrom(entity);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<ApiActionResult> CreateRangeAsync(List<TCreateModel> models)
        {
            try
            {
                if (models == null)
                    return null;
                var userId = GetUserGuidId();
                List<TEntity> entities = new List<TEntity>();
                foreach (var model in models)
                {
                    string uniqueId = UniqueIDHelper.GenarateRandomString(12);
                    TEntity entity = new TEntity();
                    entity.InjectFrom(model);
                    entity.SetValueByNameIfExists("CreatedBy", userId);
                    entity.SetValueByNameIfExists("CreatedDate", DateTime.UtcNow);
                    entity.SetValueByNameIfExists("LastModifiedBy", userId);
                    entity.SetValueByNameIfExists("LastModifiedDate", DateTime.UtcNow);
                    entity.SetValueByNameIfExists("Deleted", false);
                    entity.SetValueByNameIfExists("UniqueId", uniqueId);
                    entities.Add(entity);
                }

                var result = await _repository.CreateEntitiesAsync(entities, userId);
                if (result.Succeeded)
                    return await ApiActionResult.SuccessAsync();
                return await ApiActionResult.FailedAsync("Error");
            }
            catch (Exception ex)
            {
                return await ApiActionResult.FailedAsync(ex.Message);
            }
        }

        public virtual async Task<ApiActionResult> CreateAsync(TCreateModel model)
        {
            try
            {
                if (model == null)
                    return null;
                var userId = GetUserGuidId();
                string uniqueId = UniqueIDHelper.GenarateRandomString(12);
                TEntity entity = new TEntity();
                entity.InjectFrom(model);
                entity.SetValueByNameIfExists("CreatedBy", userId);
                entity.SetValueByNameIfExists("CreatedDate", DateTime.UtcNow);
                entity.SetValueByNameIfExists("LastModifiedBy", userId);
                entity.SetValueByNameIfExists("LastModifiedDate", DateTime.UtcNow);
                entity.SetValueByNameIfExists("Deleted", false);
                entity.SetValueByNameIfExists("UniqueId", uniqueId);

                var result = await _repository.CreateEntityAsync(entity, userId);
                var viewModel = new TViewModel();
                viewModel.InjectFrom(result.ResponseData);
                return await ApiActionResult.SuccessAsync(viewModel);
            }
            catch (Exception ex)
            {
                return await ApiActionResult.FailedAsync(ex.Message);
            }
        }

        public virtual async Task<ApiActionResult> UpdateAsync(TUpdateModel model)
        {
            try
            {
                if (model == null)
                    return null;

                if (!model.HasProperty("Id"))
                    return await ApiActionResult.FailedAsync("Data not found.");
                var userid = GetUserGuidId();
                var id = model.GetValueByNameIfExists("Id");
                var entityDb = await _repository.FindEntityByIdAsync(id);
                if (entityDb == null)
                    return await ApiActionResult.FailedAsync("Data not found.");
                var ignoreProperties = new LoopInjection(new[] { "CreatedBy", "CreatedDate", "UniqueId", "UniqueCode", "Id" });

                entityDb.InjectFrom(ignoreProperties, model);
                entityDb.SetValueByNameIfExists("LastModifiedBy", userid);
                entityDb.SetValueByNameIfExists("LastModifiedDate", DateTime.UtcNow);

                entityDb.InjectFrom(model);
                return await _repository.UpdateEntityAsync(entityDb, userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<ApiActionResult> DeleteAsync(int id)
        {
            try
            {
                var userid = GetUserGuidId();
                return await _repository.DeleteEntityAsync(id, userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

