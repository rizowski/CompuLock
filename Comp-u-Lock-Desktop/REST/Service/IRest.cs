using System.Collections.Generic;

namespace REST.Service
{
    interface IRest<T>
    {
        T Create(string token, T item);

        IEnumerable<T> GetAll(string token);

        T GetOneById(string token, int id);

        T Update(string token, T item);

        void Destroy(string token, int id);
    }
}
