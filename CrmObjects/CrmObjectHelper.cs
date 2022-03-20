using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D365.Data.Helper.CrmObjects
{
    public class CrmObjectHelper
    {
        internal protected void ConvertToEntityReference(ref Dictionary<string, object> keyValues)
        {
            foreach (var item in  keyValues.ToList())
            {
                if (item.Key.Contains("_value"))
                {
                    Guid guid = Guid.Empty;

                    if(item.Value != null)
                    {
                        if (Guid.TryParse(item.Value.ToString(), out guid))
                        {
                            string tempKey = item.Key;
                            string newKey = item.Key.Remove(0, 1).Replace("_value", "");
                            EntityReference reference = new EntityReference(item.Key.Remove(0, 1));
                            reference.Id = guid;

                            keyValues.Remove(tempKey);
                            keyValues.Add(newKey, reference);
                        }
                    }
                   
                }
            }
        }

        internal protected void ConvertToOData(ref Dictionary<string, object> keyValues)
        {
            Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();

            foreach (var item in keyValues.ToList())
            {
                if (item.Value.GetType() == typeof(EntityReference))
                {
                    string tempKey = item.Key;
                    EntityReference reference = (EntityReference)Convert.ChangeType(item.Value, typeof(EntityReference));
                    string newKey = item.Key + "@odata.bind";
                    string newvalue = $"/{pluralizer.Pluralize(reference.LogicalName)}({reference.Id})";
                    keyValues.Remove(tempKey);
                    keyValues.Add(newKey, newvalue);
                }
            }

        }

        internal protected void CleanNullValues(ref Dictionary<string, object> keyValues)
        {

            foreach (var item in keyValues.ToList())
            {
                if (item.Value == null)
                {
                    keyValues.Remove(item.Key);
                }
            }

        }

    }
}
