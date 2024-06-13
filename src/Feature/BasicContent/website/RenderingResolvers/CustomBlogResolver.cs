using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace RenderingResolver
{
    public class CustomBlogResolver : RenderingContentsResolver
    {
        private List<Item> items = new List<Item>();

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));
            Assert.ArgumentNotNull(renderingConfig, nameof(renderingConfig));

            // Retrieve the category from the rendering parameters
            string category = rendering.Parameters["Category"];
            if (string.IsNullOrEmpty(category))
            {
                Log.Warn("CustomBlogResolver: No category specified in rendering parameters.", this);
                return null;
            }

            // Retrieve the datasource item
            Item ds = GetContextItem(rendering, renderingConfig);

            // Filter children based on the category parameter
            var filteredChildren = ds.Children
                .Where(child => child.Fields["Category"]?.Value == category)
                .ToList();

            // Add filtered children to the list
            if (filteredChildren.Any())
            {
                items.AddRange(filteredChildren);
            }

            // If no items, return null
            if (!items.Any())
                return null;

            // Create a JObject to hold the result
            JObject jobject = new JObject()
            {
                ["items"] = new JArray()
            };

            // Process items and add to the JObject
            jobject["items"] = ProcessItems(items, rendering, renderingConfig);
            return jobject;
        }
    }
}
