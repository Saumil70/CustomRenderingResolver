using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace RenderingResolver.SitecoreCommand
{
    public class CreateApiItemCommand : Command
    {
        private static readonly ID ItemTemplateID = Templates.Employees.Content.ItemId;
        private static readonly ID ParentItemID = Templates.Employees.Content.ParentId;

        public override void Execute(CommandContext context)
        {
            Database database = Sitecore.Configuration.Factory.GetDatabase("master");
            if (database == null)
            {
                Log.Error("Could not find database 'master'.", this);
                return;
            }

            Item parentItem = database.GetItem(ParentItemID);
            if (parentItem == null)
            {
                Log.Error($"Could not find parent item with ID {ParentItemID}.", this);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Synchronous HTTP request
                    HttpResponseMessage response = client.GetAsync("http://localhost:5065/Employee").Result;
                    response.EnsureSuccessStatusCode();

                    // Synchronous reading of response content
                    string json = response.Content.ReadAsStringAsync().Result;
                    JArray apiDataArray = JArray.Parse(json);

                    using (new SecurityDisabler())
                    {
                        // Check if the folder item already exists
                        Item folderItem = parentItem.Children["Data"];
                        if (folderItem == null)
                        {
                            folderItem = parentItem.Add("Data", new TemplateID(Templates.Employees.Content.FolderId));
                        }

                        foreach (var apiData in apiDataArray)
                        {
                            string employeeName = apiData["employeeName"].ToString().Trim();

                            // Check if the employee item already exists
                            Item existingItem = folderItem.Children[employeeName];
                            if (existingItem != null)
                            {
                                Log.Info($"Item with name {employeeName} already exists. Skipping creation.", this);
                                continue;
                            }

                            // Create a new item inside the folder for each employee
                            Item newItem = folderItem.Add(employeeName, new TemplateID(Templates.Employees.Content.ItemId));
                            newItem.Editing.BeginEdit();
                            newItem["EmployeeId"] = apiData["employeeId"].ToString();
                            newItem["EmployeeName"] = apiData["employeeName"].ToString();
                            newItem["ProjectId"] = apiData["projectId"].ToString();
                            newItem["ProjectManagerId"] = apiData["projectManagerId"].ToString();
                            newItem.Editing.EndEdit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error occurred while fetching data from the API or creating the item.", ex, this);
                }
            }
        }
    }
}
