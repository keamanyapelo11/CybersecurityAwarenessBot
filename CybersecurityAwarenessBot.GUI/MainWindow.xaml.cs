using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityAwarenessBot.GUI.Models;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotEngine _bot = new ChatbotEngine();
        private bool _greeted = false;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var player = new SoundPlayer("Assets/greeting.wav"))
                {
                    player.Play();
                }
            }
            catch
            {
                // Silently ignore if audio missing so app doesn't crash
            }

            AddBotMessage("🔒 Welcome to the Cybersecurity Awareness Bot!");
            AddBotMessage("I'm here to help you stay safe online. Before we begin, what is your name?");
            UserInputBox.Focus();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessUserInput();

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessUserInput();
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var player = new SoundPlayer("Assets/greeting.wav"))
                {
                    player.Play();
                }
            }
            catch
            {
                AddBotMessage("(Voice greeting file not found. Please ensure Assets/greeting.wav exists.)");
            }
        }

        private void ProcessUserInput()
        {
            string input = UserInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AddUserMessage(input);
            UserInputBox.Clear();

            if (!_greeted)
            {
                _bot.Memory.Name = ExtractName(input);
                _greeted = true;
                AddBotMessage(_bot.GetPersonalisedIntro());
                AddBotMessage("You can ask me about:\n• Password safety\n• Scams\n• Privacy\n• Phishing\n• Malware\n• Safe browsing\n• Two-factor authentication");
                return;
            }

            string reply = _bot.GetResponse(input);
            AddBotMessage(reply);
        }

        private string ExtractName(string input)
        {
            string cleaned = input.Trim();
            if (cleaned.Length == 0) return "User";
            return char.ToUpper(cleaned[0]) + cleaned.Substring(1).ToLower();
        }

        private void AddUserMessage(string message)
        {
            var bubble = CreateBubble(message, true);
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        private void AddBotMessage(string message)
        {
            var bubble = CreateBubble(message, false);
            ChatPanel.Children.Add(bubble);
            ScrollToBottom();
        }

        private Border CreateBubble(string message, bool isUser)
        {
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                Foreground = isUser
                    ? new SolidColorBrush(Color.FromRgb(0x0D, 0x1B, 0x2A))
                    : new SolidColorBrush(Color.FromRgb(0xE0, 0xE1, 0xDD))
            };

            var bubble = new Border
            {
                Background = isUser
                    ? new SolidColorBrush(Color.FromRgb(0x00, 0xB4, 0xD8))
                    : new SolidColorBrush(Color.FromRgb(0x0D, 0x1B, 0x2A)),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(isUser ? 80 : 0, 4, isUser ? 0 : 80, 4),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 600,
                Child = textBlock
            };

            var label = new TextBlock
            {
                Text = isUser ? "You" : "🤖 CyberBot",
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = isUser
                    ? new SolidColorBrush(Color.FromRgb(0x2E, 0xC4, 0xB6))
                    : new SolidColorBrush(Color.FromRgb(0xFF, 0xD1, 0x66)),
                Margin = new Thickness(isUser ? 0 : 4, 8, isUser ? 4 : 0, 0),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };

            var container = new StackPanel();
            container.Children.Add(label);
            container.Children.Add(bubble);

            return new Border { Child = container };
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
