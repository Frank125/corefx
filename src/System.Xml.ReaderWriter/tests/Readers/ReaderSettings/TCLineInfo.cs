﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;
using OLEDB.Test.ModuleCore;
using XmlReaderTest.Common;

namespace XmlReaderTest.ReaderSettingsTest
{
    public partial class TCLineInfo : TCXMLReaderBaseGeneral
    {
        // Type is XmlReaderTest.ReaderSettingsTest.TCLineInfo
        // Test Case
        public override void AddChildren()
        {
            // for function ln02
            {
                this.AddChild(new CVariation(ln02) { Attribute = new Variation("Line Number Offset negative values") { Pri = 1 } });
            }


            // for function lp02
            {
                this.AddChild(new CVariation(lp02) { Attribute = new Variation("Line Position Offset negative values") { Pri = 1 } });
            }
        }
    }
}
