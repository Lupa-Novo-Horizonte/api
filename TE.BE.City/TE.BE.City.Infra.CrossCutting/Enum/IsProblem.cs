using System.ComponentModel;

namespace TE.BE.City.Infra.CrossCutting.Enum;

public enum IsProblem
{
    [Description("Todos")]
    All,
    [Description("Problemas")]
    Problem,
    [Description("Em conformidade")]
    NoProblem
}