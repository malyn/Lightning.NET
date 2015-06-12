﻿using System;
using System.IO;
using LightningDB.Native;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;
using Xunit;

namespace LightningDB.Tests
{
    public class SharedFileSystem : IDisposable
    {
        private readonly ILibraryManager _libraryManager;
        private readonly string _testProjectDir;
        private readonly string _testTempDir;

        public SharedFileSystem()
        {
            var locator = CallContextServiceLocator.Locator;
            var services = locator.ServiceProvider;
            _libraryManager = (ILibraryManager) services.GetService(typeof(ILibraryManager));
            _testProjectDir = Path.GetDirectoryName(_libraryManager.GetLibraryInformation("LightningDB.Tests").Path);
            var lightningDir = Path.GetDirectoryName(_libraryManager.GetLibraryInformation("LightningDB").Path);
            var libraryLoader = new DnxLibraryLoader();
            libraryLoader.Load(lightningDir);
            _testTempDir = Path.Combine(Directory.GetParent(_testProjectDir).Parent.FullName, "testrun");
            Console.WriteLine(_testTempDir);
        }

        public void Dispose()
        {
            Directory.Delete(_testTempDir, true);
        }

        public string CreateNewDirectoryForTest()
        {
            var path = Path.Combine(_testTempDir, "TestDb", Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            return path;
        }
    }

    [CollectionDefinition("SharedFileSystem")]
    public class SharedFileSystemCollection : ICollectionFixture<SharedFileSystem>
    {
    }
}