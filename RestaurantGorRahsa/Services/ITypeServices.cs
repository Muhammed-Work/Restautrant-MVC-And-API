using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RestaurantGorRahsa.Services
{
    public interface ITypeServices
    {
        public List<Models.ModelType> GetAllType();
        public Models.ModelType GetModelType(int id);
        public bool Save(Models.ModelType type);

        public bool Delete(int id);
    }

}



