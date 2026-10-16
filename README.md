# Cybersecurity Awareness Bot

A WPF chatbot that teaches users about cybersecurity through a friendly GUI,
with keyword recognition, randomised tips, memory recall, and sentiment detection.

## Features

- **GUI (WPF)** — styled interface with chat bubbles and colour palette
- **Voice greeting** — plays `Assets/greeting.wav` at startup (Replay button available)
- **Keyword recognition** — recognises password, scam, privacy, phishing, malware, browsing, 2FA
- **Random responses** — multiple tips per topic, randomly selected
- **Conversation flow** — handles "another tip", "tell me more", "explain more"
- **Memory & recall** — remembers name and favourite topic; personalises replies
- **Sentiment detection** — detects worried, frustrated, curious, happy; adds empathy
- **Error handling** — default fallback for unknown inputs (no crashes)
- **OOP + Delegates** — `ChatbotEngine`, `ResponseManager`, `UserMemory`, `SentimentDetector`

## Project Structure
CybersecurityAwarenessBot/
├── CybersecurityAwarenessBot.slnx # solution
├── CybersecurityAwarenessBot.GUI/ # Part 2 WPF app
│ ├── App.xaml
│ ├── MainWindow.xaml
│ ├── MainWindow.xaml.cs
│ ├── Assets/
│ │ └── greeting.wav
│ └── Models/
│ ├── ChatbotEngine.cs
│ ├── UserMemory.cs
│ ├── SentimentDetector.cs
│ └── ResponseManager.cs
└── ConsoleApp1/ # Part 1 console (reference)


## Setup

1. Open `CybersecurityAwarenessBot.slnx` in Visual Studio 2022 (with **.NET desktop development** workload installed).
2. Set `CybersecurityAwarenessBot.GUI` as the startup project.
3. Build (Ctrl+Shift+B) and run (F5).
4. Ensure `Assets/greeting.wav` exists and its properties are set to:
   - **Build Action** = Content
   - **Copy to Output Directory** = Copy if newer

## Usage Examples

| Input | Response type |
|---|---|
| `My name is Alex` | Bot remembers your name |
| `I'm interested in privacy` | Bot saves favourite topic |
| `Tell me about password safety` | Random password tip |
| `Give me a phishing tip` | Random phishing tip |
| `I'm worried about online scams` | Empathetic prefix + scam tip |
| `Give me another tip` | More on same topic |
| `asdfgh` | Graceful fallback (no crash) |

## YouTube Demo

https://youtu.be/eeq4P-6_vj8

## Author

Keamogetswe Manyapelo, ST104989