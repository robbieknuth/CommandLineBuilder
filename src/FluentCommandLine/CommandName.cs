using System;

namespace FluentCommandLine
{
    public readonly struct CommandName : IEquatable<CommandName>
    {
        private readonly string name;

        private CommandName(string name)
        {
            this.name = name;
        }

        public override string ToString() => this.name;

        public static bool TryConstruct(string name, out CommandName commandName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            // TODO normalization that can be customized
            else if (name.StartsWith("-"))
            {
                commandName = default;
                return false;
            }
            else
            {
                // TODO culture aware
                commandName = new CommandName(name);
                return true;
            }
        }

        public static CommandName FromApplicationName(string name)
        {
            return new CommandName(name);
        }

        public override int GetHashCode() => this.name?.GetHashCode() ?? 0;

        public override bool Equals(object? obj) => obj is CommandName val && Equals(this, val);

        public bool Equals(CommandName other) => string.Equals(this.name, other.name);
    }
}
