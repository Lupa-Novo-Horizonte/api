using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using TE.BE.City.Domain.Interfaces;

namespace TE.BE.City.Domain
{
    public class OpenAIWebProvider : IOpenAIWebProvider
    {
        private APIAuthentication authentication;

        public OpenAIWebProvider(IConfiguration config)
        {
            authentication = new APIAuthentication(config["OpenAIKey"]);
        }

        public async Task<string> GenerateNewsRecomendation(string subject)
        {
            string recomendation = string.Empty;

            var api = new OpenAIAPI(authentication);

            // Create a new conversation with ChatGPT
            var conversation = api.Chat.CreateConversation();

            // Append user input and get response from ChatGPT
            conversation.AppendUserInput(subject);

            recomendation = await conversation.GetResponseFromChatbot();

            return recomendation;
        }
    }
}