using System;

namespace AQBooking.Admin.API.Extentions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name, object key)
        : base($"Entity '{name}' ({key}) was not found.")
        {
        }
    }
}
