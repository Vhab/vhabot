using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace AoLib.Utils
{
    public static class HTML
    {
        private enum StripState
        {
            Text,
            InsideTag,
            InsideString
        }

        public static readonly String ItemLink = "<a href={0}itemref://{1}/{2}/{3}{0}{4}>";
        public static readonly String TextLink = "<a href={0}text://{1}{0}{2}>";
        public static readonly String CommandLink = "<a href={0}chatcmd://{1}{0}{2}>";
        public static readonly String LinkEnd = "</a>";
        public static readonly String CleanLink = " style={0}text-decoration:none{0}";
        public static readonly String ColorStart = "<font color=#{0}>";
        public static readonly String ColorEnd = "</font>";
        public static readonly String ImgIcon = "<img src=rdb://{0}>";
        public static readonly String ImgGui = "<img src=tdb://id:{0}>";
        public static readonly String AlignStart = "<div align='{0}'>";
        public static readonly String AlignEnd = "</div>";
        public static readonly String UnderlineStart = "<u>";
        public static readonly String UnderlineEnd = "</u>";

        public static string CreateItem(string name, int lowID, int highID, int QL) { return CreateItem(name, lowID, highID, QL, false, "'"); }
        public static string CreateItem(string name, int lowID, int highID, int QL, bool disableStyle) { return CreateItem(name, lowID, highID, QL, disableStyle, "'"); }
        public static string CreateItem(string name, int lowID, int highID, int QL, bool disableStyle, string quotes)
        {
            if (lowID > highID)
            {
                int tmpID = lowID;
                lowID = highID;
                highID = tmpID;
            }
            return String.Format("{0}{1}{2}", CreateItemStart(lowID, highID, QL, disableStyle, quotes), EscapeString(name), CreateLinkEnd());
        }

        public static string CreateItemStart(int lowID, int highID, int QL) { return CreateItemStart(lowID, highID, QL, false, "'"); }
        public static string CreateItemStart(int lowID, int highID, int QL, bool disableStyle) { return CreateItemStart(lowID, highID, QL, disableStyle, "'"); }
        public static string CreateItemStart(int lowID, int highID, int QL, bool disableStyle, string quotes)
        {
            if (lowID > highID)
            {
                int tmpID = lowID;
                lowID = highID;
                highID = tmpID;
            }
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            return String.Format(ItemLink, quotes, lowID, highID, QL, style);
        }

        public static string CreateCommand(string name, string command) { return CreateCommand(name, command, false, "'"); }
        public static string CreateCommand(string name, string command, bool disableStyle) { return CreateCommand(name, command, disableStyle, "'"); }
        public static string CreateCommand(string name, string command, bool disableStyle, string quotes) { return CreateCommand(name, command, disableStyle, "'", false); }
        public static string CreateCommand(string name, string command, bool disableStyle, string quotes, bool rawMode)
        {
            return String.Format("{0}{1}{2}", CreateCommandStart(command, disableStyle, quotes, rawMode), EscapeString(name), CreateLinkEnd());
        }

        public static string CreateCommandStart(string command) { return CreateCommandStart(command, false, "'", false); }
        public static string CreateCommandStart(string command, bool disableStyle) { return CreateCommandStart(command, disableStyle, "'", false); }
        public static string CreateCommandStart(string command, bool disableStyle, string quotes) { return CreateCommandStart(command, disableStyle, quotes, false); }
        public static string CreateCommandStart(string command, bool disableStyle, string quotes, bool rawMode)
        {
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            if (rawMode)
               return String.Format(CommandLink, quotes, command, style);
            else
               return String.Format(CommandLink, quotes, HTML.EscapeString(command), style);
        }

        public static string CreateWindow(string link, string contents) { return CreateWindow(link, contents, false, "\""); }
        public static string CreateWindow(string link, string contents, bool disableStyle) { return CreateWindow(link, contents, disableStyle, "\""); }
        public static string CreateWindow(string link, string contents, bool disableStyle, string quotes)
        {
            return String.Format("{0}{1}{2}", CreateWindowStart(contents, disableStyle, quotes), link, CreateLinkEnd());
        }

        public static string CreateWindowStart(string contents) { return CreateWindowStart(contents, false, "\""); }
        public static string CreateWindowStart(string contents, bool disableStyle) { return CreateWindowStart(contents, disableStyle, "\""); }
        public static string CreateWindowStart(string contents, bool disableStyle, string quotes)
        {
            string style = "";
            if (disableStyle)
                style = String.Format(CleanLink, quotes);
            return String.Format(TextLink, quotes, contents, style);
        }

        public static string CreateLinkEnd()
        {
            return LinkEnd;
        }

        public static string CreateColorStart(string colorHex)
        {
            if (colorHex.Length == 7 && colorHex.ToCharArray()[0] == '#')
                colorHex = colorHex.Substring(1);

            if (colorHex.Length == 6)
                return string.Format(ColorStart, colorHex);
            else
                return "";
        }

        public static string CreateColorEnd()
        {
            return ColorEnd;
        }

        public static string CreateColorString(string colorHex, string text)
        {
            text = EscapeString(text);
            string color = CreateColorStart(colorHex);
            if (color != null && color != string.Empty)
                return color + text + CreateColorEnd();
            else
                return text;
        }

        public static string CreateIcon(Int32 iconID)
        {
            return string.Format(ImgIcon, iconID);
        }

        public static string CreateImage(string imageID)
        {
            return string.Format(ImgGui, imageID);
        }

        public static string CreateAlignStart(string alignment)
        {
            return string.Format(AlignStart, alignment);
        }

        public static string CreateAlignEnd()
        {
            return AlignEnd;
        }

        public static string CreateUnderlineStart() { return HTML.UnderlineStart; }
        public static string CreateUnderlineEnd() { return HTML.UnderlineEnd; }
        public static string CreateUnderlineString(string text)
        {
            return HTML.CreateUnderlineStart() + text + HTML.CreateUnderlineEnd();
        }

        public static string StripTags(string text)
        {
            text = text.Replace("\r", "");
            text = text.Replace("\t", " ");
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }

            char[] charText = text.ToCharArray();
            string strippedText = String.Empty;
            StripState state = StripState.Text;
            int readFrom = 0;

            for (int i = 0; i < charText.Length; i++)
            {
                switch (charText[i])
                {
                    case '>':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.Text;
                            readFrom = i + 1;
                        }
                        break;
                    case '"':
                        if (state == StripState.InsideTag)
                        {
                            state = StripState.InsideString;
                            break;
                        }
                        if (state == StripState.InsideString)
                        {
                            state = StripState.InsideTag;
                        }
                        break;
                    case '<':
                        if (state == StripState.Text || state == StripState.InsideTag)
                        {
                            strippedText += text.Substring(readFrom, i - readFrom);
                            readFrom = i;
                            state = StripState.InsideTag;
                            break;
                        }
                        break;
                    default:
                        break;
                }
            }
            strippedText += text.Substring(readFrom);
            return strippedText;
        }

        public static string EscapeString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("'", "&#039;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        public static string UnescapeString(string text)
        {
            text = text.Replace("&amp;", "&");
            text = text.Replace("&quot;", "\"");
            text = text.Replace("&#039;", "'");
            text = text.Replace("&lt;", "<");
            text = text.Replace("&gt;", ">");
            return text;
        }

        public static string GetHtml(string url) { return GetHtml(url, 30000); }
        public static string GetHtml(string url, Int32 timeout)
        {
            /*StreamReader stream = null;
            string result = null;
            try
            {
                DateTime timeStart = DateTime.Now;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (timeout != 0)
                {
                    request.Timeout = timeout;
                    request.ReadWriteTimeout = timeout;
                }
                request.UserAgent = "AoLib/" + AoLib.Net.Chat.VERSION + "(" + AoLib.Net.Chat.VERSIONSTRING + ")";
                request.KeepAlive = false;
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                stream = new StreamReader(response.GetResponseStream());
                result = stream.ReadToEnd();
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return result;*/

            // Parse URI
            if (!url.StartsWith("http://"))
                return null;
            if (url.Split('/').Length < 3)
                url = url + "/";
            if (url.Split('/').Length < 3)
                return null;
            if (url.Length < 9)
                return null;

            url = url.Substring(7);
            string domain = url.Substring(0, url.IndexOf('/'));
            string get = url.Substring(url.IndexOf('/'));
            string result = null;
            Int32 port = 80;
            if (domain.Contains(":"))
            {
                try
                {
                    port = Convert.ToInt32(domain.Substring(domain.IndexOf(':')).Trim(':'));
                    domain = domain.Substring(0, domain.IndexOf(':'));
                }
                catch { return null; }
            }
            if (!get.StartsWith("/"))
                get = "/" + get;
            domain = domain.Trim('/');

            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                // Connect
                client = new TcpClient();
                client.Connect(domain, port);

                // Build and Send Request
                string request = String.Format("GET {0} HTTP/1.0{2}Host: {1}{2}", get, domain, "\r\n");
                request += String.Format("Connection: Close{2}User-Agent: AoLib/{0} ({1}){2}{2}", AoLib.Net.Chat.BUILD, AoLib.Net.Chat.VERSION, "\r\n");

                client.ReceiveTimeout = timeout;
                client.SendTimeout = timeout;
                stream = client.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes(request);
                stream.Write(buffer, 0, buffer.Length);

                // Get Response
                StreamReader reader = new StreamReader(stream);
                string response = reader.ReadLine();
                string[] responseParts = response.Split(' ');
                if (responseParts.Length > 1)
                {
                    string responseVersion = responseParts[0];
                    string responseCode = responseParts[1];
                    string responseStatus = responseParts[2];

                    if (responseCode == "200")
                    {
                        // Find the Content
                        while (true)
                        {
                            string line = reader.ReadLine();
                            if (line == string.Empty || line == null)
                                break;
                        }
                        // Read the content
                        result = string.Empty;
                        for (int i = 0; i < 3; i++)
                        {
                            if (!client.Connected)
                                break;

                            string tmp = reader.ReadToEnd();
                            if (tmp != null && tmp != string.Empty)
                            {
                                result += tmp;
                                i = 0;
                            }
                            Thread.Sleep(50);
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
            return result;
        }
    }
}
