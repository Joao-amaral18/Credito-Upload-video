using System;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IUnitOfWork
{
    bool Commit(
            string entity,
            int usuarioId,
            object oldObject,
            object newObject
    );

    Task<bool> CommitAsync(
            string entity,
            int usuarioId,
            object oldObject,
            object newObject
    );
}
