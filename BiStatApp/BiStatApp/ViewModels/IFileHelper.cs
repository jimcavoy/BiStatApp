using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiStatApp.ViewModels
{
    public interface IFileHelper
    {
        Task<bool> ExistsAsync(string filename);

        Task WriteTextAsync(string filename, string text);

        Task<string> ReadTextAsync(string filename);

        Task<IEnumerable<string>> GetFilesAsync();

        Task DeleteAsync(string filename);

        Task<bool> CopyFileAsync(string target, string source);

        string CreateFilepathInAppFolder(string filename);
    }
}
