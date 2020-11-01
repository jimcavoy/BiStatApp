using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Util;

using Xamarin.Forms;

[assembly: Dependency(typeof(BiStatApp.Droid.FileHelperImpl))]

namespace BiStatApp.Droid
{
    class FileHelperImpl : BiStatApp.ViewModels.IFileHelper
    {
        public Task<bool> ExistsAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            bool exists = File.Exists(filepath);
            return Task<bool>.FromResult(exists);
        }

        public async Task WriteTextAsync(string filename, string text)
        {
            string filepath = GetFilePath(filename);
            using (StreamWriter writer = File.CreateText(filepath))
            {
                await writer.WriteAsync(text);
            }
        }

        public async Task<string> ReadTextAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public Task<IEnumerable<string>> GetFilesAsync()
        {
            IEnumerable<string> filenames =
                from filepath in Directory.EnumerateFiles(GetDocsFolder())
                select Path.GetFileName(filepath);

            return Task<IEnumerable<string>>.FromResult(filenames);
        }

        public Task DeleteAsync(string filename)
        {
            File.Delete(GetFilePath(filename));
            return Task.FromResult(true);
        }

        public async Task<bool> CopyFileAsync(string target, string source)
        {
            try
            {
                string resolvedSource = source;
                if (source.Contains("content://"))
                {
                    Context context = Android.App.Application.Context;
                    Android.Net.Uri uri = Android.Net.Uri.Parse(source);
                    resolvedSource = FilesHelper.GetActualPathForFile(uri, context);
                }

                string localFolder = Android.App.Application.Context.FilesDir.Path;
                if (target != "")
                {
                    localFolder = Path.Combine(localFolder, target);
                    if (!Directory.Exists(localFolder))
                    {
                        var createTask = await Task.Run(() => Directory.CreateDirectory(localFolder));
                    }
                }

                string targetFilePath = Path.Combine(localFolder, Path.GetFileName(resolvedSource));
                File.Copy(resolvedSource, targetFilePath, true);

                return true;
            }
            catch (Exception ex)
            {
                string tag = "BiStatApp.Droid.FileHelperImpl.CopyFileAsync";
                Log.Warn(tag, ex.Message);
                if (ex.InnerException != null)
                    Log.Warn(tag, ex.InnerException.Message);
                return false;
            }
        }

        public string CreateFilepathInAppFolder(string filepath)
        {
            string resolvedPath = ResolveFilePath(filepath);
            string filename = Path.GetFileName(resolvedPath);
            return Path.Combine(Android.App.Application.Context.FilesDir.Path, filename);
        }

        string GetDocsFolder()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }

        string GetFilePath(string filename)
        {
            return Path.Combine(GetDocsFolder(), filename);
        }

        public string ResolveFilePath(string source)
        {
            if (source.Contains("content://"))
            {
                Context context = Android.App.Application.Context;
                Android.Net.Uri uri = Android.Net.Uri.Parse(source);
                return FilesHelper.GetActualPathForFile(uri, context);
            }
            else
            {
                return source;
            }
        }
    }
}