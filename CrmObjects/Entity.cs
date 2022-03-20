using System;
using System.Collections.Generic;
using System.Text;

namespace D365.Data.Helper.CrmObjects
{
    public class Entity:CrmObjectHelper
    {   
        private Guid _id = Guid.Empty;

        public Guid Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                if (LogicalName == "letter" || LogicalName == "task" || LogicalName == "appointment" || LogicalName == "phonecall" || LogicalName == "email" || LogicalName == "fax")
                {
                    if (!Attributes.TryGetValue("activityid", out object Value))
                        Attributes.Add("activityid", value);
                }
                else
                {
                    if (!Attributes.TryGetValue(LogicalName + "id", out object Value))
                        Attributes.Add(LogicalName + "id", value);
                }
            }
        }

        public string LogicalName { get; set; }

        public Entity(string Name)
        {
            this.LogicalName = Name;
        }

        public Dictionary<string, object> Attributes = new Dictionary<string, object>();

        public object this[string Key]
        {
            get
            {
                object Value;

                if (Attributes.TryGetValue(Key, out Value))
                {
                    return Value;
                }

                return null;
            }
            set
            {
                if (!Attributes.TryGetValue(Key, out object Value))
                {
                    Attributes.Add(Key, value);
                }
            }
        }


    }



}
