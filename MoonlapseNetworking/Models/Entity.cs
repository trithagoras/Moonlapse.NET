using System;
using System.Collections.Generic;
using MoonlapseNetworking.Models.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MoonlapseNetworking.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string TypeName { get; set; }

        [JsonIgnore]
        [NotMapped]
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
            SetComponent(component);
            return component;
        }

        public void SetComponent(Component component)
        {
            component.Entity = this;
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
}
