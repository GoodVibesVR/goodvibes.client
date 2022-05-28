using GoodVibes.Client.Lovense.Dtos;
using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Lovense.Models.Abstractions
{
    public abstract class LovenseToy
    {
        public abstract string? Id { get; set; }
        public abstract string? Nickname { get; set; }
        public abstract string? Name { get; set; }
        public abstract bool Status { get; set; }
        public abstract string? Version { get; set; }
        public abstract int? Battery { get; set; }

        public string? DisplayName =>
            string.IsNullOrEmpty(Nickname) ? $"{Name} {Version}" : $"{Nickname} ({Name} {Version})";

        public abstract LovenseCommandEnum Function1 { get; set; }
        public abstract LovenseCommandEnum Function2 { get; set; }
        public abstract LovenseCommandEnum[] ToyFunctions { get; }

        public Dictionary<string, List<int>> ToyCommands { get; set; }
        private int Function1LastValue { get; set; }
        private int Function2LastValue { get; set; }

        protected LovenseToy()
        {
            ToyCommands = new Dictionary<string, List<int>>();
        }

        public void AddCommandToList(string function, int value)
        {
            Function1LastValue = value;

            var functionExists = ToyCommands.TryGetValue(function, out var values);
            if (!functionExists)
            {
                ToyCommands.Add(function, new List<int>()
                {
                    value
                });
            }
            else
            {
                values!.Add(value);
            }
        }

        public string GetCommandString()
        {
            if (ToyCommands.Count == 0)
            {
                Console.WriteLine($"CommandString returned: Vibrate:{Function1LastValue}");
                return $"Vibrate:{Function1LastValue}"; // TODO: FIX ME
            }
            
            var test = new Dictionary<string, int>();

            foreach (var toyCommand in ToyCommands)
            {
                var values = toyCommand.Value;
                var highestValue = values.Prepend(0).Max();

                test.Add(toyCommand.Key, highestValue);
            }
            
            var commandStr = string.Empty;
            foreach (var i in test)
            {
                if (commandStr != string.Empty)
                {
                    commandStr += ",";
                }

                commandStr += $"{i.Key}:{i.Value}";
            }

            Console.WriteLine($"CommandString returned: {commandStr}");
            return commandStr;
        }

        public List<CommandDto> GetCommandList()
        {
            var test = new Dictionary<string, int>();

            foreach (var toyCommand in ToyCommands)
            {
                var values = toyCommand.Value;
                var highestValue = values.Prepend(0).Max();

                test.Add(toyCommand.Key, highestValue);
            }

            //var functionStr = string.Empty;
            var commandList = test.Select(i => new CommandDto { Command = i.Key, Value = i.Value }).ToList();

            return commandList;
        }

        public void ClearCommandList()
        {
            ToyCommands.Clear();
        }
    }
}
