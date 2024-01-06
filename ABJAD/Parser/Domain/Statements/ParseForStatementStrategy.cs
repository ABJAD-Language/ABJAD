using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Statements;

public class ParseForStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly IBindingFactory bindingFactory;
    private readonly IStatementStrategyFactory statementStrategyFactory;

    public ParseForStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
        IBindingFactory bindingFactory, IStatementStrategyFactory statementStrategyFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        Guard.Against.Null(bindingFactory);
        Guard.Against.Null(statementStrategyFactory);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
        this.bindingFactory = bindingFactory;
        this.statementStrategyFactory = statementStrategyFactory;
    }

    public Statement Parse()
    {
        tokenConsumer.Consume(TokenType.FOR);
        tokenConsumer.Consume(TokenType.OPEN_PAREN);

        var target = GetTargetBinding();

        var condition = statementStrategyFactory.Get().Parse();

        var targetCallback = expressionParser.Parse();

        tokenConsumer.Consume(TokenType.CLOSE_PAREN);

        var body = statementStrategyFactory.Get().Parse();

        return new ForStatement(target, condition, targetCallback, body);
    }

    private Binding GetTargetBinding()
    {
        var binding = bindingFactory.Get();
        if (binding is DeclarationBinding { Declaration: VariableDeclaration { Value: null } })
        {
            throw new ArgumentNullException();
        }

        if (binding is not StatementBinding { Statement: AssignmentStatement } && binding is not DeclarationBinding
            {
                Declaration: VariableDeclaration
            })
        {
            throw new ArgumentException();
        }

        return binding;
    }
}