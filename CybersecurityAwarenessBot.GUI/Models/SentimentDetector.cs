using System.Collections.Generic;
using System.Linq;

namespace CybersecurityAwarenessBot.GUI.Models
{
    /// <summary>
    /// Detects simple sentiment from user input so the chatbot can
    /// adjust its tone (worried, curious, frustrated, happy, neutral).
    /// </summary>
    public class SentimentDetector
    {
        private readonly Dictionary<string, string[]> _sentimentKeywords =
            new Dictionary<string, string[]>
        {
            { "worried",    new[] { "worried", "scared", "afraid", "nervous", "anxious", "fear", "concerned", "unsure" } },
            { "frustrated", new[] { "frustrated", "annoyed", "angry", "confused", "stuck", "hate", "terrible", "useless" } },
            { "curious",    new[] { "curious", "wondering", "interested", "want to know", "tell me", "how does", "what is", "explain" } },
            { "happy",      new[] { "happy", "great", "awesome", "good", "excited", "love", "thanks", "thank you" } }
        };

        public string Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "neutral";

            string lower = input.ToLower();

            foreach (var sentiment in _sentimentKeywords)
            {
                if (sentiment.Value.Any(k => lower.Contains(k)))
                    return sentiment.Key;
            }

            return "neutral";
        }

        /// <summary>
        /// Returns an empathetic prefix that the chatbot prepends
        /// to its response so the reply feels natural.
        /// </summary>
        public string GetEmpathyPrefix(string sentiment, UserMemory memory)
        {
            string name = memory.HasName ? $", {memory.Name}" : "";

            switch (sentiment)
            {
                case "worried":
                    return $"It's completely understandable to feel that way{name}. Cyber threats can be scary. Let me share something helpful: ";
                case "frustrated":
                    return $"I hear you{name}, and I'm sorry this feels overwhelming. Let's take it one step at a time. ";
                case "curious":
                    return $"That's a great question{name}! I love that you're curious. ";
                case "happy":
                    return $"That's wonderful to hear{name}! ";
                default:
                    return "";
            }
        }
    }
}