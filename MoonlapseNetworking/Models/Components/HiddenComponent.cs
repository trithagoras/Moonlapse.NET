using System;
namespace MoonlapseNetworking.Models.Components
{
    /// <summary>
    /// A component with this attribute will not be included in the serialized
    /// packet when sending over network.
    ///
    /// <br></br>
    ///
    /// E.g. A Render component is useful to the client but useless to server
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class HiddenComponent : Attribute
    {
    }
}
