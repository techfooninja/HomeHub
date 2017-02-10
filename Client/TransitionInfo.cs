namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using ViewModels;

    class TransitionInfo
    {
        public RuleViewModel Rule { get; set; }
        public bool IsOverride { get; set; }
    }
}
