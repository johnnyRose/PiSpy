
namespace WebRole1.Models
{
    public static class PolicyFactory
    {
        public static Policy GetPolicyGroup(string name)
        {
            switch (name)
            {
                case "AND":
                    return new AndPolicyGroup();
                case "OR":
                    return new OrPolicyGroup();
                case "XOR":
                    return new XorPolicyGroup();
                case "NAND":
                    return new NandPolicyGroup();
                case "NOR":
                    return new NorPolicyGroup();
                default: // XNOR
                    return new XnorPolicyGroup();
            }
        }
    }
}
