using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace newRtsp4
{
    public partial class Form1 : Form
    {
        private VideoCapture capture; // Video yakalama 
        private Mat frame; // OpenCV mat nesnesi
        private Bitmap image;
        private bool isStreaming;
        private bool isButtonStart;

        public Form1()
        {
            InitializeComponent(); // Form bileşenlerini başlatır
            capture = new VideoCapture(); // Video yakalama başlatır
            frame = new Mat(); // Mat nesnesini başlatır
            isButtonStart = true; // Başlangıçta buton "Start" modunda
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (isButtonStart) // Eğer buton "Start" modundaysa
            {
                string rtspUrl = "..."; // "rtsp://user:password@url/connectionSettings"

                isStreaming = true; // Akışı kontrol eder
                capture.Open(rtspUrl); // RTSP URL ile video yakalamayı başlatır

                if (!capture.IsOpened())
                {
                    MessageBox.Show("Video yok"); // Eğer video yakalama açılmadı ise hata mesajı gösterir
                    return;
                }

                button1.Text = "Stop"; // Buton metnini "Stop" olarak değiştir
                await Task.Run(() => StartStreaming()); // Akışı başlatan metodu çalıştır
            }
            else
            {
                StopStreaming(); // Akışı durdur
                button1.Text = "Start"; // Buton metnini "Start" olarak değiştir
            }

            isButtonStart = !isButtonStart; // Butonun durumunu tersine çevir
        }

        private void StartStreaming()
        {
            while (isStreaming) // Akış devam ettiği sürece
            {
                capture.Read(frame); // Videodan bir kare okur
                if (frame.Empty()) // Eğer kare boş ise
                    continue; // Döngünün başına döner

                image = BitmapConverter.ToBitmap(frame); // Okunan kareyi Bitmap nesnesine dönüştürür
                Invoke(new Action(() =>
                {
                    pictureBox1.Image?.Dispose(); // Mevcut görüntüyü temizler
                    pictureBox1.Image = image; // Yeni görüntüyü gösterir
                }));
                Thread.Sleep(30); // Gerekirse uyku süresini ayarlar
            }
        }

        private void StopStreaming()
        {
            isStreaming = false; // Akışı durdur
            capture.Release(); // Video yakalama bitiş
            Invoke(new Action(() =>
            {
                pictureBox1.Image?.Dispose(); // Mevcut görüntüyü temizle
                pictureBox1.Image = null; // Görüntüyü null yap
            }));
            this.Close(); // Formu kapat
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopStreaming(); // Form kapatılırken akışı durdur
        }
    }
}

//._     _
//.\\ Λ //
//.||(|)||
//.\\\|///
//..\\|//
//...\|/ 
//....|  /
//..\ | /
//...\|/
//....|
//....|
//....|
//....|
//....|
//....|
// from ali özdemir in 2024 https://www.linkedin.com/in/alioz/  https://github.com/Azfe38