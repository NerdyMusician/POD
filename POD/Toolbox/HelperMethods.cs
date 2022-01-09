using POD.Windows;
using System;
using System.IO;

namespace POD.Toolbox
{
    public static class HelperMethods
    {
        public static void WriteToLogFile(string message, bool notifyUser = false)
        {
            if (notifyUser) { new NotificationDialog(message).ShowDialog(); }
            File.AppendAllText("log.txt", DateTime.Now + ": " + message + "\n");
        }
    }
}
