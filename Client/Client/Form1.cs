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

namespace Client
{
    public partial class Form1 : Form
    {
        byte[] bytes = new byte[1024];
        IPAddress ipAddress;
        IPEndPoint remoteEP;
        Socket Sender;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txt_Password.PasswordChar = '*';
   
        }

        private void btn_Invia_Click(object S, EventArgs e)
        {
            string r;
            try
            {

                ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                remoteEP = new IPEndPoint(ipAddress, 5000);
                Sender = new Socket(ipAddress.AddressFamily,
                  SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    Sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        Sender.RemoteEndPoint.ToString());


                    byte[] msg = Encoding.ASCII.GetBytes(txt_Nome.Text + ";" + txt_Password.Text + ";" + txt_Min.Text + ";" + txt_Max.Text + ";<EOF>");

                    int bytesSent = Sender.Send(msg);


                    int bytesRec = Sender.Receive(bytes);
                    r = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (Convert.ToInt32(r) == -1)
                    {
                        MessageBox.Show("Nome Utente o Password errati!", "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    else
                    {
                        lbl_Num.Text = "Numero Estratto: " + r;
                    }

                    Sender.Shutdown(SocketShutdown.Both);
                    Sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show(ane.ToString());
                }
                catch (SocketException se)
                {
                    MessageBox.Show("IMPOSSIBILE RAGGIUNGERE IL SERVER");
                }
                catch (Exception exx)
                {
                    MessageBox.Show(exx.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
