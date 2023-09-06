namespace FioryApp.Service;

public class LoggerService
{
    public static Task CreateLogFile()
    {
        string file = LogFilePath();

        if (!File.Exists(file))
        {
            using FileStream stream = File.Create(file);
            // No need to await or return anything, just dispose the stream asynchronously
            return stream.DisposeAsync().AsTask();
        }
        // Return a completed task if the file already exists
        return Task.CompletedTask;
    }

    private static string LogFilePath() {
        string folderPath = ConfigFilesService.GetConfigFilesPath("Logs");
        string fileName = "log_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".txt";
        string file = folderPath + Path.DirectorySeparatorChar + fileName;

        return file;
    }

    public static void Info(string message) => AddLog("INFO", message);
    public static void Error(string message) => AddLog("ERROR", message);
    public static void Warning(string message) => AddLog("WARN", message);

    private static void AddLog(string typeLog, string message) {
        string file = LogFilePath();
        string text = DateTime.Now + ": |" + typeLog + "| "+message + Environment.NewLine;
        StreamWriter sw = new StreamWriter(file, true);
        sw.Write(text);
        sw.Close();
    }
}