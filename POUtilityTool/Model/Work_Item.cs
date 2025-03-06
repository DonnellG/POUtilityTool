using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.Model
{
    public class Work_Item
    {
        public int Id;
        public string Title;
        public double StoryPoints;
        public string StateFlow;
        public string POStatus; // generated from state flow [R,C,G,N]
        public string POAssignee;
        public double POSprint;
        public string SubjectChannels;

        public Work_Item(int Id,
                        string Title,
                        double StoryPoints = 0,
                        string StateFlow = "1. New",
                        string POAssignee = "",
                        int POSprint = 0,
                        string SubjectChannels = "")
        {
            this.Id = Id;
            this.Title = Title;
            this.StoryPoints = StoryPoints;
            this.StateFlow = StateFlow;
            this.POAssignee = POAssignee;
            this.POSprint = POSprint;
            this.SubjectChannels = SubjectChannels;
        }

        public void AssignPOStatus()
        {
            switch (StateFlow)
            {
                case "":
                    this.POStatus = "N";
                    break;
                case "1. New":
                    this.POStatus = "N";
                    break;
                case "2. Grooming":
                    this.POStatus = "Gr";
                    break;
                case "3. Approved by Graitec":
                    this.POStatus = "G";
                    break;
                case "4. Approved by Canam":
                    this.POStatus = "C";
                    break;
                case "5. Ready to Dev":
                    this.POStatus = "R";
                    break;
                case "6. Review":
                    this.POStatus = "Gr";
                    break;
                case "7. Resolved by Graitec":
                    this.POStatus = "T";
                    break;
                case "8. Validated by Canam":
                    this.POStatus = "T";
                    break;
                case "9. On-Hold by CSC":
                    this.POStatus = "T";
                    break;
                default:
                    this.POStatus = "N";
                    break;
            }
        }
    }
}
