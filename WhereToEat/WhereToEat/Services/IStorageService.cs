using System.IO;

namespace WhereToEat.Controllers
{
    public interface IStorageService
    {
        string Save(string fileName, Stream stream);
        void Delete(string fileName);
    }
}