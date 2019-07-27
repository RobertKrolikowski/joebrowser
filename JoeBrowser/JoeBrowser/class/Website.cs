using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;

namespace JoeBrowser
{
    class Website
    {
        string mainUrl;
        string currentUrl;
        string imgUri;
        string imgType;
        int currentNumber;
        HttpClientHandler handler;
        HttpClient client;
        string nameImg;

        public Website()
        {
            mainUrl = "https://joemonster.org";
            currentNumber = 1;
            nameImg = "/101112131415161718192021222324252627282930313233343536337383940414243444546474849";
            currentUrl = "https://joemonster.org/p/" + currentNumber + nameImg;
            handler = new HttpClientHandler();
            client = new HttpClient();
            
        }

        public async Task<BitmapImage> LoadContent()//System.Windows.Controls.Image image
        {
            HttpResponseMessage response;
            string allContent ="";
            try
            {
                response = await client.GetAsync(currentUrl);
                allContent = await response.Content.ReadAsStringAsync();
                var index = allContent.IndexOf("fileFile");
                imgUri = allContent.Substring(index, 300);
                index = imgUri.IndexOf("https");
                imgUri = imgUri.Substring(index);
                var endIndex = imgUri.IndexOf("\"");
                imgUri = imgUri.Substring(0, endIndex);
                imgType = imgUri.Substring(imgUri.Length - 6);
                imgType = imgType.Substring(imgType.IndexOf("."));                                     
            }
            catch (ArgumentOutOfRangeException)
            {
                var statementIndex = allContent.IndexOf("Uwaga");
                var statement = allContent.Substring(statementIndex, 300);
                if (statement.IndexOf("prywatny") >= 0)
                {
                    MessageBox.Show("Plik "+ currentNumber +" jest prywatny");                   
                }
                else if (statement.IndexOf("Niestety") >= 0)
                {
                    MessageBox.Show("Brak pliku "+ currentNumber);
                }
                else
                    MessageBox.Show("Błąd krytyczny");
            }
            return new BitmapImage(new Uri(imgUri));
        }

        public void SaveImage()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imgUri);
            Bitmap bitmap;
            bitmap = new Bitmap(stream);

            if (bitmap != null)
                bitmap.Save(currentNumber.ToString()+imgType);           
            stream.Flush();
            stream.Close();
            client.Dispose();
        }

        public void ChangeCurrentNumber(int i)
        {
            currentNumber += i;
            if (currentNumber <= 0)
                currentNumber = 1;
        }

        public void ChangeUrl()
        {
            currentUrl = mainUrl + "/p/" + currentNumber + nameImg; 
        }

        public string GetImgLink()
        {
            return imgUri;
        }

        public Bitmap ImageShow()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imgUri);
            Bitmap bitmap;
            bitmap = new Bitmap(stream);
            return bitmap;
        }

        public async void LastNumber()
        {
            HttpResponseMessage response;
            string allContent = "";
            try
            {
                response = await client.GetAsync(mainUrl);
                allContent = await response.Content.ReadAsStringAsync();
                var index = allContent.IndexOf("/p/");
                string buff = allContent.Substring(index+3, 300);
                index = buff.IndexOf("/");
                currentNumber = int.Parse(buff.Substring(0, index));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: "+ ex.ToString());
            }
        }
    }
}
