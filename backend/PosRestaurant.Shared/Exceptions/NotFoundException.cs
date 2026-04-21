using System;

namespace PosRestaurant.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Encja \"{name}\" o kluczu ({key}) nie została znaleziona.")
        {
        }
    }
}