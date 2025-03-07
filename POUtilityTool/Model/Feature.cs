using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.Model
{
    public class Feature
    {
        public string name { get; set; }
        public int Id { get; set; }
        public int cardOrder { get; set; }
        public int userStoryCount { get; set; }
        public int userStoryClosed { get; set; }
        public double userStoryClosedPercentage { get; set; }
        public int technicalStoryCount { get; set; }
        public int technicalStoryClosed { get; set; }
        public double technicalStoryClosedPercentage { get; set; }
        public int spikeCount { get; set; }
        public int spikeClosed { get; set; }
        public double spikeClosedPercentage { get; set; }
        public int activeWorkItems { get; set; }
        public int totalClosedWorkItems { get; set; }
        public int totalWorkItems { get; set; }
        public double currentSprintProgressPercent { get; set; }
        public double nextSprintProgressPercent { get; set; }

        public Feature(string name)
        {
            this.name = name;
        }

        public void CalculateAllPercentages()
        {
            this.userStoryClosedPercentage = CalculatePercentage(userStoryClosed, userStoryCount);
            this.technicalStoryClosedPercentage = CalculatePercentage(technicalStoryClosed, technicalStoryCount);
            this.spikeClosedPercentage = CalculatePercentage(spikeClosed, spikeCount);
            this.totalClosedWorkItems = userStoryClosed + technicalStoryClosed + spikeClosed;
            this.totalWorkItems = userStoryCount + technicalStoryCount + spikeCount;
            this.currentSprintProgressPercent = CalculatePercentage(totalClosedWorkItems, totalWorkItems);
            this.nextSprintProgressPercent = CalculatePercentage(totalClosedWorkItems + activeWorkItems, totalWorkItems);
        }

        public double CalculatePercentage(double numerator, double denominator)
        {
            if (denominator != 0)
            {
                return numerator / denominator;
            }
            return 0;
        }
    }
}
