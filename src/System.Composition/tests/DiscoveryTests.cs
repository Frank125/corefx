﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Composition.Convention;
using System.Composition.Runtime;
using System.Composition.UnitTests.Util;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Composition.Hosting;
using Xunit;

namespace System.Composition.UnitTests
{
    public class DiscoveryTests : ContainerTests
    {
        public interface IRule { }

        public class RuleExportAttribute : ExportAttribute
        {
            public RuleExportAttribute() : base(typeof(IRule)) { }
        }

        [RuleExport]
        public class UnfairRule : IRule { }

        [Export(typeof(IRule))]
        public class IncompatibleRule { }

        public class IncompatibleRuleProperty
        {
            [Export(typeof(IRule))]
            public string Rule { get; set; }
        }

        [Export, PartNotDiscoverable]
        public class NotDiscoverable { }

        [Fact]
        public void DiscoversCustomExportAttributes()
        {
            var container = CreateContainer(typeof(UnfairRule));
            var rule = container.GetExport<IRule>();
            Assert.IsAssignableFrom(typeof(UnfairRule), rule);
        }

        [Fact]
        public void DiscoversCustomExportAttributesUnderConventions()
        {
            var container = CreateContainer(new ConventionBuilder(), typeof(UnfairRule));
            var rule = container.GetExport<IRule>();
            Assert.IsAssignableFrom(typeof(UnfairRule), rule);
        }

        [Fact]
        public void InstanceExportsOfIncompatibleContractsAreDetected()
        {
            var x = AssertX.Throws<CompositionFailedException>(() => CreateContainer(typeof(IncompatibleRule)));
            Assert.Equal("Exported contract type 'IRule' is not assignable from part 'IncompatibleRule'.", x.Message);
        }

        [Fact]
        public void PropertyExportsOfIncompatibleContractsAreDetected()
        {
            var x = AssertX.Throws<CompositionFailedException>(() => CreateContainer(typeof(IncompatibleRuleProperty)));
            Assert.Equal("Exported contract type 'IRule' is not assignable from property 'Rule' of part 'IncompatibleRuleProperty'.", x.Message);
        }

        [Fact]
        public void ANonDiscoverablePartIsIgnored()
        {
            var container = CreateContainer(typeof(NotDiscoverable));
            NotDiscoverable unused;
            Assert.False(container.TryGetExport(null, out unused));
        }

        public interface IBus { }

        [Export(typeof(IBus))]
        public class CloudBus : IBus { }

        public class SpecialCloudBus : CloudBus { }

        [Fact]
        public void DoesNotDiscoverExportAttributesFromBase()
        {
            var container = CreateContainer(typeof(SpecialCloudBus));

            IBus bus;
            Assert.False(container.TryGetExport(null, out bus));
        }

        public abstract class BaseController
        {
            [Import]
            public IBus Bus { get; set; }
        }

        [Export]
        public class HomeController : BaseController
        {
        }

        [Fact]
        public void SatisfiesImportsAppliedToBase()
        {
            var container = CreateContainer(typeof(HomeController), typeof(CloudBus));
            var hc = container.GetExport<HomeController>();
            Assert.IsAssignableFrom(typeof(CloudBus), hc.Bus);
        }

        private class CustomImportAttribute : ImportAttribute { }

        [Export]
        public class MultipleImportsOnProperty
        {
            [Import, CustomImport]
            public string MultiImport { get; set; }
        }

        [Fact]
        public void MultipleImportAttributesAreDetected()
        {
            var c = new ContainerConfiguration()
                .WithPart<MultipleImportsOnProperty>()
                .CreateContainer();

            var x = AssertX.Throws<CompositionFailedException>(() => c.GetExport<MultipleImportsOnProperty>());
            Assert.Equal("Multiple imports have been configured for 'MultiImport'. At most one import can be applied to a single site.", x.Message);
        }
    }
}
