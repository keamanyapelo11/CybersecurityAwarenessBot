using CybersecurityAwarenessBot.GUI.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityAwarenessBot.GUI
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotEngine _bot = new ChatbotEngine();
        private bool _greeted = false;

        // --- DATABASE CONNECTION STRING ---
        
        private readonly string _connectionString = "Server=localhost;Database=CyberBotDB;Uid=root;Pwd=admin123;";

        private QuizManager _quizManager;
        private bool _isQuizActive = false;

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
            catch { }

            AddBotMessage("🔒 Welcome to the Cybersecurity Awareness Bot!");
            AddBotMessage("I'm here to help you stay safe online. Before we begin, what is your name?");
            UserInputBox.Focus();
            LoadTasks();
        }

        // --- TASK DB LOGIC ---
        private void LoadTasks()
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();
                string query = "SELECT Id, Title, Description, ReminderDate, IsCompleted FROM Tasks WHERE IsCompleted = 0";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                var tasks = new List<TaskItem>();
                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                        ReminderDate = reader.IsDBNull("ReminderDate") ? null : reader.GetDateTime("ReminderDate")
                    });
                }
                TaskListBox.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                AddBotMessage($"[DB Error] Could not load tasks: {ex.Message}");
            }
        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleBox.Text)) return;

            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();
                string query = "INSERT INTO Tasks (Title, Description, ReminderDate) VALUES (@title, @desc, @date)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", TaskTitleBox.Text);
                cmd.Parameters.AddWithValue("@desc", TaskDescBox.Text);
                cmd.Parameters.AddWithValue("@date", TaskDatePicker.SelectedDate ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();

                ActivityLog.Instance.AddLog($"Task added to DB: {TaskTitleBox.Text}");
                AddBotMessage($"Task '{TaskTitleBox.Text}' successfully added to the database!");

                TaskTitleBox.Clear();
                TaskDescBox.Clear();
                TaskDatePicker.SelectedDate = null;
                LoadTasks();
            }
            catch (Exception ex)
            {
                AddBotMessage($"[DB Error] Could not add task: {ex.Message}");
            }
        }

        private void DeleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                try
                {
                    using var conn = new MySqlConnection(_connectionString);
                    conn.Open();
                    string query = "DELETE FROM Tasks WHERE Id = @id";
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();

                    ActivityLog.Instance.AddLog($"Task ID {taskId} deleted.");
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    AddBotMessage($"[DB Error] Could not delete task: {ex.Message}");
                }
            }
        }

        private void CompleteTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                try
                {
                    using var conn = new MySqlConnection(_connectionString);
                    conn.Open();
                    string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id";
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();

                    ActivityLog.Instance.AddLog($"Task ID {taskId} marked as completed.");
                    LoadTasks();
                }
                catch (Exception ex)
                {
                    AddBotMessage($"[DB Error] Could not update task: {ex.Message}");
                }
            }
        }

        // --- CHAT & NLP LOGIC ---
        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessUserInput();

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ProcessUserInput();
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
                AddBotMessage("You can ask me about:\n• Password safety\n• Scams\n• Privacy\n• Phishing\n• Malware\n• Safe browsing\n• Two-factor authentication\n\nOr type 'start quiz', 'add task', or 'show activity log'!");
                return;
            }

            if (_isQuizActive)
            {
                HandleQuizAnswer(input);
                return;
            }

            string reply = _bot.GetResponse(input);

            if (reply.StartsWith("CMD:"))
            {
                HandleCommand(reply);
                return;
            }

            AddBotMessage(reply);
        }

        private void HandleCommand(string command)
        {
            if (command == "CMD:START_QUIZ")
            {
                StartQuiz();
            }
            else if (command == "CMD:SHOW_LOG")
            {
                var logs = ActivityLog.Instance.GetRecentLogs();
                AddBotMessage("Here are your recent activities:\n" + string.Join("\n", logs));
            }
            else if (command.StartsWith("CMD:ADD_TASK:"))
            {
                string taskName = command.Substring("CMD:ADD_TASK:".Length);
                TaskTitleBox.Text = taskName;
                AddBotMessage($"I've prepared a task called '{taskName}'. Please go to the 'Task Assistant' tab to set the description and reminder date, then click 'Add Task'.");

                // Switch to Task tab (Index 1)
                if (Content is Grid mainGrid && mainGrid.Children[1] is TabControl tabControl)
                {
                    tabControl.SelectedIndex = 1;
                }
            }
        }

        // --- QUIZ LOGIC ---
        private void StartQuiz()
        {
            _quizManager = new QuizManager();
            _quizManager.CurrentQuestionIndex = 0;
            _quizManager.Score = 0;
            _isQuizActive = true;
            AskQuizQuestion();
        }

        private void AskQuizQuestion()
        {
            if (_quizManager.CurrentQuestionIndex >= _quizManager.Questions.Count)
            {
                EndQuiz();
                return;
            }

            var q = _quizManager.Questions[_quizManager.CurrentQuestionIndex];
            string questionText = $"Question {_quizManager.CurrentQuestionIndex + 1}/{_quizManager.Questions.Count}:\n{q.Question}\n\n";
            foreach (var opt in q.Options)
            {
                questionText += opt + "\n";
            }
            AddBotMessage(questionText);
        }

        private void HandleQuizAnswer(string input)
        {
            var q = _quizManager.Questions[_quizManager.CurrentQuestionIndex];
            int selectedIndex = -1;

            input = input.ToUpper().Trim();
            if (input == "A" || input == "1") selectedIndex = 0;
            else if (input == "B" || input == "2") selectedIndex = 1;
            else if (input == "C" || input == "3") selectedIndex = 2;
            else if (input == "D" || input == "4") selectedIndex = 3;

            if (selectedIndex == -1)
            {
                AddBotMessage("Please answer with A, B, C, or D.");
                return;
            }

            if (selectedIndex == q.CorrectIndex)
            {
                AddBotMessage("✅ Correct! " + q.Explanation);
                _quizManager.Score++;
            }
            else
            {
                AddBotMessage($"❌ Incorrect. The correct answer was {q.Options[q.CorrectIndex]}. {q.Explanation}");
            }

            _quizManager.CurrentQuestionIndex++;
            AskQuizQuestion();
        }

        private void EndQuiz()
        {
            _isQuizActive = false;
            string feedback = _quizManager.Score >= 8 ? "Great job! You're a cybersecurity pro!" : "Keep learning to stay safe online!";
            AddBotMessage($"Quiz Completed! Your score: {_quizManager.Score}/{_quizManager.Questions.Count}\n{feedback}");
            ActivityLog.Instance.AddLog($"Quiz completed. Score: {_quizManager.Score}/{_quizManager.Questions.Count}");
        }

        // --- HELPERS ---
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
                AddBotMessage("(Voice greeting file not found.)");
            }
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
                Foreground = isUser ? new SolidColorBrush(Color.FromRgb(0x0D, 0x1B, 0x2A)) : new SolidColorBrush(Color.FromRgb(0xE0, 0xE1, 0xDD))
            };

            var bubble = new Border
            {
                Background = isUser ? new SolidColorBrush(Color.FromRgb(0x00, 0xB4, 0xD8)) : new SolidColorBrush(Color.FromRgb(0x0D, 0x1B, 0x2A)),
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
                Foreground = isUser ? new SolidColorBrush(Color.FromRgb(0x2E, 0xC4, 0xB6)) : new SolidColorBrush(Color.FromRgb(0xFF, 0xD1, 0x66)),
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