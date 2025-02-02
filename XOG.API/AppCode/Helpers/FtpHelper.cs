﻿using XOG.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace XOG.Helpers
{
    public static class FtpHelper
    {
        const string FTPProtocol = "ftp://";

        public static void Delete(string filePath, string fileName, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            try
            {
                var uri = $"{FTPProtocol}{ftpServerIP}/{Path.Combine(filePath, fileName)}";

                var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(uri));

                reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;

                var response = (FtpWebResponse)reqFtp.GetResponse();
                var datastream = response.GetResponseStream();
                using (var sr = new StreamReader(datastream))
                {
                    sr.Close();
                    datastream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }
        }

        public static void Download(string downloadFilePath, string ftpFilePath, string fileName, string newFileName, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            FtpWebRequest reqFtp;

            try
            {
                using (var outputStream = new FileStream(Path.Combine(downloadFilePath, newFileName), FileMode.Create))
                {
                    reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{Path.Combine(ftpFilePath, fileName)}"));
                    reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFtp.UseBinary = true;
                    reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                    var response = (FtpWebResponse)reqFtp.GetResponse();
                    var ftpStream = response.GetResponseStream();
                    const int bufferSize = 2048;
                    int readCount;
                    var buffer = new byte[bufferSize];

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }

                    ftpStream.Close();
                    outputStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }
        }

        public static void DownloadFtpDirectory(string url, NetworkCredential credentials, string localPath)
        {
            FtpWebRequest reqFtp = (FtpWebRequest)WebRequest.Create(url);
            reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            reqFtp.Credentials = credentials;

            List<string> lines = new List<string>();

            using (FtpWebResponse listResponse = (FtpWebResponse)reqFtp.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream))
            {
                while (!listReader.EndOfStream)
                {
                    lines.Add(listReader.ReadLine());
                }
            }

            foreach (string line in lines)
            {
                string[] tokens =
                    line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[8];
                string permissions = tokens[0];

                string localFilePath = Path.Combine(localPath, name);
                string fileUrl = url + name;

                if (permissions[0] == 'd')
                {
                    if (!Directory.Exists(localFilePath))
                    {
                        Directory.CreateDirectory(localFilePath);
                    }

                    DownloadFtpDirectory(fileUrl + "/", credentials, localFilePath);
                }
                else
                {
                    FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
                    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    downloadRequest.Credentials = credentials;

                    using (FtpWebResponse downloadResponse =
                              (FtpWebResponse)downloadRequest.GetResponse())
                    using (Stream sourceStream = downloadResponse.GetResponseStream())
                    using (Stream targetStream = File.Create(localFilePath))
                    {
                        byte[] buffer = new byte[10240];
                        int read;
                        while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            targetStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
        }

        public static string[] GetFileList(string path, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            string[] downloadFiles;
            var result = new StringBuilder();
            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{path}"));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectory;
                var response = reqFtp.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = reader.ReadLine();
                    }
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                    reader.Close();
                    response.Close();

                    return result.ToString().Split('\n');
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public static string[] GetFilesDetailList(string path, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            string[] downloadFiles;
            try
            {
                var result = new StringBuilder();
                var ftp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{path}"));
                ftp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var response = ftp.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = reader.ReadLine();
                    }

                    result.Remove(result.ToString().LastIndexOf("\n"), 1);
                    reader.Close();
                    response.Close();
                    return result.ToString().Split('\n');
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public static long GetFileSize(string filePath, string filename, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            FtpWebRequest reqFtp;
            long fileSize = 0;
            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{Path.Combine(filePath, filename)}"));
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                var response = (FtpWebResponse)reqFtp.GetResponse();
                var ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }

            return fileSize;
        }

        public static bool IfDirectoryExists(string path, string dirName, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            dirName += "/";
            var exists = false;

            try
            {
                var request = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{Path.Combine(path, dirName)}"));
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUserId, ftpPassword);

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    exists = true;
                }
            }
            catch (WebException ex)
            {
                ErrorLogger.LogError(ex);

                if (ex.Response != null)
                {
                    var response = (FtpWebResponse)ex.Response;

                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        exists = false;
                    }
                }
            }

            return exists;
        }

        public static bool InitializeRemotePath(string path, string loginName, string password, string host)
        {
            var initialized = true;

            var directories = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            var pathInsideWhichToCheck = string.Empty;

            for (var index = 0; index < directories.Length; index++)
            {
                if (!IfDirectoryExists(pathInsideWhichToCheck, directories[index], loginName, password, host))
                {
                    if (!MakeDir(pathInsideWhichToCheck, directories[index], loginName, password, host))
                    {
                        initialized = false;
                        break;
                    }
                }

                pathInsideWhichToCheck = GetPathTillIndex(index, directories);
            }

            return initialized;
        }

        public static bool MakeDir(string path, string dirName, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            var success = false;

            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{Path.Combine(path, dirName)}"));
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                var response = (FtpWebResponse)reqFtp.GetResponse();
                var ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                ErrorLogger.LogError(ex);
            }

            return success;
        }

        public static void Rename(string filePath, string currentFilename, string newFilename, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest)WebRequest.Create(new Uri($"{FTPProtocol}{ftpServerIP}/{Path.Combine(filePath, currentFilename)}"));
                reqFtp.Method = WebRequestMethods.Ftp.Rename;
                reqFtp.RenameTo = newFilename;
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);
                var response = (FtpWebResponse)reqFtp.GetResponse();
                var ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
            }
        }

        public static bool SendFileToRemoteServer(string path, string mappedFilePath, string loginName, string password, string host)
        {
            var success = false;

            if (InitializeRemotePath(path, loginName, password, host))
            {
                success = Upload(path, mappedFilePath, loginName, password, host);
            }

            return success;
        }

        public static bool Upload(string filePath, string filename, string ftpUserId, string ftpPassword, string ftpServerIP)
        {
            var success = false;

            var fileInf = new FileInfo(filename);

            var uri = $"{FTPProtocol}{ftpServerIP}/{Path.Combine(filePath, fileInf.Name)}";

            var reqFtp = (FtpWebRequest)WebRequest.Create(new Uri(uri));

            reqFtp.Credentials = new NetworkCredential(ftpUserId, ftpPassword);

            reqFtp.KeepAlive = false;

            reqFtp.Method = WebRequestMethods.Ftp.UploadFile;

            reqFtp.UseBinary = true;

            reqFtp.ContentLength = fileInf.Length;

            const int buffLength = 2048;
            var buff = new byte[buffLength];
            int contentLen;

            var fs = fileInf.OpenRead();

            try
            {
                var strm = reqFtp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();

                success = true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                success = false;
            }

            return success;
        }

        static string GetPathTillIndex(int tillIndex, string[] directories)
        {
            var stringTillIndex = string.Empty;

            for (var index = 0; index < tillIndex + 1; index++)
            {
                stringTillIndex = $"{stringTillIndex}{directories[index]}/";
            }

            return stringTillIndex;
        }
    }
}
