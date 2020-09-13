using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdaterCSharp
{
    // Version: 3 Jan 2013. 
    
    /// <summary>
    /// Updater can be added to your C# program. Call CheckForUpdates with the URL where an XML file
    /// must be located that has update information. It fails silently if not found.
    /// Otherwise it downloads the MSI, starts it with /passive, and then closes the hosting
    /// Windows app.
    /// </summary>
    class Updater
    {

        /// <summary>
        /// Check for an update at urlToCheck and download and run it.
        /// </summary>
        /// <param name="urlToCheck"></param>
        public static void CheckForUpdates(string urlToCheck)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            try
            {
                xmlDoc.Load(urlToCheck);
            }
            catch
            {
                // Not online or problem with XML. Fail silently.
                return;
            }
            string version = xmlDoc.DocumentElement.SelectSingleNode("Version").InnerText;
            if (version != System.Windows.Forms.Application.ProductVersion)
            {
                // Need to update.
                System.Net.WebClient wc = new System.Net.WebClient();
                string filename = xmlDoc.DocumentElement.SelectSingleNode("Filename").InnerText;
                string path = System.IO.Path.GetTempPath() + filename;
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                        System.Windows.Forms.Application.DoEvents();
                    }
                    catch
                    {
                        // Probably already in use. Fail silently.
                        return;
                    }
                }
                try
                {
                    // Download the new installer.
                    wc.DownloadFileAsync(new System.Uri(xmlDoc.DocumentElement.SelectSingleNode("URL").InnerText), path);
                    while (wc.IsBusy)
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }

                    // Create an installer batch file.
                    // Get the path
                    string batchPath = System.IO.Path.GetTempPath() + filename + ".bat";
                    if (System.IO.File.Exists(batchPath))
                    {
                        System.IO.File.Delete(batchPath);
                        System.Windows.Forms.Application.DoEvents();
                    }
                    // Write the batch file.
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(batchPath);
                    sw.WriteLine("@title Updating " + System.Windows.Forms.Application.ProductName);
                    sw.WriteLine("@echo Updating " + System.Windows.Forms.Application.ProductName);
                    sw.WriteLine("@cd \"" + System.IO.Path.GetTempPath() + "\"");
                    // Put in a pause to allow this application to close.
                    sw.WriteLine("@choice /D:Y /T:2 /N");
                    sw.WriteLine("@msiexec /I \"" + filename + "\" /passive");
                    //sw.WriteLine("pause");
                    sw.Close();
    
                    // Now execute the batch file.
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    // Turns out we probably want to show the window so the user can get an
                    // idea something is happening.
                    //proc.StartInfo.CreateNoWindow = true;
                    //proc.StartInfo.RedirectStandardOutput = true;
                    //proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.FileName = batchPath;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();

                    // And close this application!
                    System.Windows.Forms.Application.Exit();
                    return;
                }
                catch
                {
                    // Failed to download for some reason, fail silently.
                    return;
                }
            }
        }
    }
}
