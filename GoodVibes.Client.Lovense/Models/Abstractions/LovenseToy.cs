using GoodVibes.Client.Common.Enums;
using GoodVibes.Client.Lovense.Dtos;
using GoodVibes.Client.Lovense.Enums;
using Newtonsoft.Json;

namespace GoodVibes.Client.Lovense.Models.Abstractions
{
    public abstract class LovenseToy
    {
        public virtual string? Id { get; set; }
        public virtual string? Nickname { get; set; }
        public virtual string? Name { get; set; }
        public virtual string? Version { get; set; }
        public virtual int? Battery { get; set; }
        public string? DisplayName =>
            string.IsNullOrEmpty(Nickname) ? CombinedName : $"{Nickname} ({CombinedName})";
        public int Function1MaxStrengthPercentage { get; set; }
        public int Function2MaxStrengthPercentage { get; set; }
        public int Function3MaxStrengthPercentage { get; set; }

        public abstract ToyTypeEnum ToyType { get; }
        public abstract bool Enabled { get; set; }
        public abstract LovenseCommandEnum Function1 { get; set; }
        public abstract LovenseCommandEnum Function2 { get; set; }
        public abstract LovenseCommandEnum Function3 { get; set; }

        [JsonIgnore]
        public virtual bool? Status { get; set; }

        [JsonIgnore]
        public abstract LovenseCommandEnum[] ToyFunctions { get; }

        [JsonIgnore]
        public Dictionary<LovenseCommandEnum, List<int>> ToyCommands { get; set; }
        
        [JsonIgnore]
        private string CombinedName => string.IsNullOrEmpty(Version) ? Name! : $"{Name} {Version}";
        
        [JsonIgnore]
        private int Function1LastValue { get; set; }

        [JsonIgnore]
        private int Function2LastValue { get; set; }

        [JsonIgnore]
        private int Function3LastValue { get; set; }

        protected LovenseToy()
        {
            ToyCommands = new Dictionary<LovenseCommandEnum, List<int>>();
        }

        public int ConvertPercentageByCommand(LovenseCommandEnum command, float percentage)
        {
            switch (command)
            {
                case LovenseCommandEnum.Vibrate:
                case LovenseCommandEnum.Vibrate1:
                case LovenseCommandEnum.Vibrate2:
                case LovenseCommandEnum.Vibrate3:
                    return ConvertVibratePercentage(percentage);
                case LovenseCommandEnum.Rotate:
                case LovenseCommandEnum.RotateAntiClockwise:
                    return ConvertRotatePercentage(percentage);
                case LovenseCommandEnum.Fingering:
                    return ConvertFingeringPercentage(percentage);
                case LovenseCommandEnum.Thrusting:
                    return ConvertThrustingPercentage(percentage);
                case LovenseCommandEnum.Suction:
                    return ConvertSuctionPercentage(percentage);
                case LovenseCommandEnum.Pump:
                    return ConvertPumpPercentage(percentage);
                case LovenseCommandEnum.Depth:
                    return ConvertDepthPercentage(percentage);
                case LovenseCommandEnum.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        private int ConvertVibratePercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 5) * 100);
        }

        private int ConvertRotatePercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 5) * 100);
        }

        private int ConvertDepthPercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 33) * 100);
        }

        private int ConvertSuctionPercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 5) * 100);
        }

        private int ConvertThrustingPercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 5) * 100);
        }

        private int ConvertFingeringPercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 10) * 100);
        }

        private int ConvertPumpPercentage(float percentage)
        {
            return (int)Math.Round((double)(percentage / 33) * 100);
        }

        private int DivideByStrengthPercentage(LovenseCommandEnum function, int value)
        {
            if (function == Function1)
            {
                var percentage = (float)Function1MaxStrengthPercentage / 100;
                return (int)Math.Round(value * percentage);
            }

            if (function == Function2)
            {
                var percentage = (float)Function2MaxStrengthPercentage / 100;
                return (int)Math.Round(value * percentage);
            }

            if (function == Function3)
            {
                var percentage = (float)Function3MaxStrengthPercentage / 100;
                return (int)Math.Round(value * percentage);
            }

            return 0;
        }

        public void AddCommand(LovenseCommandEnum function, int value)
        {
            if (function == LovenseCommandEnum.None) return;

            if (function == Function1)
            {
                Function1LastValue = value;
                AddCommandToList(function, value);
            }

            if (function == Function2)
            {
                Function2LastValue = value;
                AddCommandToList(function, value);
            }

            if (function == Function3)
            {
                Function3LastValue = value;
                AddCommandToList(function, value);
            }
        }

        private void AddCommandToList(LovenseCommandEnum function, int value)
        {
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

        public string? GetCommandString()
        {
            if (ToyCommands.Count == 0) return null;

            var function1Value = Function1LastValue;
            var function2Value = Function2LastValue;
            var function3Value = Function3LastValue;

            foreach (var toyCommand in ToyCommands)
            {
                var values = toyCommand.Value;
                if (values == null) continue;

                if (toyCommand.Key == Function1)
                {
                    function1Value = values.Last();
                }

                if (toyCommand.Key == Function2)
                {
                    function2Value = values.Last();
                }

                if (toyCommand.Key == Function3)
                {
                    function3Value = values.Last();
                }
            }

            var commandStr = string.Empty;
            if (Function2 == LovenseCommandEnum.None && Function3 == LovenseCommandEnum.None)
            {
                commandStr = $"{Function1.ToString()}:{DivideByStrengthPercentage(Function1, function1Value)}";
            }

            if (Function3 == LovenseCommandEnum.None)
            {
                commandStr =
                    $"{Function1.ToString()}:{DivideByStrengthPercentage(Function1, function1Value)},{Function2.ToString()}:{DivideByStrengthPercentage(Function2, function2Value)}";
            }

            if (Function3 != LovenseCommandEnum.None)
            {
                commandStr =
                    $"{Function1.ToString()}:{DivideByStrengthPercentage(Function1, function1Value)},{Function2.ToString()}:{DivideByStrengthPercentage(Function2, function2Value)},{Function3.ToString()}:{DivideByStrengthPercentage(Function3, function3Value)}";
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

                test.Add(toyCommand.Key.ToString(), highestValue);
            }

            //var functionStr = string.Empty;
            var commandList = test.Select(i => new CommandDto { Command = i.Key.ToString(), Value = i.Value }).ToList();

            return commandList;
        }

        public void ClearCommandList()
        {
            ToyCommands.Clear();
        }

        public void SetStrengthPercentage(int strength1Percentage, int strength2Percentage, int strength3Percentage)
        {
            Function1MaxStrengthPercentage = strength1Percentage;
            if (Function1 != LovenseCommandEnum.None)
            {
                AddCommandToList(Function1, Function1LastValue);
            }

            Function2MaxStrengthPercentage = strength2Percentage;
            if (Function2 != LovenseCommandEnum.None)
            {
                AddCommandToList(Function2, Function2LastValue);
            }

            Function3MaxStrengthPercentage = strength3Percentage;
            if (Function3 != LovenseCommandEnum.None)
            {
                AddCommandToList(Function3, Function3LastValue);
            }
        }
    }
}