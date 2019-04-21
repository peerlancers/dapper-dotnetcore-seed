using System;

namespace DotNetCore.DataLayer
{
    public interface IEntity : IDisposable
    {
        Guid Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? LastUpdatedOn { get; set; }
    }
}
