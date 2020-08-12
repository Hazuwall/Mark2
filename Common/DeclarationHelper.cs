using System;

namespace Common
{
    public static class DeclarationHelper
    {
        public static void FillStringStaticFieldsWithNames(Type classType, string prefix)
        {
            foreach (var field in classType.GetFields())
            {
                if (field.FieldType.Equals(typeof(string)) && field.IsStatic)
                {
                    field.SetValue(null, prefix + field.Name);
                }
            }
        }
    }
}
