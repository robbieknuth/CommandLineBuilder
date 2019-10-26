using System;
using System.Collections.Generic;

namespace CommandLineBuilder
{
    internal sealed class ParseContext
    {
        internal readonly HelpOptions helpOptions;

        public ParseContext(HelpOptions helpOptions)
        {
            this.helpOptions = helpOptions;
        }

        public List<Func<object, ApplicationResult>> OptionApplicators { get; } = new List<Func<object, ApplicationResult>>();
        public List<Func<object, ApplicationResult>> PositionalApplicators { get; } = new List<Func<object, ApplicationResult>>();
        public List<Action<object>> SwitchApplicators { get; } = new List<Action<object>>();
        public object? Settings { get; set; }
    }
}
