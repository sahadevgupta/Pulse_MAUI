namespace Pulse_MAUI.Helpers
{
    /// <summary>
    /// Class with all File Handling Utility methods
    /// Fogbugz Case:
    /// Author: Neil Backhurst
    /// Created: 30/03/2013
    /// </summary>
    public static class FileUtility
    {
        public static string GetFileName(string filePath)
        {
            string filename = "";
            string[] parts = filePath.Split('/');
            if (parts.Length > 0)
            {
                filename = parts[parts.Length - 1];
            }
            else
            {
                filename = filePath;
            }

            return filename;
        }

        /// <summary>
        /// Pathes the only.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string PathOnly(string filePath)
        {
            IList<string> Parts = GetFoldersFromPath(filePath);
            return string.Join("/",Parts);
        }

        /// <summary>
        /// Gets the folders from path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static IList<string> GetFoldersFromPath(string filePath)
        {
            IList<string> folders = new List<string>();
            foreach(string element in filePath.Split('/'))
            {
                if (!element.Contains("."))
                {
                    folders.Add(element);
                }
            }

            return folders;
        }


        /// <summary>
        /// Gets the punch storage path.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="pcaId">The pca identifier.</param>
        /// <returns></returns>
        public static string GetPunchStoragePath(int projectId, int pcaId)
        {
            string basePath = FileSystem.Current.AppDataDirectory;

            string path = Path.Combine(
                basePath,
                Helpers.Keys.BlobStorage,
                "Storage_Projects",
                $"Project_{projectId}",
                "Punch",
                pcaId.ToString()
            );

            return path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Gets the activity storage path.
        /// </summary>
        /// <param name="ProjectId">The project identifier.</param>
        /// <param name="PCAId">The pca identifier.</param>
        /// <returns></returns>
        public static string GetActivityStoragePath(int pcaId)
        {
            string basePath = FileSystem.Current.AppDataDirectory;

            string path = Path.Combine(
                basePath,
                Helpers.Keys.BlobStorage
            );

            return path + Path.DirectorySeparatorChar;
        }
    }
}
