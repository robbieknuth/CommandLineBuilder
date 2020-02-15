using System;
using System.Collections.Generic;

namespace CommandLine
{
    internal sealed class ParseContext
    {
        internal readonly HelpOptions helpOptions;
        internal readonly ParserOptions parserOptions;

        public ParseContext(ParserOptions parserOptions, HelpOptions helpOptions)
        {
            this.helpOptions = helpOptions;
            this.parserOptions = parserOptions;
        }

        public List<Func<object, ApplicationResult>> OptionApplicators { get; } = new List<Func<object, ApplicationResult>>();
        public List<Func<object, ApplicationResult>> PositionalApplicators { get; } = new List<Func<object, ApplicationResult>>();
        public List<Action<object>> SwitchApplicators { get; } = new List<Action<object>>();
        public List<Action<object>> SettingDefaultApplicators { get; } = new List<Action<object>>();
        public object? Settings { get; set; }
    }
}
