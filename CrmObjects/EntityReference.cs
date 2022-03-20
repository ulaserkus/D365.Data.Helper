using System;
using System.Collections.Generic;
using System.Text;

namespace D365.Data.Helper.CrmObjects
{
    public class EntityReference
    {
        public Guid Id { get; set; }

        public string LogicalName { get; set; }

        public EntityReference(string Name)
        {
            this.LogicalName = Name;

            if (Id == null) Id = Guid.Empty; 
        }
    }
}
