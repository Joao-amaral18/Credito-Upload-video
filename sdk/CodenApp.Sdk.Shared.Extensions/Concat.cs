using System;
using System.Linq.Expressions;
using CodenApp.Sdk.Domain.Abstraction.Entities;

namespace CodenApp.Sdk.Shared.Extensions
{
    public static class Concat<EntityBase> where EntityBase : class
    {
        public static Expression<Func<EntityBase, bool>> ConcatExpression(
            Expression<Func<EntityBase, bool>> ex1,
            Expression<Func<EntityBase, bool>> ex2
        )
        {
            if (ex1 == null)
                return ex2;

            return Expression.Lambda<Func<EntityBase, bool>>(Expression.AndAlso(
                new SwapVisitor(ex1.Parameters[0], ex2.Parameters[0]).Visit(ex1.Body),
                ex2.Body), ex2.Parameters);
        }
    }

    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;
        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }
        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}