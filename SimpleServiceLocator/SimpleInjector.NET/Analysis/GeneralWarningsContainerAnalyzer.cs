﻿#region Copyright (c) 2013 S. van Deursen
/* The Simple Injector is an easy-to-use Inversion of Control library for .NET
 * 
 * Copyright (C) 2013 S. van Deursen
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ or mail to steven at 
 * cuttingedge.it.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial 
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT 
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO 
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace SimpleInjector.Analysis
{
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class GeneralWarningsContainerAnalyzer : List<IContainerAnalyzer>, IContainerAnalyzer
    {
        internal GeneralWarningsContainerAnalyzer()
        {
            this.Add(new PotentialLifestyleMismatchContainerAnalyzer());
            this.Add(new ContainerRegisteredContainerAnalyzer());
            this.Add(new ShortCircuitContainerAnalyzer());
        }

        public DebuggerViewItem Analyse(Container container)
        {
            const string WarningsName = "Configuration Warnings";

            var analysisResults = (
                from analyzer in this
                let result = analyzer.Analyse(container)
                where result != null
                select result)
                .ToArray();

            if (!analysisResults.Any())
            {
                return new DebuggerViewItem(WarningsName, "No warnings detected.");
            }
            else if (analysisResults.Length == 1)
            {
                return analysisResults.Single();
            }
            else
            {
                return new DebuggerViewItem(WarningsName, "Warnings in multiple groups have been detected.",
                    analysisResults);
            }
        }
    }
}