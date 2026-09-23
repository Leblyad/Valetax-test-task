using System.Linq.Expressions;

namespace Users.Application.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    void Create(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    IQueryable<TEntity> FindAll(bool trackChanges);

    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges);
}
