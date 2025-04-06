using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LlamaChat.Services;

namespace LlamaChat.Pages
{
    public class ChatModel : PageModel
    {
        private readonly TogetherService _aiService;

        public ChatModel(TogetherService aiService)
        {
            _aiService = aiService;
        }
        [BindProperty]
        public string? UserPrompt { get; set; }
        public string? AiResponse { get; set; }

        public async Task <IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrWhiteSpace(UserPrompt))
            {
                AiResponse = await _aiService.GetChatResponseAsync(UserPrompt);
            }
            return Page();

        }
    }
}