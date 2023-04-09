using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TE.BE.City.Infra.CrossCutting.Enum
{
    public enum ErrorCode
    {
        [Description("Usuário ou senha inválidos.")]
        UserNotIdentified = 1000,
        [Description("Não foi encontrado nenhum registro.")]
        SearchHasNoResult = 1001,
        [Description("Erro ao criar o registro.")]
        CreateIssueFail = 1002,
        [Description("Erro ao inserir o contato.")]
        InsertContactFail = 1003,
        [Description("Tente novamente mais tarde.")]
        GenericError = 1004,
        [Description("Erro ao salvar pesquisa.")]
        SurveyError = 1005,
    }
}
