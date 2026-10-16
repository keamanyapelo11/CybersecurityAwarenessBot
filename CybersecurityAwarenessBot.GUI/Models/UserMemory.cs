using System.Collections.Generic;

namespace CybersecurityAwarenessBot.GUI.Models
{
    /// <summary>
    /// Stores information the user has shared with the chatbot
    /// so responses can be personalised later in the conversation.
    /// </summary>
    public class UserMemory
    {
        public string Name { get; set; } = "";
        public string FavouriteTopic { get; set; } = "";
        public string LastTopic { get; set; } = "";
        public string LastSentiment { get; set; } = "";
        public int InteractionCount { get; set; } = 0;
        public List<string> TopicsDiscussed { get; set; } = new List<string>();

        public bool HasName => !string.IsNullOrWhiteSpace(Name);
        public bool HasFavouriteTopic => !string.IsNullOrWhiteSpace(FavouriteTopic);

        public void RememberTopic(string topic)
        {
            if (!string.IsNullOrWhiteSpace(topic) && !TopicsDiscussed.Contains(topic))
            {
                TopicsDiscussed.Add(topic);
            }
            LastTopic = topic;
        }

        public void SetFavouriteTopic(string topic)
        {
            FavouriteTopic = topic;
            RememberTopic(topic);
        }
    }
}