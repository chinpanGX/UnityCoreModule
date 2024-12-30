using System;

namespace ScreenSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AssetNameAttribute : Attribute
    {

        public AssetNameAttribute(string prefabName)
        {
            PrefabName = prefabName;
        }
        public string PrefabName { get; }
    }
}