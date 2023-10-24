using SQLite;
using System;

namespace Tongue.Models
{
    public class DbObject
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
