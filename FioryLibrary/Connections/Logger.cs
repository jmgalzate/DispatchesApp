using System;
using FioryLibrary.Sales;
using System.Text;
using System.IO;

namespace FioryLibrary.Connections;

public static class Logger
{

    public static string logFilePath() {
        string folderPath = ConfigFiles.getConfigFilesPath("Logs");
        string fileName = "log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
        string file = folderPath + Path.DirectorySeparatorChar + fileName;

        if (!File.Exists(file))
            System.IO.File.Create(file);

        return file;
    }

	public static void removeOldFiles() { }

	public static void info(string message) => addLog("INFO", message);
	public static void error(string message) => addLog("ERROR", message);
	public static void warning(string message) => addLog("WARN", message);

    private static void addLog(string typeLog, string message) {

        string file = Logger.logFilePath();

        string text = DateTime.Now + ": |" + typeLog + "| "+message + Environment.NewLine;
		StreamWriter sw = new StreamWriter(file, true);
		sw.Write(text);
		sw.Close();
    }
}

