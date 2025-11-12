using System.Collections.Generic; // For Comparer<T>
using System.Linq.Expressions;

namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Interface for filter operators that can be applied to IQueryable sources.
/// </summary>
public interface IFilterOperator<TValue>
{
    string Name { get; }
    string Symbol { get; }
    IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue);
}

/// <summary>
/// Base class for filter operators with common functionality.
/// </summary>
public abstract class FilterOperatorBase<TValue> : IFilterOperator<TValue>
{
    public abstract string Name { get; }
    public abstract string Symbol { get; }

    public abstract IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue);

    protected Expression<Func<TItem, bool>> BuildPredicate<TItem>(
        Expression<Func<TItem, TValue>> propertyExpression,
        Expression<Func<TValue, bool>> valueCondition)
    {
        var parameter = propertyExpression.Parameters[0];
        var propertyAccess = propertyExpression.Body;
        
        // Replace parameter in value condition with property access
        var visitor = new ParameterReplacementVisitor(
            valueCondition.Parameters[0], propertyAccess);
        var newBody = visitor.Visit(valueCondition.Body);
        
        return Expression.Lambda<Func<TItem, bool>>(newBody, parameter);
    }

    private class ParameterReplacementVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly Expression _newExpression;

        public ParameterReplacementVisitor(ParameterExpression oldParameter, Expression newExpression)
        {
            _oldParameter = oldParameter;
            _newExpression = newExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newExpression : base.VisitParameter(node);
        }
    }
}

// String Filter Operators
public class StringEqualsOperator : FilterOperatorBase<string>
{
    public override string Name => "Equals";
    public override string Symbol => "=";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, string>> propertyExpression,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return source;

        var predicate = BuildPredicate(propertyExpression, 
            value => value != null && value.Equals(filterValue, StringComparison.OrdinalIgnoreCase));
        
        return source.Where(predicate);
    }
}

public class StringContainsOperator : FilterOperatorBase<string>
{
    public override string Name => "Contains";
    public override string Symbol => "⊇";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, string>> propertyExpression,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return source;

        var predicate = BuildPredicate(propertyExpression,
            value => value != null && value.Contains(filterValue, StringComparison.OrdinalIgnoreCase));
        
        return source.Where(predicate);
    }
}

public class StringStartsWithOperator : FilterOperatorBase<string>
{
    public override string Name => "Starts With";
    public override string Symbol => "⊐";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, string>> propertyExpression,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return source;

        var predicate = BuildPredicate(propertyExpression,
            value => value != null && value.StartsWith(filterValue, StringComparison.OrdinalIgnoreCase));
        
        return source.Where(predicate);
    }
}

public class StringEndsWithOperator : FilterOperatorBase<string>
{
    public override string Name => "Ends With";
    public override string Symbol => "⊏";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, string>> propertyExpression,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return source;

        var predicate = BuildPredicate(propertyExpression,
            value => value != null && value.EndsWith(filterValue, StringComparison.OrdinalIgnoreCase));
        
        return source.Where(predicate);
    }
}

// Numeric Filter Operators
public class NumericEqualsOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Equals";
    public override string Symbol => "=";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) == 0);
        
        return source.Where(predicate);
    }
}

public class NumericNotEqualsOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Not Equals";
    public override string Symbol => "≠";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) != 0);
        
        return source.Where(predicate);
    }
}

public class NumericGreaterThanOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Greater Than";
    public override string Symbol => ">";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) > 0);
        
        return source.Where(predicate);
    }
}

public class NumericGreaterThanOrEqualOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Greater Than or Equal";
    public override string Symbol => "≥";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) >= 0);
        
        return source.Where(predicate);
    }
}

public class NumericLessThanOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Less Than";
    public override string Symbol => "<";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) < 0);
        
        return source.Where(predicate);
    }
}

public class NumericLessThanOrEqualOperator<TValue> : FilterOperatorBase<TValue>
{
    public override string Name => "Less Than or Equal";
    public override string Symbol => "≤";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        var comparer = Comparer<TValue>.Default;
        var predicate = BuildPredicate(propertyExpression,
            value => comparer.Compare(value, filterValue) <= 0);
        
        return source.Where(predicate);
    }
}

public class NumericBetweenOperator<TValue> : IFilterOperator<TValue>
{
    public string Name => "Between";
    public string Symbol => "↔";
    
    public TValue MinValue { get; set; } = default!;
    public TValue MaxValue { get; set; } = default!;

    public IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, TValue>> propertyExpression,
        TValue filterValue)
    {
        // For Between operator, we use MinValue and MaxValue instead of filterValue
        var parameter = propertyExpression.Parameters[0];
        var propertyAccess = propertyExpression.Body;
        
        // Build: value >= MinValue && value <= MaxValue
        var minConstant = Expression.Constant(MinValue, typeof(TValue));
        var maxConstant = Expression.Constant(MaxValue, typeof(TValue));
        
        var greaterThanMin = Expression.GreaterThanOrEqual(propertyAccess, minConstant);
        var lessThanMax = Expression.LessThanOrEqual(propertyAccess, maxConstant);
        var combined = Expression.AndAlso(greaterThanMin, lessThanMax);
        
        var predicate = Expression.Lambda<Func<TItem, bool>>(combined, parameter);
        
        return source.Where(predicate);
    }
}

// DateTime Filter Operators
public class DateEqualsOperator : FilterOperatorBase<DateTime>
{
    public override string Name => "On Date";
    public override string Symbol => "=";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, DateTime>> propertyExpression,
        DateTime filterValue)
    {
        var predicate = BuildPredicate(propertyExpression,
            value => value.Date == filterValue.Date);
        
        return source.Where(predicate);
    }
}

public class DateAfterOperator : FilterOperatorBase<DateTime>
{
    public override string Name => "After";
    public override string Symbol => ">";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, DateTime>> propertyExpression,
        DateTime filterValue)
    {
        var predicate = BuildPredicate(propertyExpression,
            value => value.Date > filterValue.Date);
        
        return source.Where(predicate);
    }
}

public class DateBeforeOperator : FilterOperatorBase<DateTime>
{
    public override string Name => "Before";
    public override string Symbol => "<";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, DateTime>> propertyExpression,
        DateTime filterValue)
    {
        var predicate = BuildPredicate(propertyExpression,
            value => value.Date < filterValue.Date);
        
        return source.Where(predicate);
    }
}

// Boolean Filter Operator
public class BooleanEqualsOperator : FilterOperatorBase<bool>
{
    public override string Name => "Is";
    public override string Symbol => "=";

    public override IQueryable<TItem> Apply<TItem>(
        IQueryable<TItem> source,
        Expression<Func<TItem, bool>> propertyExpression,
        bool filterValue)
    {
        var predicate = BuildPredicate(propertyExpression,
            value => value == filterValue);
        
        return source.Where(predicate);
    }
}
