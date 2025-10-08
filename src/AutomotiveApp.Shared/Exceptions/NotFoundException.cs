using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Shared.Exceptions
{
    public class NotFoundException<T> : Exception
    {
        public NotFoundException(Guid id)
            : base($"{typeof(T).Name} with Id {id} was not found")
        { }

        public NotFoundException(string message)
            : base($"{typeof(T).Name} {message}")
        { }
    }
}