using System.IO;

namespace BilibiliSubtitleConverter
{
    public class ProgramConfig
    {
        /// <summary>
        /// Bilibili subtitle file save path.
        /// </summary>
        public string InputDir { get; set; }

        /// <summary>
        /// Converted subtitle file save path.
        /// </summary>
        public string OutputDir { get; set; }

        /// <summary>
        /// If true, program will be output .ass file, otherwise .srt file will be output.
        /// </summary>
        public bool IsASS { get; set; }

        /// <summary>
        /// File extension (to be specified based on IsAss).
        /// </summary>
        /// <remarks>If IsAss is true, it will be ".ass", otherwise it will be ".srt".</remarks>
        public string FileExt { get; set; }

        private ProgramConfig()
        {
            OutputDir = InputDir = Directory.GetCurrentDirectory();
            IsASS = false;
            FileExt = ".srt";
        }

        /// <summary>
        /// Process input parameters.
        /// </summary>
        /// <param name="args">Input parameters</param>
        /// <param name="message">Error message. If it not null, program will be shutdown.</param>
        /// <returns></returns>
        public static ProgramConfig ProcArgs(string[] args, out string message)
        {
            message = null;
            var config = new ProgramConfig();

            // temporary variables
            bool boolVal;
            string strVal;

            // start process...
            for (int i = 0; i < args.Length; i++)
            {
                var item = args[i]?.ToLower();
                switch (item)
                {
                    case "--type":
                        if (!GetTypeIsOrNotIsASS(args, i, out boolVal, out message))
                            return config;
                        config.IsASS = boolVal;
                        config.FileExt = boolVal ? ".ass" : ".srt";
                        break;
                    case "--input":
                        if (!GetDirPath(args, i, out strVal, out message))
                        {
                            message += " (--input)";
                            return config;
                        }
                        config.InputDir = strVal;
                        break;
                    case "--output":
                        if (!GetDirPath(args, i, out strVal, out message))
                        {
                            message += " (--output)";
                            return config;
                        }
                        config.OutputDir = strVal;
                        break;
                    default:
                        break;
                }
            }

            return config;
        }

        private static bool GetDirPath(string[] args, int paramIndex, out string value, out string message)
        {
            if (!GetParameterValue(args, paramIndex, out value))
            {
                message = "The path is empty.";
                return false;
            }

            message = null;
            return true;
        }

        private static bool GetTypeIsOrNotIsASS(string[] args, int paramIndex, out bool value, out string message)
        {
            value = false;

            string strVal;
            if (!GetParameterValue(args, paramIndex, out strVal))
            {
                message = "Invalid type.";
                return false;
            }

            switch (strVal?.ToLower())
            {
                case "a":
                case "ass":
                    value = true;
                    message = null;
                    return true;
                case "s":
                case "srt":
                    //value = false;
                    message = null;
                    return true;
                default:
                    message = "Invalid type.";
                    return false;
            }
        }

        private static bool GetParameterValue(string[] args, int paramIndex, out string value)
        {
            int valIndex = paramIndex + 1;
            if (valIndex >= args.Length)
            {
                value = null;
                return false;
            }

            value = args[valIndex];
            return true;
        }
    }
}
