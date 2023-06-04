using System;
using FioryLibrary.Sales;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;

namespace FioryLibrary.Connections;

public static class Logger
{

	public static Task createLogFile()
	{
		string file = logFilePath();

		if (!File.Exists(file))
		{
			using FileStream stream = File.Create(file);
			// No need to await or return anything, just dispose the stream asynchronously
			return stream.DisposeAsync().AsTask();
		}

		// Return a completed task if the file already exists
		return Task.CompletedTask;
	}

	private static string logFilePath() {
        string folderPath = ConfigFiles.getConfigFilesPath("Logs");
        string fileName = "log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
        string file = folderPath + Path.DirectorySeparatorChar + fileName;

        return file;
    }

	public static void info(string message) => addLog("INFO", message);
	public static void error(string message) => addLog("ERROR", message);
	public static void warning(string message) => addLog("WARN", message);

    private static void addLog(string typeLog, string message) {
	    string file = logFilePath();
        string text = DateTime.Now + ": |" + typeLog + "| "+message + Environment.NewLine;
		StreamWriter sw = new StreamWriter(file, true);
		sw.Write(text);
		sw.Close();
    }
}

