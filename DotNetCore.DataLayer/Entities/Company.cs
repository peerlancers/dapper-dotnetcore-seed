using DotNetCore.DataLayer.Attributes;

namespace DotNetCore.DataLayer.Entities
{
    [DbTableName("companies")]
    public class Company : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
