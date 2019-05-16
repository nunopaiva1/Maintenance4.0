using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;
//using System.Drawing;
using System.Data.SqlClient;
using System.IO.Compression;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using System.Drawing.Imaging;
//using System.Runtime.Serialization.Formatters.Binary;

public class GetImageDB : MonoBehaviour {

    //public Button button;
    public Button[] but;
    //public Sprite sp;
    //public Image imagee;
    void Start () {
        int i;
        
        //byte[] image = GetImage("Mário Fernandes");
        //for (i = 0; i < but.Length; i++)
        //{
            byte[] image = GetImage();
            
            //MemoryStream stream = new MemoryStream(image);
            //File.WriteAllBytes(Application.dataPath + "/../MarioFernandes.png", image);
            //string enc = System.Convert.ToBase64String(image);
            //byte[] bytes2 = System.Convert.FromBase64String(enc);
            //Texture2D texture = new Texture2D(100, 100, TextureFormat.Alpha8, false);
            Texture2D texture = new Texture2D(8,8, TextureFormat.PVRTC_RGBA4, false);
            //byte[] bytes2 = texture.EncodeToJPG();
            //File.WriteAllBytes(Application.dataPath + "/../MarioFernandes.jpg", bytes2);
            //texture.LoadImage(image);
            texture.LoadRawTextureData(image);
            texture.Apply();

            Sprite foto = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
              //but[i].GetComponent<Image>().sprite = foto;

            //but[i].GetComponent<Renderer>().material.mainTexture = texture;
        //}
    }



    //byte[] GetImage(string Nome)
    byte[] GetImage()
    {
        string ConnectionString = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
        using (var conn = new MySqlConnection(ConnectionString))
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT foto FROM catraport.utilizador";
           // cmd.CommandText = "SELECT foto FROM catraport.utilizador WHERE nome = ?";
            //cmd.Parameters.AddWithValue("?", Nome);
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }

                const int CHUNK_SIZE = 2*1024;
                byte[] buffer = new byte[CHUNK_SIZE];
                long bytesRead;
                long fieldOffset = 0;
                using (var stream = new MemoryStream())
                {
                    while ((bytesRead = reader.GetBytes(reader.GetOrdinal("foto"), fieldOffset, buffer, 0, buffer.Length)) == buffer.Length)
                    {
                        stream.Write(buffer, 0, (int)bytesRead);
                        fieldOffset += bytesRead;                       
                    }
                    
                    return stream.ToArray();
                    
                }
            }
        }
    }
}
