using GoodVibes.Client.Common.Enums;

namespace GoodVibes.Client.Common.Extensions
{
    public static class ToyTypeExtensions
    {
        public static ToyTypeEnum GetToyTypeFromTypeString(string type)
        {
            return (ToyTypeEnum)Enum.Parse(typeof(ToyTypeEnum), type, true);
        }

        public static ToySupplierEnum GetToySupplierFromToyType(ToyTypeEnum toyType)
        {
            switch (toyType)
            {
                case ToyTypeEnum.Ambi:
                case ToyTypeEnum.Calor:
                case ToyTypeEnum.Diamo:
                case ToyTypeEnum.Dolce:
                case ToyTypeEnum.Domi:
                case ToyTypeEnum.Edge:
                case ToyTypeEnum.Exomoon:
                case ToyTypeEnum.Ferri:
                case ToyTypeEnum.Gush:
                case ToyTypeEnum.Hush:
                case ToyTypeEnum.Hyphy:
                case ToyTypeEnum.Lush:
                case ToyTypeEnum.Max:
                case ToyTypeEnum.Nora:
                case ToyTypeEnum.Osci:
                case ToyTypeEnum.SexMachine:
                case ToyTypeEnum.Tenera:
                case ToyTypeEnum.Gravity:
                case ToyTypeEnum.Flexer:
                case ToyTypeEnum.Gemini:
                case ToyTypeEnum.Ridge:
                    return ToySupplierEnum.Lovense;
                case ToyTypeEnum.PiVault:
                case ToyTypeEnum.PiShock:
                    return ToySupplierEnum.PiShock;
                case ToyTypeEnum.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(nameof(toyType), toyType, null);
            }
        }
    }
}