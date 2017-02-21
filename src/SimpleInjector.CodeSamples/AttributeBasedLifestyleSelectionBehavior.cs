﻿namespace SimpleInjector.CodeSamples
{
    using System;
    using System.Reflection;
    using SimpleInjector.Advanced;

    public enum CreationPolicy { Transient, Scoped, Singleton }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        Inherited = false, AllowMultiple = false)]
    public sealed class CreationPolicyAttribute : Attribute
    {
        public CreationPolicyAttribute(CreationPolicy policy)
        {
            this.Policy = policy;
        }

        public CreationPolicy Policy { get; }
    }

    public class AttributeBasedLifestyleSelectionBehavior : ILifestyleSelectionBehavior
    {
        private const CreationPolicy DefaultPolicy = CreationPolicy.Transient;

        public Lifestyle SelectLifestyle(Type type) => ToLifestyle(GetPolicy(type));

        private static Lifestyle ToLifestyle(CreationPolicy policy) =>
            policy == CreationPolicy.Singleton ? Lifestyle.Singleton :
            policy == CreationPolicy.Scoped ? Lifestyle.Scoped :
            Lifestyle.Transient;

        private static CreationPolicy GetPolicy(Type type) =>
            type.GetCustomAttribute<CreationPolicyAttribute>()?.Policy ?? DefaultPolicy;
    }
}