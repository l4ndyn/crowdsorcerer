using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ChromeBrowserCookieReader
{
    public class Cookie
    {
        [Column("host_key")]
        public string HostKey { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("encrypted_value")]
        public byte[] EncryptedValue { get; set; }
    }

    public class ChromeCookieDbContext : DbContext
    {
        public DbSet<Cookie> Cookies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            var dbPath = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Network\Cookies");
            
            string newFile = Path.GetRandomFileName();
            string tempFileName = Path.Combine(localAppData, "Temp", newFile);
            File.Copy(dbPath, tempFileName);
            dbPath = tempFileName;

            if (!File.Exists(dbPath)) 
                throw new FileNotFoundException("Can't find cookie store", dbPath); // race condition, but i'll risk it

            var connectionString = "Data Source=" + dbPath + ";Mode=ReadOnly;";

            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cookie>().ToTable("cookies").HasNoKey();
        }
    }
}
