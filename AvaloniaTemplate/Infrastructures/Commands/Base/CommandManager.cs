using System;

namespace AvaloniaTemplate.Infrastructures.Commands.Base
{
    public static class CommandManager
    {
        public static event EventHandler RequireSuggested;

        public static void InvalidateRequireSuggested()
            => RequireSuggested?.Invoke(null, EventArgs.Empty);
    }
}
