using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityAwarenessBot.GUI.Models
{
    /// <summary>
    /// Core chatbot logic: keyword recognition, conversation flow,
    /// memory recall and sentiment-driven responses.
    /// </summary>
    public class ChatbotEngine
    {
        private readonly ResponseManager _responses = new ResponseManager();
        private readonly SentimentDetector _sentiment = new SentimentDetector();
        public UserMemory Memory { get; } = new UserMemory();

        // Delegates — used to solve a programming problem as required.
        public delegate string ResponseGenerator(string input);

        private readonly Dictionary<string, ResponseGenerator> _handlers;

        public ChatbotEngine()
        {
            // Delegate map: each intent maps to a method returning a string.
            _handlers = new Dictionary<string, ResponseGenerator>(StringComparer.OrdinalIgnoreCase)
            {
                ["greeting"] = HandleGreeting,
                ["howareyou"] = HandleHowAreYou,
                ["purpose"] = HandlePurpose,
                ["whatcanask"] = HandleWhatCanAsk,
                ["name"] = HandleName,
                ["favouritetopic"] = HandleFavouriteTopic,
                ["followup"] = HandleFollowUp,
                ["help"] = HandleHelp
            };
        }

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't quite understand that. Could you rephrase?";

            Memory.InteractionCount++;
            string input = userInput.ToLower().Trim();

            // 1. Detect sentiment first
            string detectedSentiment = _sentiment.Detect(input);
            Memory.LastSentiment = detectedSentiment;
            string empathy = _sentiment.GetEmpathyPrefix(detectedSentiment, Memory);

            // 2. Capture user's name if they introduce themselves
            TryCaptureName(userInput);

            // 3. Detect favourite topic memory
            TryCaptureFavouriteTopic(input);

            // 4. Handle general intents via delegates
            foreach (var handler in _handlers)
            {
                if (MatchesIntent(input, handler.Key))
                {
                    return empathy + handler.Value(input);
                }
            }

            // 5. Cybersecurity keyword recognition (randomised responses)
            string? keyword = FindKeyword(input);
            if (keyword != null)
            {
                Memory.RememberTopic(keyword);
                string? tip = _responses.GetRandomResponse(keyword);

                // Personalise using memory if user has a favourite topic
                string personal = "";
                if (Memory.HasFavouriteTopic && Memory.FavouriteTopic == keyword)
                {
                    personal = $"Since {_responses.TopicNames[keyword]} is your favourite topic, here's something extra: ";
                }

                return empathy + personal + tip + "\n\nWould you like another tip on this topic, or ask about something else?";
            }

            // 6. Follow-up requests like "another tip" / "explain more"
            if (IsFollowUp(input) && !string.IsNullOrEmpty(Memory.LastTopic))
            {
                string? tip = _responses.GetRandomResponse(Memory.LastTopic);
                return empathy + $"Sure{NameSuffix()}, here's another tip on {_responses.TopicNames[Memory.LastTopic]}: {tip}";
            }

            // 7. Default fallback — no crash on unknown input
            return empathy + "I'm not sure I understand. Can you try rephrasing? You can ask me about passwords, scams, privacy, phishing, malware, or safe browsing.";
        }

        // ---------- Delegate handlers ----------

        private string HandleGreeting(string input)
        {
            if (Memory.HasName)
                return $"Hello again, {Memory.Name}! How can I help you stay safe online today?";
            return "Hello! I'm the Cybersecurity Awareness Bot. What is your name?";
        }

        private string HandleHowAreYou(string input)
            => "I'm doing well, thank you! I'm always ready to help you stay safe online. What would you like to know?";

        private string HandlePurpose(string input)
            => "My purpose is to help you learn about cybersecurity and stay safe online. Ask me about passwords, scams, privacy, phishing, malware, or safe browsing.";

        private string HandleWhatCanAsk(string input)
            => "You can ask me about:\n• Password safety\n• Scams\n• Privacy\n• Phishing\n• Malware\n• Safe browsing\n• Two-factor authentication";

        private string HandleName(string input)
        {
            if (Memory.HasName)
                return $"Your name is {Memory.Name}. I remember it!";
            return "I don't think you've told me your name yet. Say \"My name is ...\" and I'll remember it.";
        }

        private string HandleFavouriteTopic(string input)
        {
            if (Memory.HasFavouriteTopic)
                return $"You told me you're interested in {_responses.TopicNames[Memory.FavouriteTopic]}. It's a crucial part of staying safe online.";
            return "You haven't told me your favourite cybersecurity topic yet. Try saying \"I'm interested in privacy\".";
        }

        private string HandleFollowUp(string input)
        {
            if (!string.IsNullOrEmpty(Memory.LastTopic))
            {
                string? tip = _responses.GetRandomResponse(Memory.LastTopic);
                return $"Here's more on {_responses.TopicNames[Memory.LastTopic]}: {tip}";
            }
            return "Sure! What topic would you like more information on?";
        }

        private string HandleHelp(string input)
            => "I can help with: passwords, scams, privacy, phishing, malware, safe browsing, and 2FA. Just ask!";

        // ---------- Helpers ----------

        private bool MatchesIntent(string input, string intent)
        {
            switch (intent)
            {
                case "greeting":
                    return input == "hi" || input == "hello" || input == "hey" || input.StartsWith("good ");
                case "howareyou":
                    return input.Contains("how are you");
                case "purpose":
                    return input.Contains("purpose") || input.Contains("what do you do");
                case "whatcanask":
                    return input.Contains("what can i ask") || input.Contains("what can you do");
                case "name":
                    return input.Contains("my name") || input.Contains("what is my name");
                case "favouritetopic":
                    return input.Contains("what is my favourite") || input.Contains("what am i interested in");
                case "followup":
                    return IsFollowUp(input);
                case "help":
                    return input == "help" || input.Contains("help me");
            }
            return false;
        }

        private bool IsFollowUp(string input)
        {
            return input.Contains("another tip")
                || input.Contains("more tip")
                || input.Contains("tell me more")
                || input.Contains("explain more")
                || input.Contains("give me another")
                || input.Contains("more info");
        }

        private string? FindKeyword(string input)
        {
            // Check longer keys first to avoid partial collisions
            var orderedKeys = _responses.KeywordResponses.Keys.OrderByDescending(k => k.Length);
            foreach (var key in orderedKeys)
            {
                if (input.Contains(key))
                    return key;
            }

            // Also handle common synonyms
            if (input.Contains("passwords")) return "password";
            if (input.Contains("scams") || input.Contains("scamming")) return "scam";
            if (input.Contains("phish")) return "phishing";
            if (input.Contains("private")) return "privacy";
            if (input.Contains("virus") || input.Contains("ransomware")) return "malware";
            if (input.Contains("browser") || input.Contains("browse") || input.Contains("surf")) return "browsing";
            if (input.Contains("two factor") || input.Contains("two-factor") || input.Contains("2 factor")) return "2fa";

            return null;
        }

        private void TryCaptureName(string rawInput)
        {
            string lower = rawInput.ToLower();
            string[] triggers = { "my name is ", "i am ", "i'm ", "call me " };
            foreach (var t in triggers)
            {
                int idx = lower.IndexOf(t);
                if (idx >= 0)
                {
                    string rest = rawInput.Substring(idx + t.Length).Trim();
                    string? captured = rest.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(captured) && captured.Length > 1)
                    {
                        Memory.Name = char.ToUpper(captured[0]) + captured.Substring(1).ToLower();
                    }
                    return;
                }
            }
        }

        private void TryCaptureFavouriteTopic(string input)
        {
            if (input.Contains("interested in") || input.Contains("favourite topic") || input.Contains("favorite topic") || input.Contains("i like"))
            {
                foreach (var key in _responses.KeywordResponses.Keys)
                {
                    if (input.Contains(key))
                    {
                        Memory.SetFavouriteTopic(key);
                        return;
                    }
                }
            }
        }

        private string NameSuffix() => Memory.HasName ? $", {Memory.Name}" : "";

        public string GetPersonalisedIntro()
        {
            if (Memory.HasName)
                return $"Nice to meet you, {Memory.Name}! Let's learn how to stay safe online.";
            return "Let's learn how to stay safe online.";
        }
    }
}
