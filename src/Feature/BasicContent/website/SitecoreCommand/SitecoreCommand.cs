using Sitecore.Shell.Framework.Commands;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using RenderingResolver;


namespace RenderingResolver
{
    public class CreateItemCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            // Check if the context is valid (e.g., a specific item template or location)
            if (context.Items != null && context.Items.Length > 0)
            {
                // Get the parent item where the new item will be created
                Item parentItem = context.Items[0];

                // Check if the parent item exists
                if (parentItem != null)
                {
                    // Start a security disabler to create items even if user doesn't have permission
                    using (new SecurityDisabler())
                    {
                        // Create a new item under the parent item
                        Item newItem = parentItem.Add("New Item Name", new TemplateID(Templates.Blogs.Content.TemplateID));

                        // Check if the new item was created successfully
                        if (newItem != null)
                        {
                            Log.Info($"Item '{newItem.Name}' created under '{parentItem.Paths.FullPath}'", this);
                        }
                        else
                        {
                            Log.Error($"Failed to create item under '{parentItem.Paths.FullPath}'", this);
                        }
                    }
                }
            }
        }
    }
}

