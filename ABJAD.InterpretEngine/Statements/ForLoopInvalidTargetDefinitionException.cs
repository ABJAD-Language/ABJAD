namespace ABJAD.InterpretEngine.Statements;

public class ForLoopInvalidTargetDefinitionException : InterpretationException
{
    public ForLoopInvalidTargetDefinitionException() 
        : base("يمكنك فقط تعريف متغير أو إعطاء واحد قيمة جديدة داخل تعريف الهدف في حلقة التكرار.", 
            "Only variable declarations and assignments are allowed inside a for-loop target definition.")
    {
    }
}