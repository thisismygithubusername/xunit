﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
    /// <summary>
    /// Reflection-based implementation of <see cref="IReflectionTypeInfo"/>.
    /// </summary>
    public class ReflectionTypeInfo : LongLivedMarshalByRefObject, IReflectionTypeInfo
    {
        const BindingFlags publicBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        const BindingFlags nonPublicBindingFlags = BindingFlags.NonPublic | publicBindingFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionTypeInfo"/> class.
        /// </summary>
        /// <param name="type">The type to wrap.</param>
        public ReflectionTypeInfo(Type type)
        {
            Type = type;
        }

        /// <inheritdoc/>
        public IAssemblyInfo Assembly
        {
            get { return Reflector.Wrap(Type.Assembly); }
        }

        /// <inheritdoc/>
        public ITypeInfo BaseType
        {
            get { return Reflector.Wrap(Type.BaseType); }
        }

        /// <inheritdoc/>
        public IEnumerable<ITypeInfo> Interfaces
        {
            get { return Type.GetInterfaces().Select(Reflector.Wrap).Cast<ITypeInfo>().ToList(); }
        }

        /// <inheritdoc/>
        public bool IsAbstract
        {
            get { return Type.IsAbstract; }
        }

        /// <inheritdoc/>
        public bool IsSealed
        {
            get { return Type.IsSealed; }
        }

        /// <inheritdoc/>
        public string Name
        {
            get { return Type.FullName; }
        }

        /// <inheritdoc/>
        public Type Type { get; private set; }

        /// <inheritdoc/>
        public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
        {
            return ReflectionAttributeInfo.GetCustomAttributes(Type, assemblyQualifiedAttributeTypeName).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<IMethodInfo> GetMethods(bool includePrivateMethods)
        {
            return Type.GetMethods(includePrivateMethods ? nonPublicBindingFlags : publicBindingFlags)
                       .Select(Reflector.Wrap)
                       .Cast<IMethodInfo>()
                       .ToList();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}