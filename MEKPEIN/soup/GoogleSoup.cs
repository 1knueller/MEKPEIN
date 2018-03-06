using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;

namespace MEKPEIN.soup
{
    public class GoogleSoup
    {
        public async System.Threading.Tasks.Task Ting()
        {
            string html = GetHtmlCodeForSearchString("kandinsky");
            List<string> urls = GetUrls(html).Where(m => m.EndsWith(".jpg") || m.EndsWith(".png")).ToList();

            Directory.CreateDirectory(Pathmaster.Images);

            using (var httpClient = new HttpClient())
            {
                foreach (var url in urls)
                {
                    var fname = Path.Combine(Pathmaster.Images , Guid.NewGuid().ToString() + new string(url.TakeLast(4).ToArray()));
                    using (var contentStream = await httpClient.GetStreamAsync(new Uri(url)))
                    using (var fileStream = new FileStream(fname, FileMode.Create, FileAccess.Write, FileShare.None, 1048576, true))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }

            //byte[] image = GetImage(luckyUrl);
            //using (var ms = new MemoryStream(image))
            //{

            //    //pictureBox1.Image = Image.FromStream(ms);
            //}
        }

        private string GetHtmlCodeForSearchString(string searchstring)
        {
            var rnd = new Random();

            string url = "https://www.google.com/search?q=" + searchstring + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }
        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }
        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }

            return null;
        }
    }
}
