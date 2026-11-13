Cybersecurity Awareness Bot - Part 3 POE

A WPF chatbot that teaches users about cybersecurity through a friendly GUI, with keyword recognition, randomized tips, memory recall, sentiment detection, and advanced Part 3 features.

FEATURES

- GUI (WPF): Styled interface with chat bubbles, tabbed navigation, and color palette.
- Voice greeting: Plays Assets/greeting.wav at startup (Replay button available).
- Keyword recognition: Recognises password, scam, privacy, phishing, malware, browsing, 2FA.
- Random responses: Multiple tips per topic, randomly selected.
- Conversation flow: Handles "another tip", "tell me more", "explain more".
- Memory & recall: Remembers name and favourite topic; personalises replies.
- Sentiment detection: Detects worried, frustrated, curious, happy; adds empathy.
- Error handling: Default fallback for unknown inputs (no crashes).

Part 3 Advanced Features:
- Task Assistant (MySQL): Add, view, complete, and delete cybersecurity tasks. All tasks are saved to a local MySQL database.
- Mini-Game (Quiz): A 10-question cybersecurity quiz with immediate feedback, scoring, and NLP-based answer recognition (A, B, C, D).
- NLP Simulation: The chatbot uses string manipulation to detect commands like "add task", "start quiz", and "show activity log".
- Activity Log: Keeps a timestamped record of the last 10 actions (tasks added, quiz started, etc.).

DATABASE SETUP (MYSQL)

1. Ensure MySQL Server and MySQL Workbench are installed.
2. Open MySQL Workbench and run the following SQL script to create the database:

CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderDate DATETIME NULL,
    IsCompleted BOOLEAN DEFAULT FALSE
);

3. Open MainWindow.xaml.cs in Visual Studio and update the _connectionString variable with your MySQL username and password (e.g., Pwd=admin123;).

USAGE EXAMPLES

Input: "My name is Alex"
Action: Bot remembers your name.

Input: "I'm interested in privacy"
Action: Bot saves your favourite topic.

Input: "Tell me about password safety"
Action: Returns a random password tip.

Input: "add task to enable 2FA"
Action: Switches to the Task tab and pre-fills the title.

Input: "start quiz"
Action: Begins the 10-question mini-game.

Input: "show activity log"
Action: Displays the last 10 actions the bot has taken.

PROJECT STRUCTURE

CybersecurityAwarenessBot/
  CybersecurityAwarenessBot.slnx
  CybersecurityAwarenessBot.GUI/
    App.xaml
    MainWindow.xaml
    MainWindow.xaml.cs
    Assets/greeting.wav
    Models/
      ChatbotEngine.cs
      UserMemory.cs
      SentimentDetector.cs
      ResponseManager.cs
      TaskItem.cs
      ActivityLog.cs
      QuizManager.cs
  ConsoleApp1/

YOUTUBE DEMO
https://youtu.be/eeq4P-6_vj8

AUTHOR
Keamogetswe Manyapelo, ST10498901