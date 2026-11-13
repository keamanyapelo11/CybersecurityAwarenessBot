# Cybersecurity Awareness Bot - Part 3 POE

A WPF chatbot that teaches users about cybersecurity through a friendly GUI, with keyword recognition, randomized tips, memory recall, sentiment detection, and advanced Part 3 features.

## Features

- **GUI (WPF)** — Styled interface with chat bubbles, tabbed navigation, and color palette
- **Voice greeting** — Plays `Assets/greeting.wav` at startup (Replay button available)
- **Keyword recognition** — Recognises password, scam, privacy, phishing, malware, browsing, 2FA
- **Random responses** — Multiple tips per topic, randomly selected
- **Conversation flow** — Handles "another tip", "tell me more", "explain more"
- **Memory & recall** — Remembers name and favourite topic; personalises replies
- **Sentiment detection** — Detects worried, frustrated, curious, happy; adds empathy
- **Error handling** — Default fallback for unknown inputs (no crashes)
- **OOP + Delegates** — `ChatbotEngine`, `ResponseManager`, `UserMemory`, `SentimentDetector`

### Part 3 New Features
- **Task Assistant (MySQL):** Users can add, view, complete, and delete cybersecurity tasks. All tasks are saved to a local MySQL database (`CyberBotDB`).
- **Mini-Game (Quiz):** A 10-question cybersecurity quiz with immediate feedback, scoring, and NLP-based answer recognition (A, B, C, D).
- **NLP Simulation:** The chatbot uses string manipulation to detect commands like "add task", "start quiz", and "show activity log", passing them to the GUI.
- **Activity Log:** Keeps a timestamped record of the last 10 actions (tasks added, quiz started, etc.) viewable via the chat.

## Project Structure
```text
CybersecurityAwarenessBot/
├── CybersecurityAwarenessBot.slnx # solution
├── CybersecurityAwarenessBot.GUI/ # Parts 2 & 3 WPF app
│ ├── App.xaml
│ ├── MainWindow.xaml
│ ├── MainWindow.xaml.cs
│ ├── Assets/
│ │ └── greeting.wav
│ └── Models/
│ ├── ChatbotEngine.cs
│ ├── UserMemory.cs
│ ├── SentimentDetector.cs
│ ├── ResponseManager.cs
│ ├── TaskItem.cs
│ ├── ActivityLog.cs
│ └── QuizManager.cs
└── ConsoleApp1/ # Part 1 console (reference)
Database Setup (MySQL)
Ensure MySQL Server and MySQL Workbench are installed.

Open MySQL Workbench and run the following SQL script to create the database:

sql
CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    ReminderDate DATETIME NULL,
    IsCompleted BOOLEAN DEFAULT FALSE
);
Open MainWindow.xaml.cs and update the _connectionString variable with your MySQL username and password (e.g., Pwd=admin123;).

Usage Examples
Input	Response type
My name is Alex	Bot remembers your name
I'm interested in privacy	Bot saves favourite topic
Tell me about password safety	Random password tip
add task to enable 2FA	Switches to Task tab and pre-fills title
start quiz	Begins the 10-question quiz
show activity log	Displays recent bot actions
YouTube Demo
https://youtu.be/eeq4P-6_vj8

Author
Keamogetswe Manyapelo, ST10498901