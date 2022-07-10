using System.Reflection;

namespace TE.BE.City.Infra.CrossCutting
{
    public static class BooleanExtensionMethods
    {
        public static string ToSimNao(this bool value)
        {
            if (value)
                return "Sim";
            else
                return "Não";
        }
    }
}
