using System;
using System.Collections.Generic;

#nullable disable

namespace ModelClient
{
    [Serializable]
    public partial class MainTable
    {
        public MainTable()
        {
            SecondaryTables = new HashSet<SecondaryTable>();
        }

        public Guid Id { get; set; }
        public string Descripton { get; set; }

        public virtual ICollection<SecondaryTable> SecondaryTables { get; set; }
    }
}
