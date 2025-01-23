using OllamaSharp;

namespace OllamaChatApp
{
    class Program
    {
        private static string chatHistoryFile = "chat_history.txt";
        private static List<string> chatHistory = [];

        static async Task Main(string[] _)
        {
            var uri = new Uri("http://localhost:11434");
            var ollama = new OllamaApiClient(uri);
            ollama.SelectedModel = "coder"; 

            LoadChatHistory();
            chatHistory.Add("system: You are an AI bot that tries to help the user complete coding task");
            chatHistory.Add("AI: How may I help you with your coding challenges today?");

            try
            {
                Console.WriteLine("You can now chat with the model. Type 'exit' to end the session.");
                bool firstRun = true;
                while (true)
                {
                    Console.Write("You: ");
                    var userInput = Console.ReadLine();
                    if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
                        break;

                    chatHistory.Add($"You: {userInput}");
                    Console.WriteLine("AI: ");

                    if (userInput is null) continue;
                    if (userInput == "exit") break;

                    if (firstRun)
                    {
                        string newMessage = "";
                        foreach(var hist in chatHistory)
                        {
                            newMessage = hist + "\n";
                        }

                        userInput = newMessage + "\n" + userInput;
                        firstRun = false;
                    }

                    await foreach (var stream in ollama.GenerateAsync(userInput))
                    {
                        if (stream is null) continue;
                        Console.Write(stream.Response);
                        chatHistory.Add($"AI: {stream.Response}");
                    }
                    Console.WriteLine();
                }

                SaveChatHistory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void LoadChatHistory()
        {
            if (File.Exists(chatHistoryFile))
            {
                chatHistory = File.ReadAllLines(chatHistoryFile).ToList();
                foreach (var line in chatHistory)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private static void SaveChatHistory()
        {
            File.WriteAllLines(chatHistoryFile, chatHistory);
            Console.WriteLine("Chat history saved.");
        }
    }
}