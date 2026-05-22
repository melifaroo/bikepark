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

            var modelExpressionProvider = html.ViewContext.HttpContext.RequestServices
            .GetRequiredService<IModelExpressionProvider>() as ModelExpressionProvider;

            var modelExplorer = modelExpressionProvider!.CreateModelExpression(html.ViewData, expression) 
            ?? throw new InvalidOperationException($"Failed to get model explorer for {modelExpressionProvider.GetExpressionText(expression)}");
            
            return new HtmlString(property(modelExplorer.Metadata));
        }

        public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.MetaDataFor(expression, m => m.Description ?? m.DisplayName ?? string.Empty);
        }

        public static IHtmlContent ShortNameFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return html.MetaDataFor(expression, m =>
            {
                if (m is Microsoft.AspNetCore.Mvc.ModelBinding.Metadata.DefaultModelMetadata defaultMetadata)
                {
                    var displayAttribute = defaultMetadata.Attributes.Attributes
                        .OfType<DisplayAttribute>()
                        .FirstOrDefault();
                    if (displayAttribute != null)
                    {
                        return displayAttribute.ShortName ?? m.DisplayName ?? string.Empty;
                    }
                }
                //Return a default value if the property doesn't have a DisplayAttribute
                return m.DisplayName ?? string.Empty;
            });
        }
    }
}
