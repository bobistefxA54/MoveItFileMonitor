﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MoveItFileMonitor
{
    public class FileMonitor
    {
        private readonly string _path;
        private readonly string _token;
        private readonly string _homeFolderID;
        private readonly FileUploader _uploader;
        private readonly ILogger _logger;

        public FileMonitor(string path, string token, string homeFolderID, FileUploader fileUploader, ILogger logger)
        {
            _path = path;
            _token = token;
            _homeFolderID = homeFolderID;
            _uploader = fileUploader;
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = logger;
        }

        public void Start()
        {

            FileSystemWatcher watcher = new()
            {
                Path = _path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.FileName,
                Filter = "*.*"
            };

            watcher.Created += OnChanged;
            watcher.EnableRaisingEvents = true;
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            await _uploader.UploadFileAsync(e.FullPath, _token, _homeFolderID);
            _logger.LogInformation("{FileName} uploaded to folder with ID {FolderID}", e.Name, _homeFolderID);
        }

    }
}
