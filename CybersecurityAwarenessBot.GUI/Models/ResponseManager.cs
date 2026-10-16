using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot.GUI.Models
{
    /// <summary>
    /// Central store for keyword responses and random tip lists.
    /// Using dictionaries + lists keeps the code optimised and easy to expand.
    /// </summary>
    public class ResponseManager
    {
        private readonly Random _random = new Random();

        // Multiple responses per keyword -> random selection
        public Dictionary<string, List<string>> KeywordResponses { get; } =
            new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
            {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details like your name or birthday.",
                "A password manager can generate and store complex passwords so you don't have to memorise them all.",
                "Enable two-factor authentication (2FA) wherever possible — it adds a crucial extra layer of security.",
                "Never reuse the same password across multiple sites. If one site is breached, all your accounts become vulnerable."
            },
                ["scam"] = new List<string>
            {
                "If an offer sounds too good to be true, it probably is. Scammers rely on urgency and emotion.",
                "Never send money or gift cards to someone you met online and haven't met in person.",
                "Check the sender's email address carefully — scammers often mimic trusted brands with small spelling changes.",
                "Hang up on unsolicited calls asking for banking details or remote access to your computer."
            },
                ["privacy"] = new List<string>
            {
                "Review the privacy settings on your social media accounts regularly — defaults are often too open.",
                "Be mindful of what you share online. Personal details can be used for identity theft.",
                "Use a VPN on public Wi-Fi to encrypt your traffic and protect your privacy.",
                "Consider using privacy-focused browsers and search engines to reduce tracking."
            },
                ["phishing"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Hover over links before clicking to see the real destination URL.",
                "Legitimate companies will never ask for your password via email.",
                "Look for spelling mistakes and generic greetings — common signs of phishing."
            },
                ["malware"] = new List<string>
            {
                "Keep your operating system and antivirus software updated to defend against malware.",
                "Avoid downloading software from unofficial websites — it's a common malware source.",
                "Be wary of email attachments from unknown senders, especially .exe or .zip files."
            },
                ["browsing"] = new List<string>
            {
                "For safe browsing, avoid suspicious websites and links, check that websites use HTTPS, and keep your browser updated.",
                "Use an ad-blocker to reduce exposure to malicious ads (malvertising).",
                "Log out of accounts when you're done, especially on shared computers."
            },
                ["2fa"] = new List<string>
            {
                "Two-factor authentication is one of the best ways to protect your accounts. Use an authenticator app rather than SMS when possible.",
                "Never share your 2FA codes with anyone — not even someone claiming to be from support."
            }
            };

        // Topic display names
        public Dictionary<string, string> TopicNames { get; } = new Dictionary<string, string>
        {
            ["password"] = "Password Safety",
            ["scam"] = "Scams",
            ["privacy"] = "Privacy",
            ["phishing"] = "Phishing",
            ["malware"] = "Malware",
            ["browsing"] = "Safe Browsing",
            ["2fa"] = "Two-Factor Authentication"
        };

        public string? GetRandomResponse(string keyword)
        {
            if (KeywordResponses.ContainsKey(keyword) && KeywordResponses[keyword].Count > 0)
            {
                int index = _random.Next(KeywordResponses[keyword].Count);
                return KeywordResponses[keyword][index];
            }
            return null;
        }

        public bool IsKnownKeyword(string keyword) => KeywordResponses.ContainsKey(keyword);
    }
}