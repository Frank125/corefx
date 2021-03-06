﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Xunit;

namespace System.Globalization.Tests
{
    public class CultureInfoTextInfo
    {
        [Fact]
        public void PosTest1()
        {
            string localeName = new CultureInfo("en-us").Name;
            Assert.Equal(localeName, new CultureInfo("en-us").TextInfo.CultureName);
        }

        [Fact]
        public void PosTest2()
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            string localeName = ci.Name;
            Assert.Equal(ci.TextInfo.CultureName, localeName);
        }

        [Fact]
        public void PosTest3()
        {
            CultureInfo myTestCulture = CultureInfo.InvariantCulture;
            Assert.Equal(myTestCulture.TextInfo.CultureName, myTestCulture.Name);
        }
    }
}
