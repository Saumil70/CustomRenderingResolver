using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Pipelines;
using System;

namespace RenderingResolver.SitecoreCommand
{
    public class CreateApiItemCommand : Command
    {
        public override void Execute(CommandContext context)
        {
            try
            {
                // Call the custom pipeline
                CorePipeline.Run("customPipeline", new PipelineArgs());
            }
            catch (Exception ex)
            {
                Log.Error("Error occurred while executing the custom pipeline.", ex, this);
            }
        }
    }
}
