using System;
using Microsoft.Expression.Encoder;
using Microsoft.Expression.Encoder.Profiles;
using System.Drawing;

namespace wmvocplus
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaItem mediaItem;
            if (args.Length == 0) {
                System.Console.WriteLine("Usage: wmvocplus.exe <wmvfilename> <jpg filler to append> <seconds to display filler>");
                return;
            }
            string wmvfilename = args[0];
            string jpgfillername = args[1];
            string outputfilename = wmvfilename.Substring(0, wmvfilename.Length -3 );
            outputfilename += "oc.wmv";
            string captionfilename = wmvfilename.Substring(0, wmvfilename.Length - 3);
            captionfilename += ".smi";
            int timeoptions = Convert.ToInt32(args[2]);

            System.Console.WriteLine("wmvfilename:\t" + wmvfilename);
            System.Console.WriteLine("jpgfillername:\t" + jpgfillername);
            System.Console.WriteLine("timeoptions:\t" + timeoptions);
            System.Console.WriteLine("output:\t" + outputfilename);

            try
            {
            // sets file name to media item
            mediaItem = new MediaItem(wmvfilename);
            
            // challenge... these next three lines fix aspect ratio screen clipping that occurs when 
            // source is also wmv.  ...unfortunately it also causes the captions not to burn into the video...
      //      mediaItem.OutputFormat.VideoProfile = new AdvancedVC1VideoProfile();
      //      mediaItem.OutputFormat.VideoProfile = mediaItem.SourceVideoProfile;
           // mediaItem.OutputFormat.AudioProfile = mediaItem.SourceAudioProfile;

            /*
              mediaItem.OutputFormat.VideoProfile.AspectRatio = mediaItem.SourceVideoProfile.AspectRatio;
              mediaItem.OutputFormat.VideoProfile = new AdvancedVC1VideoProfile();
              mediaItem.OutputFormat.VideoProfile.Bitrate = new ConstantBitrate(200); // VariableConstrainedBitrate(200, 1500);
              mediaItem.OutputFormat.VideoProfile.Streams[0].Size = new Size(320, 240);
              mediaItem.OutputFormat.VideoProfile.Size = new Size(320, 240); 
              mediaItem.OutputFormat.VideoProfile.FrameRate = 30;
              mediaItem.OutputFormat.VideoProfile.KeyFrameDistance = new TimeSpan(0, 0, 4); */
              
              // audio on one didn't work out so this helped but blows up on others...
            
      //        mediaItem.OutputFormat.AudioProfile = new WmaAudioProfile();
      //        mediaItem.OutputFormat.AudioProfile.Bitrate = new ConstantBitrate(128); // VariableConstrainedBitrate(128, 192);
      //        mediaItem.OutputFormat.AudioProfile.Codec = AudioCodec.WmaProfessional;
      //        mediaItem.OutputFormat.AudioProfile.BitsPerSample = 24;

              // captions
              //mediaItem.Sources[0].CaptionFiles.Add(new CaptionFile(captionfilename));

            // add trailer
            mediaItem.Sources.Add(new Source(@""+jpgfillername+""));
            mediaItem.Sources[1].Clips[0].EndTime = mediaItem.Sources[1].MediaFile.Duration + TimeSpan.FromSeconds(timeoptions);

            }
            catch (InvalidMediaFileException exp)
            {
                // Media file was invalid and it returns an error msg
                Console.WriteLine(exp.ToString());
                return;
            }

            // verifies encoding of file
            Console.WriteLine("\nEncoding: {0}", wmvfilename );

            // Create a job and the media item for the video we wish
            // to encode.
            Job job = new Job();
            job.MediaItems.Add(mediaItem);

            // Set up the progress callback function
            job.EncodeProgress
                += new EventHandler<EncodeProgressEventArgs>(OnProgress);

            // Set the output directory and encode.
            job.OutputDirectory = Environment.CurrentDirectory;
            job.DefaultMediaOutputFileName = outputfilename;
            job.CreateSubfolder = false;

            // encodes job
            job.Encode();
            job.Dispose();
        }

        /// <summary>
        /// This method will create a bitmap based 
        /// </summary>
        /// <param name="overlayText"></param>
        /// <param name="rootPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static string createOverlayImage(string overlayText, string rootPath, int width, int height)
        {
            // full path for a temporary bitmap
            string overlayFileName = rootPath + "\\" + Guid.NewGuid().ToString() + ".bmp";

            // create a bitmap
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            // define the font
            Font font = new Font("Arial", (float)14.0);

            // define the area to draw on
            Rectangle area = new Rectangle(new Point(0, 0), new Size(width, height));

            // draw the new image
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString(overlayText, font, Brushes.White, area);

            // save the picture with the text overlay
            bitmap.Save(overlayFileName);

            // return the path to the overlay image
            return overlayFileName;
        }

        private static string createOverlayText(string overlayText)
        {
            int width=320; int height=40;
            // full path for a temporary bitmap
            string overlayFileName = Environment.CurrentDirectory + "\\" + Guid.NewGuid().ToString() + ".bmp";

            // create a bitmap
            Bitmap bitmap = new Bitmap(width, height); // width, height
            Graphics g = Graphics.FromImage(bitmap);

            // define the font
            Font font = new Font("Calibri", (float)14.0);

            // define the area to draw on
            Rectangle area = new Rectangle(new Point(0, 0), new Size(width, height));

            // draw the new image
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString(overlayText, font, Brushes.White, area);

            // save the picture with the text overlay
            bitmap.Save(overlayFileName);

            // return the path to the overlay image
            return overlayFileName;
        }


        static void OnProgress(object sender, EncodeProgressEventArgs e)
        {
            Console.Write("\b\b\b\b\b\b\b\b");
            Console.Write("{0:F2}%", e.Progress);
        }
    }
}
