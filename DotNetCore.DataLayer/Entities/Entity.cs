using DotNetCore.DataLayer.Attributes;
using System;

namespace DotNetCore.DataLayer.Entities
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public void Dispose() { }
    }
}
