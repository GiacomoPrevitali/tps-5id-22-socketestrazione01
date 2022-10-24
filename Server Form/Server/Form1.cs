using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Server
{
    public partial class Form1 : Form
    { 

        Thread t;
       
        IPAddress ipAddress;
        IPEndPoint localEndPoint;
        Socket listener;
        byte[] bytes = new Byte[1024];
        Random r;
        bool check = false;
        int num;
        public static string data = null;
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {            
            Inizializza();
        }
        public void Inizializza()
        {
            ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
            localEndPoint = new IPEndPoint(ipAddress, 5000);
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            r = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t = new Thread(server);
            t.Start();

        }
        public void server()
        {
            lbl_Stato.Text = "Acceso";
            if (lbl_Stato.Text == "Spento")
            {
                lbl_Stato.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lbl_Stato.ForeColor = System.Drawing.Color.Green;
            }
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;

                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    string[] info = data.Split(';');
                    var reader = new StreamReader(@"Utenti.csv");
                    while (!reader.EndOfStream)
                    {
                        var l = reader.ReadLine();
                        string[] v = l.Split(';');

                        if (v[0] == info[0] && v[1] == info[1])
                        {
                            check = true;
                        }
                    }
                    if (check)
                    {
                        num = r.Next(Convert.ToInt32(info[2]), Convert.ToInt32(info[3]));
                    }
                    else
                    {
                        num = -1;
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(Convert.ToString(num));

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    check = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
