namespace LexEngine;

public static class Patterns
{
    public static readonly string DigitRegex = "[0-9]";
    public static readonly string LetterRegex = $"[\u0620-\u063A]|[\u0641-\u064A]";
    public static readonly string NumberRegex = @"^(0|[1-9][0-9]*)(\.0|(\.[0-9]*[1-9]))?$";
    public static readonly string LiteralRegex = @$"^({LetterRegex})({LetterRegex}|{DigitRegex}|(_))*$";
    public static readonly string WordTerminalRegex = @"[();؛×،{}\r\n !<>@#$%&÷×؟‘*--+=.,/\`~'"":\\\[\]\|\?\^]";
    public static readonly string NumberTerminalRegex = @"[();؛×،{} !@#$%&*-+=/\`~'"":\\\[\]\?\^]";
}