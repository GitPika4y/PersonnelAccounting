using System.Linq.Expressions;

namespace Data.Extensions;

public static class ExpressionExtensions
{
    public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression memberExp)
            return memberExp.Member.Name;

        if (expression.Body is UnaryExpression unaryExp && unaryExp.Operand is MemberExpression memberOperand)
            return memberOperand.Member.Name;

        throw new ArgumentException("Expression must be a member expression or a unary expression.", nameof(expression));
    }

    public static MemberExpression GetMemberExpression<T>(this Expression<Func<T, object>> expression)
    {
        // Убираем Convert (для value types)
        var body = expression.Body;
        if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
        {
            body = unary.Operand;
        }

        return (MemberExpression)body;
    }
}