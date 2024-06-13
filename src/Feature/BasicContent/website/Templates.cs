using Sitecore.Data;

namespace RenderingResolver
{
    public static class Templates
    {
        public static class Navigation
        {
            public static class content
            {
                public static readonly ID TemplateID = new ID("{4AC4AF4F-C72C-4979-B5E6-93C55E028735}");
            }
        }

        public static class Blogs
        {
            public static class Content
            {
                public static readonly ID TemplateID = new ID("{8D615655-D441-4708-AF2E-54B89FE3B120}");
            }
        }
        public static class Employees
        {
            public static class Content
            {
                public static readonly ID ItemId = new ID("{72E1EE9D-00E4-4694-82AC-46C78A66D30A}");
                public static readonly ID ParentId = new ID("{770BEADE-C7C6-4280-90B5-B7B1ABC99EB4}");
                public static readonly ID FolderId = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
            }
        }

    }
}   