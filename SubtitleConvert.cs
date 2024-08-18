using System;
using System.Text;
using System.Linq;

namespace BilibiliSubtitleConverter
{
    public static class SubtitleConvert
    {
        const string ENGCHAR = "abcdefghijklmnopqrstuvwxyz ";
        const string ENGCHAR_BIG = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ToASS(SourceFile source)
        {
            // Refer: https://nic.upc.edu.cn/2012/0321/c183a4389/pagem.htm

            StringBuilder content = new StringBuilder();

            //============================== [Script Info]
            content.AppendLine("[Script Info]");
            content.AppendLine("Title: Default Aegisub file");
            content.AppendLine("ScriptType: v4.00+");
            content.AppendLine("WrapStyle: 0");
            content.AppendLine("ScaledBorderAndShadow: yes");
            content.AppendLine("YCbCr Matrix: None");
            content.AppendLine("PlayResX: Directdraw");
            content.AppendLine("PlayResY: Directdraw");
            content.AppendLine();

            //============================== [V4+ Styles]
            string fontColor = source.FontColor.Substring(5, 2) + source.FontColor.Substring(3, 2) + source.FontColor.Substring(1, 2);
            content.AppendLine("[V4+ Styles]");
            content.AppendLine("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding");
            content.AppendLine($"Style: TXT,Noto Sans,18,&H00{fontColor},&H000000FF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,0,0,2,10,10,20,1");
            content.AppendLine("Style: Default,Arial,20,&H00FFFFFF,&H000000FF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,2,2,2,10,10,10,1");
            content.AppendLine();

            //============================== [Events]
            content.AppendLine("[Events]");
            content.AppendLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");
            string bgColor = source.BackgroundColor.Substring(5, 2) + source.BackgroundColor.Substring(3, 2) + source.BackgroundColor.Substring(1, 2);
            string bgAlpha = Convert.ToInt32(source.BackgroundAlpha / (1f / 255f)).ToString("X");

            foreach (var item in source.Body)
            {
                var startTime = new TimeSpan(Convert.ToInt64(item.From * 10000000)).ToString("hh\\:mm\\:ss\\.ff");
                var endTime = new TimeSpan(Convert.ToInt64(item.To * 10000000)).ToString("hh\\:mm\\:ss\\.ff");

                content.AppendLine($"Dialogue: 1,{startTime},{endTime},TXT,,0,0,0,,{item.Content}");

                int engCount = item.Content.Count(w => ENGCHAR.Contains(w));
                int engBigCount = item.Content.Count(w => ENGCHAR_BIG.Contains(w));
                int width = engCount * 4 + engBigCount * 6 + (item.Content.Length - engCount - engBigCount) * 12 + 10;
                content.AppendLine($"Dialogue: 0,{startTime},{endTime},Default,,0,0,0,,{{\\a2\\an7\\p1\\shad0\\bord0\\c&H{bgColor}\\alpha&H{bgAlpha}}}m 0 -8 l {width} -8 l {width} 15 l 0 15 l 0 -8"); // font height is 16?
            }
            content.AppendLine();

            return content.ToString();
        }

        public static string ToSRT(SourceFile source)
        {
            // Refer: https://ale5000.altervista.org/subtitles.htm
            // Refer: https://www.cnblogs.com/tocy/p/subtitle-format-srt.html

            StringBuilder content = new StringBuilder(source.Body.Length * 4);
            for (int i = 0; i < source.Body.Length; i++)
            {
                var item = source.Body[i];

                // TimeSpan(long ticks)
                // 1 tick = 100 ns
                // 1 ms = 10000 ticks
                // 1 s = 10000000 ticks
                // 1s = 1000ms, 1ms = 1000μs, 1μs = 1000ns
                var startTime = new TimeSpan(Convert.ToInt64(item.From * 10000000));
                var endTime = new TimeSpan(Convert.ToInt64(item.To * 10000000));

                content.AppendLine((i + 1).ToString());
                content.AppendLine($"{startTime:hh\\:mm\\:ss\\,fff} --> {endTime:hh\\:mm\\:ss\\,fff}");
                content.AppendLine(item.Content);
                content.AppendLine(); // DO NOT DELETE
            }

            return content.ToString();
        }
    }
}
