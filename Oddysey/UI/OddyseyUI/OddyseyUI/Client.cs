﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace OddyseyUI
{
    class Client
    {
        private Socket clientSocket;
        private List<AudioFile> SongList;
        MD5 md5;

        public Client ()
        {
            SongList = new List<AudioFile>();
            md5 = new MD5();
            UpdateSongs();
        }

        public String SendMessage(string message, string OPCode)
        {
            // Main parameters
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverAddress);

            // Sending OPCode
            byte[] toSendCode = System.Text.Encoding.UTF8.GetBytes(OPCode);
            clientSocket.Send(toSendCode);
            clientSocket.Close();

            // Re-open (literally the one and only way I found to make this work)
            /*
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverAddress);

            // Receiving
            byte[] replyBytes = new byte[1000];
            int bytesRead;
            string reply = "";
            while ((bytesRead = clientSocket.Receive(replyBytes)) > 0)
            {
                reply = reply + System.Text.Encoding.UTF8.GetString(replyBytes);
            }

            // Sending data based on OPCode
            byte[] toSendBytes = System.Text.Encoding.UTF8.GetBytes(message);
            clientSocket.Send(toSendBytes);

            clientSocket.Close();
            */

            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            Int32 port = 6000;
            TcpClient client = new TcpClient("localhost", port);

            // String to store the response UTF8 representation.
            String reply = String.Empty;

            using (NetworkStream stream = client.GetStream())
            {
                var data = new Byte[256];
                using (MemoryStream ms = new MemoryStream())
                {

                    int numBytesRead;
                    while ((numBytesRead = stream.Read(data, 0, data.Length)) > 0)
                    {
                        ms.Write(data, 0, numBytesRead);
                        if (numBytesRead < 256)
                        {
                            break;
                        }
                    }
                    reply = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);

                    byte[] bytesToSend = Encoding.UTF8.GetBytes(message);
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                }
            }            

            return reply;
            
        }

        // Testing
        public void CreateTestFile(string message)
        {
            string path = @"Sending.txt";
            File.WriteAllText(path, message);
        }

        public void AddSong(string fileName)
        {
            // Recordar verificar si la canción ya existe
            XmlMessage m1 = new XmlMessage();
            AudioFile audio = new AudioFile();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            audio.SetMainParameters(f2.name, f2.author);
            audio.Data = Convert.ToBase64String(File.ReadAllBytes(fileName));
            string toSend = m1.GetAddSongXML(audio);
            SendMessage(toSend, "001/null");
            UpdateSongs();

            // Testing
            CreateTestFile(toSend);

        }
        
        public void DeleteSong(string name, string author)
        {
            XmlMessage xml = new XmlMessage();
            string toSend = xml.GetDeleteSongXML(name, author);
            SendMessage(toSend, "004/null");
            UpdateSongs();
        }

        public void UpdateSongs()
        {
            SongList = new List<AudioFile>();
            string songMetadataXml = SendMessage("", "002/null");

            // Testing
            string path = @"Received.xml";
            File.WriteAllText(path, songMetadataXml);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(songMetadataXml);
            XmlNodeList audioFiles = doc.SelectNodes("XmlMessage/OperationData/AudioFiles/AudioFile");
            XmlSerializer serializer = new XmlSerializer(typeof(AudioFile));

            foreach (XmlNode node in audioFiles)
            {
                SongList.Add((AudioFile)serializer.Deserialize(new XmlNodeReader(node)));
            }
        }
        public void SortSongsTitle()
        {
            SongList = new List<AudioFile>();
            string songMetadataXml = SendMessage("", "008/null");

            // Testing
            string path = @"Received.xml";
            File.WriteAllText(path, songMetadataXml);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(songMetadataXml);
            XmlNodeList audioFiles = doc.SelectNodes("XmlMessage/OperationData/AudioFiles/AudioFile");
            XmlSerializer serializer = new XmlSerializer(typeof(AudioFile));

            foreach (XmlNode node in audioFiles)
            {
                SongList.Add((AudioFile)serializer.Deserialize(new XmlNodeReader(node)));
            }
        }
        public void SortSongsAuthor()
        {
            SongList = new List<AudioFile>();
            string songMetadataXml = SendMessage("", "009/null");

            // Testing
            string path = @"Received.xml";
            File.WriteAllText(path, songMetadataXml);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(songMetadataXml);
            XmlNodeList audioFiles = doc.SelectNodes("XmlMessage/OperationData/AudioFiles/AudioFile");
            XmlSerializer serializer = new XmlSerializer(typeof(AudioFile));

            foreach (XmlNode node in audioFiles)
            {
                SongList.Add((AudioFile)serializer.Deserialize(new XmlNodeReader(node)));
            }
        }
        public void SortSongsAlbum()
        {
            SongList = new List<AudioFile>();
            string songMetadataXml = SendMessage("", "010/null");

            // Testing
            string path = @"Received.xml";
            File.WriteAllText(path, songMetadataXml);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(songMetadataXml);
            XmlNodeList audioFiles = doc.SelectNodes("XmlMessage/OperationData/AudioFiles/AudioFile");
            XmlSerializer serializer = new XmlSerializer(typeof(AudioFile));

            foreach (XmlNode node in audioFiles)
            {
                SongList.Add((AudioFile)serializer.Deserialize(new XmlNodeReader(node)));
            }
        }

        public void DownloadSong(string name, string author)
        {
            string songData = SendMessage("", "003" + "/" + name + "-!%!-" + author);
            string path = @"Temp\" + name + "-" + author + ".mp3";
            File.WriteAllBytes(path, Convert.FromBase64String(songData));
        }

       public Boolean GetMetadataOnline(AudioFile audio)
        {
            string updatedSongXml = SendMessage("", "005" + "/" + audio.Name + "-!%!-" + audio.Author);

            XmlSerializer serializer = new XmlSerializer(typeof(XmlMessage));
            String dataBackup = audio.Data;
            using (var reader = new StringReader(updatedSongXml))
            {
                XmlMessage message = (XmlMessage)serializer.Deserialize(reader);
                // audio.Data = dataBackup; // creo que es innecesario
                audio = message.OperationData.SongToUpdate;
            }

            if (audio.Album is null)
            {
                return false;
            }
            else
            {
                UpdateSongs();
                return true;
            }

        }
        public void AddUser(Form3 form)
        {
            XmlMessage m1 = new XmlMessage();
            User user = new User();
            Form3 f3 = form;
            // string hashedPass = md5.CreateMD5(f3.password);
            user.SetMainParameter(f3.name, f3.lastname, f3.nickname, md5.CreateMD5(f3.password), f3.age);
            string toSend = m1.GetAddUserXML(user);
            SendMessage(toSend, "006/null");

        }
        public void CheckUser(Form3 form)
        {
            XmlMessage m1 = new XmlMessage();
            User user = new User();
            Form3 f3 = form;
            user.SetLogParameter(f3.nickname, md5.CreateMD5(f3.password));
            string toSend = m1.GetCheckUserXML(user);
            SendMessage(toSend, "007/null");
        }

        public List<AudioFile> GetSongList()
        {
            return SongList;
        }

        public AudioFile GetAudio(string name, string author)
        {
            for (int i = 0; i < SongList.Count; i++)
            {
                var song = SongList[i];
                if(song.Name == name && song.Author == author)
                {
                    return song;
                }
            }
            return null;
        }

        public AudioFile findSongTitle(String title)
        {
            for (int i = 0; i < SongList.Count; i++)
            {
                if (SongList[i].Name == title)
                {
                    return SongList[i];
                }
            }
            return null;
        }

        public AudioFile findSongAuthor(String author)
        {
            for (int i = 0; i < SongList.Count; i++)
            {
                if (SongList[i].Author == author)
                {
                    return SongList[i];
                }
            }
            return null;
        }

        public AudioFile findSongAlbum(String album)
        {
            for (int i = 0; i < SongList.Count; i++)
            {
                if (SongList[i].Album == album)
                {
                    return SongList[i];
                }
            }
            return null;
        }

    }
}
