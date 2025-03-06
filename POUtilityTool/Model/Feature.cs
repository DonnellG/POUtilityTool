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

        public Feature(string name)
        {
            this.name = name;
        }

        public void CalculateAllPercentages()
        {
            CalculateUserStoryPercentage();
            CalculateTechnicalStoryPercentage();
            CalculateSpikePercentage();
        }

        public void CalculateUserStoryPercentage()
        {
            if (userStoryCount != 0)
            {
                this.userStoryClosedPercentage = (double)userStoryClosed / userStoryCount;
            }
        }
        public void CalculateTechnicalStoryPercentage()
        {
            if (technicalStoryCount != 0)
            {
                this.technicalStoryClosedPercentage = (double)technicalStoryClosed / technicalStoryCount;
            }
        }
        public void CalculateSpikePercentage()
        {
            if (spikeCount != 0)
            {
                this.spikeClosedPercentage = (double)spikeClosed / spikeCount;
            }
        }
    }
}
