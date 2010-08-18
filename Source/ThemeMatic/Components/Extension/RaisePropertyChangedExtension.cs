using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace ThemeMatic.Components.Extension
{
    public static class RaisePropertyChangedExtension
    {
        public static void Changed<TProperty>(this INotifyPropertyChanged self, Expression<Func<TProperty>> propertyExpresion, PropertyChangedEventHandler eventMethod)
        {
            var property = propertyExpresion.Body as MemberExpression;
            if (property == null || !(property.Member is PropertyInfo) ||
                !IsPropertyOfThis(property, self))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Expression must be of the form 'this.PropertyName'. Invalid expression '{0}'.", propertyExpresion), "propertyExpresion");
            }
            if (eventMethod != null)
            {
                eventMethod.Invoke(self, new PropertyChangedEventArgs(property.Member.Name));
            }
        }

        private static bool IsPropertyOfThis(MemberExpression property, INotifyPropertyChanged self)
        {
            var constant = RemoveCast(property.Expression) as ConstantExpression;
            return constant != null && constant.Value == self;
        }

        private static Expression RemoveCast(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert ||
                expression.NodeType == ExpressionType.ConvertChecked)
                return ((UnaryExpression)expression).Operand;

            return expression;
        }



    }
}
