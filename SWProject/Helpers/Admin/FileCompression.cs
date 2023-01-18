using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace SWProject.Helpers.Admin
{
    public class FileCompression
    {
        //microsoft
        public static void Compress(FileInfo fi)
        {
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and already compressed files. 
                if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden)
                        != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file. 
                    using (FileStream outFile = File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream Compress = new GZipStream(outFile,
                                CompressionMode.Compress))
                        {
                            // Copy the source file into the compression stream.
                            byte[] buffer = new byte[4096];
                            int numRead;
                            while ((numRead = inFile.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                Compress.Write(buffer, 0, numRead);
                            }
                        }
                    }
                }
            }
        }

        public static void Decompress(FileInfo fi)
        {
            // Get the stream of the source file. 
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                //Create the decompressed file. 
                using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                            CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outFile.Write(buffer, 0, numRead);
                        }
                    }
                }
            }
        }

        //git
        //public void compression(int pageId, int userProjectId, int formId, string documentType, HttpPostedFileBase file)
        ////This method runs if the browser that sends the request can upload the file asynchronously
        //{
        //        if (file.ContentLength > 0)
        //        {
        //            //Compress

        //            var data = new byte[file.ContentLength];

        //            file.InputStream.Read(data, 0, file.ContentLength);

        //            var outputMemoryStream = new MemoryStream();

        //            var compressedStream = new GZipStream(outputMemoryStream, CompressionMode.Compress);
        //            compressedStream.Write(data, 0, data.Length);
        //            compressedStream.Close();

        //            var compressedByteArray = outputMemoryStream.ToArray();

        //            var document = new DocumentModel
        //            {
        //                ContentType = documentType,
        //                DocumentLength = file.ContentLength,
        //                DocumentType = documentType,
        //                FileName = file.FileName,
        //                FileContents = GetString(compressedByteArray)
        //            };


        //            //Decompress

        //            var dbMemoryStream = new MemoryStream(compressedByteArray);
        //            var unCompressedStream = new GZipStream(dbMemoryStream, CompressionMode.Decompress);

        //            var resultStream = new MemoryStream(file.ContentLength);

        //            unCompressedStream.CopyTo(resultStream);

        //            var unCompressedByteArray = resultStream.ToArray();

        //            string filePath = Path.Combine(@"C:\Dev\ERM\ERM\TRUNK\ERM\Forms.Web\App_Data", Path.GetFileName(file.FileName));

        //            var file2 = System.IO.File.Create(filePath);

        //            file2.Write(unCompressedByteArray, 0, unCompressedByteArray.Length);

        //            file2.Flush();
        //            file2.Close();
        //            file2.Dispose();
        //        }
            
        //}
    }
}