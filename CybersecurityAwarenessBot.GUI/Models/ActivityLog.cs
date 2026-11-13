using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot.GUI.Models
{
    public class ActivityLog
    {
        private static ActivityLog _instance;
        public static ActivityLog Instance => _instance ??= new ActivityLog();

        private readonly List<string> _logs = new List<string>();

        public void AddLog(string action)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            _logs.Add($"[{timestamp}] {action}");
        }

        public List<string> GetRecentLogs(int count = 10)
        {
            int start = Math.Max(0, _logs.Count - count);
            return _logs.GetRange(start, _logs.Count - start);
        }
    }
}
