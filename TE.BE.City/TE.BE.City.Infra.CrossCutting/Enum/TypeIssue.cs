using System.ComponentModel;

namespace TE.BE.City.Infra.CrossCutting.Enum
{
    public enum TypeIssue
    {
        [Description("Todos")]
        All = -1,

        [Description("Calçadas e Asfalto")]
        Asphalt = 0,
        [Description("Coleta de Lixo")]
        Collect = 1,
        [Description("Iluminação Pública")]
        Light = 2,
        [Description("Serviços Público")]
        PublicService = 3,
        [Description("Tratamento de Esgoto")]
        Sewer = 4,
        [Description("Limpeza Urbana")]
        Trash = 5,
        [Description("Água Potável")]
        Water = 6
    }
}
