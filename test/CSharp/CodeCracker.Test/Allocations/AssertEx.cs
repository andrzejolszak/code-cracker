﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;

namespace ClrHeapAllocationsAnalyzer.Test
{
    public static class AssertEx
    {
        public static void ContainsDiagnostic(List<Diagnostic> diagnostics, string id, int line, int character)
        {
            var msg = string.Format("\r\nExpected {0} at ({1},{2}), i.e. line {1}, at character position {2})\r\nDiagnostics:\r\n{3}\r\n",
                                    id, line, character, string.Join("\r\n", diagnostics));
            Assert.Equal(1,
                            diagnostics.Count(d =>
                                d.Id == id &&
                                d.Location.GetLineSpan().StartLinePosition.Line + 1 == line &&
                                d.Location.GetLineSpan().StartLinePosition.Character + 1 == character));
        }
    }
}