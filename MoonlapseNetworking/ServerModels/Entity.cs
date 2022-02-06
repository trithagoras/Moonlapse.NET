using System;
using System.Collections.Generic;
using MoonlapseNetworking.ServerModels.Components;
using Newtonsoft.Json;

namespace MoonlapseNetworking.ServerModels
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }

        [JsonIgnore]
        public readonly Dictionary<Type, Component> Components;

        public Entity()
        {
            Components = new Dictionary<Type, Component>();
        }

        public T? GetComponent<T>() where T : Component
        {
            try
            {
                var component = Components[typeof(T)];
                return (T)component;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public T AddComponent<T>() where T : Component, new()
        {
            if (GetComponent<T>() != null)
            {
                throw new ComponentAlreadyAttachedException();
            }

            var component = new T();
            SetComponent<T>(component);
            return component;
        }

        public void SetComponent<T>(T component) where T : Component
        {
            Components[typeof(T)] = component;
        }

        public void SetComponent(Component component)
        {
            Components[component.GetType()] = component;
        }

        public T RemoveComponent<T>() where T : Component
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                throw new ComponentNotAttachedException();
            }

            Components.Remove(typeof(T));
            return component;
        }
    }

    public class ComponentAlreadyAttachedException : Exception
    {
    }

    public class ComponentNotAttachedException : Exception
    {
    }

    /// <summary>
    /// This class can be used to instantiate objects
    /// </summary>
    public static class EntityTemplates
    {
        public static Entity MakePlayer()
        {
            var e = new Entity()
            {
                TypeName = "Player"
            };
            e.AddComponent<Position>();

            return e;
        }
    }
}
