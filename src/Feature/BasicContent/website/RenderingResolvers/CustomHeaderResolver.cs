using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RenderingResolver;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;

namespace RenderingResolver
{
    public class CustomHeader : RenderingContentsResolver
    {
        private List<Item> items = new List<Item>();

        public override object ResolveContents(Sitecore.Mvc.Presentation.Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));
            Assert.ArgumentNotNull(renderingConfig, nameof(renderingConfig));

            Item ds = GetContextItem(rendering, renderingConfig);

            /* var recommendedItemsFieldId = Templates.Navigation.content.TemplateID;*/



            var filteredChildren = ds.Children
                 .Where(child => child.Fields["EnableHeaderLinks"]?.Value == "1")
                 .ToList();


            if (filteredChildren.Any())
            {
                items.AddRange(filteredChildren);
            }

            if (!items.Any())
                return null;

            JObject jobject = new JObject()
            {
                ["items"] = (JToken)new JArray()
            };

            List<Item> objList = items != null ? items.ToList() : null;
            if (objList == null || objList.Count == 0)
                return jobject;
            jobject["items"] = ProcessItems(objList, rendering, renderingConfig);
            return jobject;
        }
    }
}
    