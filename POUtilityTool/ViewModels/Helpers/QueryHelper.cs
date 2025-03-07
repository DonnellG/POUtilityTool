using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Organization;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using POUtilityTool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels.Helpers
{
    public class QueryHelper
    {
        private HttpClient httpClient { get; set; }
        private VssBasicCredential credentials { get; set; }
        private Uri uri { get; set; }

        private string orgName;

        public string OrgName
        {
            get { return orgName; }
            set
            {
                orgName = value;
                uri = new Uri("https://dev.azure.com/" + value);
            }
        }

        private string pat;
        public string PAT
        {
            get { return pat; }
            set
            {
                pat = value;
                credentials = new VssBasicCredential(string.Empty, value);
            }
        }

        public QueryHelper()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> CheckAuthentication()
        {
            string authenticationUri = $"https://dev.azure.com/{OrgName}/_apis/projects?api-version=7.1-preview.1";

            using (HttpClient client = new HttpClient())
            {
                string patToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($":{pat}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", patToken);

                HttpResponseMessage response = await client.GetAsync(authenticationUri);

                if (response.IsSuccessStatusCode)
                {
                    return "Authentication successful";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return "Authentication failed: Invalid PAT.";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return "Invalid Organization URI.";
                }
                else
                {
                    return $"Unexpected error: {response.StatusCode}"; ;
                }
            }
        }
        public async Task<IList<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>> QueryFeatures(string project,string areaPath)
        {
            var wiql = new Wiql()
            {
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Feature'" +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.AreaPath] UNDER '" + project + "\\" + areaPath + "' " +
                        "And [System.State] <> 'Closed' " +
                        "And [System.State] <> 'Removed' " +
                        "Order By [Id] Asc",
            };

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.Title",
                                    "Microsoft.VSTS.Scheduling.StoryPoints", "Custom.StateFlow",
                                    "Custom.POAssigneePlanning", "Custom.POSprint",
                                    "Custom.SubjectChannels", "Microsoft.VSTS.Common.StackRank"};

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }

        }
        public async Task<ObservableCollection<Feature>> GenerateFeatures(string project, string areaPath)
        {
            var qFeatures = new ObservableCollection<Feature>();
            var features = await this.QueryFeatures(project, areaPath).ConfigureAwait(false);
            int count = 0; //remove when done testing

            foreach (var feature in features)
            {
                var qFeature = new Feature((string)feature.Fields["System.Title"]);
                qFeature.Id = (int)feature.Id;

                var userWorkitems = await this.QueryFeatureWIs(project, "User Story", qFeature.Id).ConfigureAwait(false);
                qFeature.userStoryCount = userWorkitems.Count();
                GetFeatureWorkItemCounts(userWorkitems, qFeature, "User Story");

                var technicalWorkitems = await this.QueryFeatureWIs(project, "Technical Story", qFeature.Id).ConfigureAwait(false);
                qFeature.technicalStoryCount = technicalWorkitems.Count();
                GetFeatureWorkItemCounts(technicalWorkitems, qFeature, "Technical Story");

                var spikeWorkitems = await this.QueryFeatureWIs(project, "Spike", qFeature.Id).ConfigureAwait(false);
                qFeature.spikeCount = spikeWorkitems.Count();
                GetFeatureWorkItemCounts(spikeWorkitems, qFeature, "Spike");

                qFeature.CalculateAllPercentages();

                qFeatures.Add(qFeature);


                count++; //remove when done testing
                if (count == 5) { break; } //remove when done testing
            }

            return qFeatures;
        }
        public void GetFeatureWorkItemCounts(IList<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem> userWorkitems, Feature feature, string workItemType)
        {
            int closedWICount = 0;
            int activeWICount = 0;
            foreach (var workItem in userWorkitems)
            {
                switch ((string)workItem.Fields["System.State"])
                {
                    case "Closed":
                        closedWICount++; break;
                    case "Resolved":
                        activeWICount++; break;
                    //case "Active":
                    //    activeWICount++; break;
                    case "Ready For Testing":
                        activeWICount++; break;
                        //case "Code Review":
                        //    activeWICount++; break;
                }
            }
            switch (workItemType)
            {
                case "User Story":
                    feature.userStoryClosed = closedWICount;
                    feature.activeWorkItems += activeWICount;
                    break;
                case "Technical Story":
                    feature.technicalStoryClosed = closedWICount;
                    feature.activeWorkItems += activeWICount;
                    break;
                case "Spike":
                    feature.spikeClosed = closedWICount;
                    feature.activeWorkItems += activeWICount;
                    break;
            }
        }
        public async Task<IList<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>> QueryFeatureWIs(string project, string WorkItemType, int id)
        {
            var wiql = new Wiql()
            {
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = '" + WorkItemType + "' " +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.State] <> 'Removed' " +
                        "And [System.Parent] = '" + id + "' " +
                        "Order By [Id] Asc",
            };

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.Title", "System.State" };

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }

        }
        public async Task<IList<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>> QuerybyWorkItemType(string project, string WorkItemType)
        {
            var wiql = new Wiql()
            {
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = '" + WorkItemType + "' " +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.AreaPath] UNDER 'Proj CANAM Steel\\Bucket 2' " +
                        "And [System.State] <> 'Closed' " +
                        "And [System.State] <> 'Removed' " +
                        "Order By [Id] Asc",
            };

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.Title",
                                    "Microsoft.VSTS.Scheduling.StoryPoints", "Custom.StateFlow",
                                    "Custom.POAssigneePlanning", "Custom.POSprint",
                                    "Custom.SubjectChannels"};

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }

        }

        public async void DisplayAllAvailableFields(string project)
        {
            try
            {
                using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
                {
                    var AllFields = await httpClient.GetFieldsAsync(project).ConfigureAwait(false); // returns Task<List<WorkItemField>>
                    if (AllFields.Count == 0)
                    {
                        Console.WriteLine("Count = 0");
                    }
                    else
                    {
                        foreach (var field in AllFields)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}", field.Name, field.ReferenceName, field.SupportedOperations);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Consider logging the exception or handling it appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        public async Task<List<Work_Item>> GenerateWIs(string project, string WorkItemType)
        {
            List<Work_Item> qWorkItems = new List<Work_Item>();
            var workItems = await this.QuerybyWorkItemType(project, WorkItemType).ConfigureAwait(false);

            foreach (var workItem in workItems)
            {
                Work_Item qWorkItem = new Work_Item((int)workItem.Id, (string)workItem.Fields["System.Title"]);

                if (workItem.Fields.ContainsKey("Microsoft.VSTS.Scheduling.StoryPoints"))
                {
                    qWorkItem.StoryPoints = (double)workItem.Fields["Microsoft.VSTS.Scheduling.StoryPoints"];
                }
                else
                {
                    qWorkItem.StoryPoints = workItem.Fields.ContainsKey("Custom.POSprint") ? (Int64)workItem.Fields["Custom.POSprint"] : 0;
                }
                qWorkItem.StateFlow = workItem.Fields.ContainsKey("Custom.StateFlow") ? (string)workItem.Fields["Custom.StateFlow"] : "";
                qWorkItem.POSprint = workItem.Fields.ContainsKey("Custom.POSprint") ? (Int64)workItem.Fields["Custom.POSprint"] : 0;
                qWorkItem.POAssignee = workItem.Fields.ContainsKey("Custom.POAssigneePlanning") ? (string)workItem.Fields["Custom.POAssigneePlanning"] : "";
                qWorkItem.SubjectChannels = workItem.Fields.ContainsKey("Custom.SubjectChannels") ? (string)workItem.Fields["Custom.SubjectChannels"] : "";
                qWorkItem.AssignPOStatus();
                qWorkItems.Add(qWorkItem);
            }
            return qWorkItems;
        }

        public async void UpdateFeatureRanksByStack(string project, string areaPath)
        {
            var features = await this.QueryFeatures(project, areaPath).ConfigureAwait(false);
            var orderedFeatures = features.OrderBy(feat => feat.Fields["Microsoft.VSTS.Common.StackRank"]);

            int rankNum = 1;

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
            {

                foreach (var feature in orderedFeatures)
                {
                    Console.WriteLine("{0}\t\t{1}", feature.Fields["System.Title"], feature.Fields["Microsoft.VSTS.Common.StackRank"]);
                    JsonPatchDocument patchDocument = new JsonPatchDocument()
                    {
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/Custom.Rank",
                            Value = rankNum
                        }
                    };

                    await httpClient.UpdateWorkItemAsync(patchDocument, (int)feature.Id);
                    rankNum++;
                }
            }
        }

        private async Task<IList<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>> QueryAllFeatures(string project)
        {
            var wiql = new Wiql()
            {
                Query = "Select [Id], [Microsoft.VSTS.Common.StackRank] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Feature'" +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.State] <> 'Closed' " +
                        "And [System.State] <> 'Removed' " +
                        "ORDER BY [Microsoft.VSTS.Common.StackRank] DESC", //some reason this doesnt work
            };

            using (var httpClient = new WorkItemTrackingHttpClient(this.uri, this.credentials))
            {
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                // some error handling
                if (ids.Length == 0)
                {
                    return Array.Empty<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem>();
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.Title",
                                    "Microsoft.VSTS.Common.StackRank", "Custom.Rank"};

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
            }
        }

    }
}

