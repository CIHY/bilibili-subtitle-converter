using System;
using System.IO;
using System.Text.Json;

namespace BilibiliSubtitleConverter
{
    public class Program
    {
        const string PROGRAM_NAME = "BILIBILI subtitle converter";
        const string PROGRAM_VERSION = "1.0";

        public static void Main(string[] args)
        {
            /** Parameters:
            * --type <value=a/ass/s/srt>: Specify whether the output is .ass or .srt (default is 'srt').
            * --input <path to source>: Source file(s) save path (default is program startup directory).
            * --output <path to result>: Subtitle file(s) save path (default is program startup directory).
            * --help: Print help information and exit.
            * 
            */
            LogInfo(PROGRAM_NAME);
            LogInfo("ver " + PROGRAM_VERSION);
            LogInfo("");
            if (args != null && args.Length != 0 && "--help".Equals(args[0]?.ToLower()))
            {
                LogInfo("Parameters:");
                LogInfo("--type <value=a/ass/s/srt>: Specify whether the output is .ass or .srt (default is 'srt').");
                LogInfo("--input <path to source>: Source file(s) save path (default is program startup directory).");
                LogInfo("--output <path to result>: Subtitle file(s) save path (default is program startup directory).");
                LogInfo("--help: Print help information and exit.");
                return;
            }

            var config = ProgramConfig.ProcArgs(args, out string argErr);
            if (!string.IsNullOrEmpty(argErr))
            {
                LogError(argErr);
                return;
            }

            var biliSubfiles = Directory.GetFiles(config.InputDir, "*.json");
            LogInfo("Program working in: " + config.InputDir);
            LogInfo("Converted file will be saved to: " + config.InputDir);
            LogInfo("Output file format: " + config.FileExt);
            LogInfo(biliSubfiles.Length + " file(s) detected.");
            LogInfo("==========================================");

            // Start...
            foreach (var item in biliSubfiles)
            {
                try
                {
                    LogInfo("+++++++++++++++++++++");
                    LogInfo("Read file: " + Path.GetFileName(item));

                    // Read file content.
                    string fileContent = File.ReadAllText(item);
                    var source = JsonSerializer.Deserialize<SourceFile>(fileContent);
                    if (source == null || source.Body == null)
                    {
                        LogWarn("Not Bilibili's subtitle file, skip.");
                        continue;
                    }

                    // Convert file content.
                    LogInfo("Converting file content...");
                    string result = config.IsASS ? SubtitleConvert.ToASS(source) : SubtitleConvert.ToSRT(source);

                    // Generate output file name and save result.
                    string fileName = Path.GetFileNameWithoutExtension(item) + config.FileExt;
                    LogInfo("Saving file: " + fileName);
                    File.WriteAllText(Path.Combine(config.OutputDir, fileName), result);
                    LogInfo("Ok.");
                }
                catch (JsonException jsonEx)
                {
                    // Not Bilibili's subtitle file, maybe.
                    LogWarn($"Not Bilibili's subtitle file, skip. ({jsonEx.Message})");
                }
                catch (Exception ex)
                {
                    // Unknow error.
                    LogError($"Unknow error. ({ex.GetType().FullName}: {ex.Message})");
                }
            }
        }

        static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        static void LogWarn(string message) => LogInfo("Warning: " + message);
        static void LogError(string message) => LogInfo("Error: " + message);
    }
}
