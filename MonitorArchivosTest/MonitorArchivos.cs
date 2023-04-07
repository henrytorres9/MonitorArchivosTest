using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MonitorArchivosTest
{
    [RunInstaller(true)]

    public partial class MonitorArchivos : ServiceBase
    {
        private FileSystemWatcher fileSystemWatcher;

        public MonitorArchivos()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = "C:\\Test";
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(OnChanged);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(OnChanged);
            fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Registra los cambios en el archivo de log
            LogToFile($"{e.ChangeType} {e.FullPath}");
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Registra los cambios en el archivo de log
            LogToFile($"Archivo {e.OldFullPath} renombrado a: {e.FullPath}");
        }

        private void LogToFile(string message)
        {
            string logFilePath = "D:\\Documentos\\Videos\\Captures\\log.txt";
            using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
            {
                streamWriter.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} - {message}");
            }
        }

        protected override void OnStop()
        {
        }
    }
}
