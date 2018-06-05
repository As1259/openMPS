#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using de.fearvel.openMPS.SQLiteConnectionTools;
using Image = System.Windows.Controls.Image;

//using Google.Authenticator;

namespace de.fearvel.openMPS.UC.Einstellungen
{
    /// <summary>
    ///     Interaktionslogik für EinstellungenTechniker.xaml
    /// </summary>
    public partial class EinstellungenTechniker : UserControl
    {
        //SetupCode setupInfo = new TwoFactorAuthenticator().GenerateSetupCode("MyApp", "user@example.com", "SuperSecretKeyGoesHere", 300, 300);

        public EinstellungenTechniker()
        {
            InitializeComponent();
            // TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();


            // WebClient wc = new WebClient();
            // MemoryStream ms = new MemoryStream(wc.DownloadData(setupInfo.QrCodeSetupImageUrl));
            // img_gauth.Source = ConvertImageToWpfImage(System.Drawing.Image.FromStream(ms)).Source;
            // string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
        }

        private void bt_SQLite_dec_Click(object sender, RoutedEventArgs e)
        {
            Collector.disableENC();
            Config.GetInstance().DisableEncryption();
        }

        public static Image ConvertImageToWpfImage(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image darf nicht null sein.");

            using (var dImg = new Bitmap(image))
            {
                using (var ms = new MemoryStream())
                {
                    dImg.Save(ms, ImageFormat.Bmp);

                    var bImg = new BitmapImage();

                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();

                    var img = new Image
                    {
                        Source = bImg
                    };

                    return img;
                }
            }
        }

        private void bt_SQLite_enc_Click(object sender, RoutedEventArgs e)
        {
            Collector.enableENC();
            Config.GetInstance().EnableEncryption();
        }
    }
}