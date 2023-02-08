using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApp_SaintSeiya.Dtos;

namespace WebApp_SaintSeiya.Repositories
{
    public class CaballerosDatabaseContext : DbContext
    {

        // Va a ejecutar al constructor del padre
        public CaballerosDatabaseContext(DbContextOptions<CaballerosDatabaseContext> options) 
            : base(options)
        {

        } 
        
        
        // Entidad que hace mapeo 1 a 1 a nuestro objeto en la base de datos


        //DbSet hace referencia a las tablas en la base de datos
        public DbSet<CaballeroEntity> Caballeros { get; set; }

        // Tomo un elemento por id
        // Si el id no existe en la BD, entonces manda un error status 404
        public async Task<CaballeroEntity?> Get(int id)
        {
            return await Caballeros.FirstOrDefaultAsync(x => x.Id == id);
        }

        //Toma el caballero por su id, luego lo elimina, guarda los cambios y devuelve true
        public async Task<bool> Delete(int id)
        {
            CaballeroEntity entity = await Get(id);
            Caballeros.Remove(entity);
            SaveChanges();

            return true;
        }

        // Agrego un elemento
        public async Task<CaballeroEntity> Add(CreateCaballeroDto caballeroDto)
        {
            // Mapeo entre la entidad y el dto
            CaballeroEntity entity = new CaballeroEntity()
            {
                Id = null,
                Nombre = caballeroDto.Nombre,
                Armadura = caballeroDto.Armadura,
                Categoria = caballeroDto.Categoria,
                Grupo = caballeroDto.Grupo,

            };

            EntityEntry<CaballeroEntity> response = await Caballeros.AddAsync(entity);

            await SaveChangesAsync();  // SaveChangesAsync está dentro de DbContext

            // Si tiene un valor, devuelve el Get. Sino, entonces larga el error.
            return await Get(response.Entity.Id ?? throw new Exception("no se ha podido guardar"));

        }

        // Método Actualizar
        public async Task<bool> Actualizar(CaballeroEntity caballeroEntity)
        {
            Caballeros.Update(caballeroEntity);

            await SaveChangesAsync();

            return true;
        }


    }

    public class CaballeroEntity
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Armadura { get; set; }
        public string Categoria { get; set; }
        public string Grupo { get; set; }

        // mapper para convertir en DTO
        public CaballeroDto ToDto()
        {
            return new CaballeroDto()
            {
                Nombre = Nombre,
                Armadura = Armadura,
                Categoria = Categoria,
                Grupo = Grupo,
                Id = Id ?? throw new Exception("el id no puede ser null")
            };
        }

    }
}
