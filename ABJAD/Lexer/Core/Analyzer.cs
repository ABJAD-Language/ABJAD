namespace ABJAD.Lexer.Core;

public interface Analyzer
{
    List<LexicalToken> AnalyzeCode(string code);
}