﻿// This software is part of the Autofac IoC container
// Copyright (c) 2007 Nicholas Blumhardt
// nicholas.blumhardt@gmail.com
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;

namespace Autofac.Extras.Startable
{
    /// <summary>
    /// Can be used to instantiate an instance of all 'startable' services in
    /// a container.
    /// </summary>
    class Starter : IStarter
    {
        IContainer _container;

        /// <summary>
        /// Saved as an extended property to identify a component as startable.
        /// </summary>
        public const string IsStartablePropertyName = "Autofac.Extras.Startable.Starter.IsStartable";

        /// <summary>
        /// Initializes a new instance of the <see cref="Starter"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Starter(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            
            _container = container;
        }

        /// <summary>
        /// Start the startable components.
        /// </summary>
        public void Start()
        {
            var startableRegistrations =
                from cr in _container.ComponentRegistrations
                where cr.Descriptor.ExtendedProperties.ContainsKey(IsStartablePropertyName) &&
                    (bool)cr.Descriptor.ExtendedProperties[IsStartablePropertyName]
                select cr.Descriptor.Id;

            foreach (var startable in startableRegistrations)
                _container.Resolve(startable);
        }
    }
}