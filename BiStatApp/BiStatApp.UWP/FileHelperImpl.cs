using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

using Xamarin.Forms;

[assembly: Dependency(typeof(BiStatApp.UWP.FileHelperImpl))]
namespace BiStatApp.UWP
{
    class FileHelperImpl : BiStatApp.ViewModels.IFileHelper
    {
        public async Task<bool> ExistsAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            try
            {
                await localFolder.GetFileAsync(filename);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task WriteTextAsync(string filename, string text)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IStorageFile storageFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(storageFile, text);
        }

        public async Task<string> ReadTextAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            IStorageFile storageFile = await localFolder.GetFileAsync(filename);
            return await FileIO.ReadTextAsync(storageFile);
        }

        public async Task<IEnumerable<string>> GetFilesAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            IEnumerable<string> filenames = from storageFile in await localFolder.GetFilesAsync() select storageFile.Name;

            return filenames;
        }

        public async Task DeleteAsync(string filename)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await localFolder.GetFileAsync(filename);
            await storageFile.DeleteAsync();
        }

        /// <summary>
        /// copies a file from outside local folder, into the local folder (or sub folder)
        /// target is a relative path from apps local folder, can be empty to refer to top level
        /// source is a full path to file to be copied
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public async Task<bool> CopyFileAsync(string target, string source)
        {
            try
            {
                Task<StorageFile> task = Task.Run<StorageFile>(async () => await StorageFile.GetFileFromPathAsync(source));
                StorageFile origFile = task.Result;
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                if (target != "")
                {
                    localFolder = await localFolder.GetFolderAsync(target);
                }
                var copyTask = Task.Run(async () => await origFile.CopyAsync(localFolder, Path.GetFileName(source), NameCollisionOption.ReplaceExisting));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException.Message);
                Debug.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public string CreateFilepathInAppFolder(string filepath)
        {
            string filename = Path.GetFileName(filepath);
            var path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string fullPath = Path.Combine(path, filename);
            return fullPath;
        }
    }
}
