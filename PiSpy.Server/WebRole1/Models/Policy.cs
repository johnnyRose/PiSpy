using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebRole1.Models
{
    public class Policy
    {
        public int Id { get; set; }

        public virtual List<PolicyGroup> PolicyGroups { get; set; }

        public int? PiSpyDeviceId { get; set; }
        public virtual PiSpyDevice PiSpyDevice { get; set; }

        public int? PolicyActionId { get; set; }
        public virtual PolicyAction PolicyAction { get; set; }

        public virtual bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return this.PolicyGroups.All(m => m.IsSatisfied(dataLog));
        }

        [NotMapped]
        public string DisplayMember { get { return Display(); } }
        public virtual string Name() { return "Base Policy Name"; }
        public virtual string Display() { return "Base Policy Display";  }
    }

    public class NumericPolicy : Policy
    {
        [DisplayName("Comparison Operator")]
        public int ComparisonOperatorId { get; set; }
        public virtual ComparisonOperator ComparisonOperator { get; set; }

        public double Value { get; set; }

        [DisplayName("Numeric Policy Type")]
        public int NumericPolicyTypeId { get; set; }
        public virtual NumericPolicyType NumericPolicyType { get; set; }

        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException("dataLog");
            }

            var testVariable = NumericPolicyType.Name == "Temperature" ? dataLog.TemperatureFahrenheit : dataLog.Humidity;

            switch (this.ComparisonOperator.Operator)
            {
                case "==": return testVariable == Value;
                case "<=": return testVariable <= Value;
                case "<": return testVariable < Value;
                case ">=": return testVariable >= Value;
                case ">": return testVariable > Value;
                default: return testVariable != Value;
            }
        }

        public override string Name()
        {
            return NumericPolicyType.Name + " Policy";
        }

        public override string Display()
        {
            return NumericPolicyType.Name + " " + ComparisonOperator.Operator + " " + Value + NumericPolicyType.PostUnit;
        }
    }

    public class ReminderPolicy : Policy
    {
        public DateTime TriggerTime { get; set; }

        public bool Triggered { get; set; }

        public override bool IsSatisfied(PiSpyDataLog dataLog) // disregard the dataLog for this override
        {
            if (DateTime.Now.AddHours(-5).AddSeconds(15) >= TriggerTime) // -5 hours for time difference, +15 seconds to account for the time it takes for SendGrid to send a message
            {
                Triggered = true;
                return true;
            }

            return false;
        }

        public override string Name()
        {
            return "Reminder Policy";
        }

        public override string Display()
        {
            return "the time is " + TriggerTime;
        }
    }

    public class DisconnectPolicy : Policy
    {
        public int MinutesDisconnected { get; set; }

        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return ((PiSpyDataLog)dataLog).TimeReceived <= DateTime.Now.AddMinutes(-MinutesDisconnected);
        }

        public override string Name()
        {
            return "Disconnect Policy";
        }

        public override string Display()
        {
            return "the PiSpy hasn't connected in " + MinutesDisconnected + (MinutesDisconnected == 1 ? " minute" : " minutes");
        }
    }

    public class PolicyGroup : Policy
    {
        public virtual List<Policy> Policies { get; set; }

        public override string Name()
        {
            return "PolicyGroupName";
        }

        public override string Display()
        {
            return "PolicyGroupDisplay";
        }
    }

    public class AndPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return this.Policies.All(m => m.IsSatisfied(dataLog));// && this.PolicyGroups.All(m => m.IsSatisfied(dataLog));
        }

        public override string Name()
        {
            return "AND";
        }

        public override string Display()
        {
            return "(" + string.Join(" AND ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class OrPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return this.Policies.Any(m => m.IsSatisfied(dataLog));// || this.PolicyGroups.Any(m => m.IsSatisfied(dataLog));
        }

        public override string Name()
        {
            return "OR";
        }

        public override string Display()
        {
            return "(" + string.Join(" OR ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class XorPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return this.Policies.Count(m => m.IsSatisfied(dataLog)) == 1;// ^ this.PolicyGroups.Count(m => m.IsSatisfied(dataLog)) == 1;
        }

        public override string Name()
        {
            return "XOR";
        }

        public override string Display()
        {
            return "(" + string.Join(" XOR ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class NandPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return !(this.Policies.All(m => m.IsSatisfied(dataLog)));// && this.PolicyGroups.All(m => m.IsSatisfied(dataLog)));
        }

        public override string Name()
        {
            return "NAND";
        }

        public override string Display()
        {
            return "(" + string.Join(" NAND ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class NorPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return !(this.Policies.Any(m => m.IsSatisfied(dataLog)));
        }

        public override string Name()
        {
            return "NOR";
        }

        public override string Display()
        {
            return "(" + string.Join(" NOR ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class XnorPolicyGroup : PolicyGroup
    {
        public override bool IsSatisfied(PiSpyDataLog dataLog)
        {
            return this.Policies.Count(m => m.IsSatisfied(dataLog)) != 1;
        }

        public override string Name()
        {
            return "XNOR";
        }

        public override string Display()
        {
            return "(" + string.Join(" XNOR ", this.Policies.Select(m => m.Display())) + ")";
        }
    }

    public class ComparisonOperator
    {
        public int Id { get; set; }
        public string Operator { get; set; }
    }

    public class NumericPolicyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostUnit { get; set; }
    }
}
