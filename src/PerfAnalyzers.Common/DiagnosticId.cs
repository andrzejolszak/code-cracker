namespace PerformanceAllocationAnalyzers
{
    public enum DiagnosticId
    {
        None = 0,
        EmptyFinalizer = 25,
        MakeLocalVariableConstWhenItIsPossible = 30,
        RemoveWhereWhenItIsPossible = 11,
        SealedAttribute = 23,
        StringBuilderInLoop = 39,
        ForInArray = 6,
        UseStaticRegexIsMatch = 81
    }
}