using System.Collections.Generic;

namespace CybersecurityAwarenessBot.GUI.Models
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
    }

    public class QuizManager
    {
        public List<QuizQuestion> Questions { get; private set; } = new List<QuizQuestion>();
        public int CurrentQuestionIndex { get; set; } = 0;
        public int Score { get; set; } = 0;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            Questions.Add(new QuizQuestion { Question = "What should you do if you receive an email asking for your password?", Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" }, CorrectIndex = 2, Explanation = "Reporting phishing emails helps prevent scams." });
            Questions.Add(new QuizQuestion { Question = "True or False: It is safe to use the same password for all your accounts.", Options = new List<string> { "A) True", "B) False" }, CorrectIndex = 1, Explanation = "If one site is breached, all your accounts become vulnerable." });
            Questions.Add(new QuizQuestion { Question = "What does '2FA' stand for?", Options = new List<string> { "A) Two-Factor Authentication", "B) Two-Factor Access", "C) Two-File Authorization", "D) Total File Access" }, CorrectIndex = 0, Explanation = "2FA adds an extra layer of security." });
            Questions.Add(new QuizQuestion { Question = "True or False: Public Wi-Fi is always secure.", Options = new List<string> { "A) True", "B) False" }, CorrectIndex = 1, Explanation = "Public Wi-Fi is often unsecured. Use a VPN." });
            Questions.Add(new QuizQuestion { Question = "Which of these is a sign of a phishing email?", Options = new List<string> { "A) Urgent language", "B) Spelling mistakes", "C) Unknown sender", "D) All of the above" }, CorrectIndex = 3, Explanation = "All these are common red flags." });
            Questions.Add(new QuizQuestion { Question = "What is malware?", Options = new List<string> { "A) A type of hardware", "B) Malicious software", "C) A secure network", "D) A strong password" }, CorrectIndex = 1, Explanation = "Malware is software designed to harm or exploit." });
            Questions.Add(new QuizQuestion { Question = "True or False: You should click links in emails from unknown senders.", Options = new List<string> { "A) True", "B) False" }, CorrectIndex = 1, Explanation = "Never click suspicious links." });
            Questions.Add(new QuizQuestion { Question = "What is social engineering?", Options = new List<string> { "A) Building social networks", "B) Manipulating people to reveal info", "C) Engineering software", "D) A type of firewall" }, CorrectIndex = 1, Explanation = "It relies on human error rather than technical hacking." });
            Questions.Add(new QuizQuestion { Question = "True or False: Antivirus software alone is enough to keep you safe.", Options = new List<string> { "A) True", "B) False" }, CorrectIndex = 1, Explanation = "You also need safe browsing habits and updates." });
            Questions.Add(new QuizQuestion { Question = "How often should you update your software?", Options = new List<string> { "A) Never", "B) Once a year", "C) Regularly/As soon as updates are available", "D) Only when it breaks" }, CorrectIndex = 2, Explanation = "Updates patch security vulnerabilities." });
        }
    }
}
