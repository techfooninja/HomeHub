namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    class TransitionInfo
    {
        public IRule Rule { get; set; }
        public bool IsOverride { get; set; }
    }
}
