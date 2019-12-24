using ICities;
using UnityEngine;

namespace UnifiedRailwaySystem
{
    public class URSMod : IUserMod
    {
        public string Name
        {
            get { return "Unified Railway System"; }
        }

        public string Description
        {
            get { return "Let Train and Metro use the same railway."; }
        }
    }
}
