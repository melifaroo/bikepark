using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bikepark.Views.Helpers
{
    public static class HtmlX 
    {
        private static IHtmlContent MetaDataFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            Func<ModelMetadata, string> property)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));

            var modelExplorer = modelExpressionProvider.CreateModelExpression(html.ViewData, expression);
            if (modelExplorer == null) throw new InvalidOperationException($"Failed to get model explorer for {modelExpressionProvider.GetExpressionText(expression)}");

            return new HtmlString(property(modelExplorer.Metadata));
        }

        public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.MetaDataFor(expression, m => m.Description);
        }

        public static IHtmlContent ShortNameFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return html.MetaDataFor(expression, m =>
            {
                var defaultMetadata = m as
                    Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata;
                if (defaultMetadata != null)
                {
                    var displayAttribute = defaultMetadata.Attributes.Attributes
                        .OfType<DisplayAttribute>()
                        .FirstOrDefault();
                    if (displayAttribute != null)
                    {
                        return displayAttribute.ShortName;
                    }
                }
                //Return a default value if the property doesn't have a DisplayAttribute
                return m.DisplayName;
            });
        }
    }
}
