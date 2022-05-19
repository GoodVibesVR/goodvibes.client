using GoodVibes.Client.Lovense.Enums;

namespace GoodVibes.Client.Lovense.Models.Abstractions
{
    public abstract class LovenseToy
    {
        protected abstract LovenseCommandEnum Function1 { get; }
        protected abstract LovenseCommandEnum Function2 { get; }
        public abstract LovenseCommandEnum[] SpecialFunctions { get; }
    }
}
