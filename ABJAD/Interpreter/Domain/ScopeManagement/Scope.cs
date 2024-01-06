namespace ABJAD.Interpreter.Domain.ScopeManagement;

public record Scope(IReferenceScope ReferenceScope, IFunctionScope FunctionScope, ITypeScope TypeScope);