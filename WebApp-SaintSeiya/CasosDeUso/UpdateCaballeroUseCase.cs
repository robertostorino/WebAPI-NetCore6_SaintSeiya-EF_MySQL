using WebApp_SaintSeiya.Dtos;
using WebApp_SaintSeiya.Repositories;

namespace WebApp_SaintSeiya.CasosDeUso
{

    // Interface: Es el contrato que va a tener una clase
    public interface IUpdateCaballeroUseCase
    {
        // Método de la interface
        // El "?" implica que nos puede devolver null
        Task<CaballeroDto?> Execute(CaballeroDto caballero);
    }

    public class UpdateCaballeroUseCase : IUpdateCaballeroUseCase
    {

        // Especifico la Base de Datos para poder inyectarla
        private readonly CaballerosDatabaseContext _caballerosDatabaseContext;

        public UpdateCaballeroUseCase(CaballerosDatabaseContext caballerosDatabaseContext)
        {
            _caballerosDatabaseContext = caballerosDatabaseContext;
        }

        public async Task<CaballeroDto?> Execute(CaballeroDto caballero)
        {
            var entity = await _caballerosDatabaseContext.Get(caballero.Id);

            if (entity == null)
                return null;

            //Si no es null, entonces actualizo
            entity.Nombre = caballero.Nombre;
            entity.Armadura = caballero.Armadura;
            entity.Categoria = caballero.Categoria;
            entity.Grupo = caballero.Grupo;

            await _caballerosDatabaseContext.Actualizar(entity);

            //Convierto la entidad a Dto
            return entity.ToDto();
        }
    }
}
