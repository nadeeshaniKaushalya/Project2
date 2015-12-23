using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TANK
{
    public partial class Form1 : Form
    {

        private bool isConnected;
        private Contestant contestant = new Contestant();
        private LifePack lifepack = new LifePack();
        private CoinPile coinpile = new CoinPile();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
  


        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected == false)
            {
                Thread aThread = new Thread(new ThreadStart(joinserver));
                aThread.Start();

                Thread bThread = new Thread(new ThreadStart(waitForConnection));
                bThread.Start();

                isConnected = true;
                
            }
            else
            {
                MessageBox.Show("You are already connected to the server");
            }
        }

        public void setText(string s)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connect("DOWN#");
            
        }

        
        
        private void button4_Click(object sender, EventArgs e)
        {
            connect("UP#");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            connect("LEFT#");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            connect("RIGHT#");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            connect("SHOOT#");
        }

        public static void joinserver()
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();      //create a TcpCLient socket to connect to server
            NetworkStream stream = null;

            //connecting to server socket with port 6000
            clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            stream = clientSocket.GetStream();

            //joining message to server
            byte[] ba = Encoding.ASCII.GetBytes("JOIN#");
            /*
            for (int x = 0; x < ba.Length; x++)
            {
                UpdateTextLine(ba[x]);
            }
            */
            stream.Write(ba, 0, ba.Length);        //send message to server
            stream.Flush();
            stream.Close();          //close network stream
        }

        public static void connect(String s)
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();      //create a TcpCLient socket to connect to server
            NetworkStream stream = null;

            //connecting to server socket with port 6000
            clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 6000);
            stream = clientSocket.GetStream();

            //joining message to server
            byte[] ba = Encoding.ASCII.GetBytes(s);
            /*
            for (int x = 0; x < ba.Length; x++)
            {
                UpdateTextLine(ba[x]);
            }
            */
            stream.Write(ba, 0, ba.Length);        //send message to server
            stream.Flush();
            stream.Close();          //close network stream
        }

        public void waitForConnection()
        {
            try
            {
                //Creating listening Socket
                TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

                UpdateText("waiting for server response");

                //Starts listening
                listener.Start();

                //Establish connection upon server request

                while (true)
                {
                    TcpClient connection = listener.AcceptTcpClient();   //connection is connected socket

                    UpdateText("Connetion is established");

                    //get the incoming data through a network stream---
                    NetworkStream serverStream = connection.GetStream();
                    byte[] buffer = new byte[connection.ReceiveBufferSize];

                    //read incoming stream
                    int bytesRead = serverStream.Read(buffer, 0, connection.ReceiveBufferSize);

                    String messageFromServer = Encoding.ASCII.GetString(buffer, 0, bytesRead);


                    UpdateText("Response from server \n" + messageFromServer);
                    accept(messageFromServer);

                    serverStream.Close();                         //close the netork stream
                                                                  

                }
            }
            catch (Exception e)
            {
                UpdateText("Communication (RECEIVING) Failed! \n " + e.StackTrace);
            }

        }


        private void UpdateText(String s)
        {
            Func<int> del = delegate ()
            {
                textBox1.AppendText(s + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }

        private void UpdateText2(String s)
        {
            Func<int> del = delegate ()
            {
                textBox2.AppendText(s + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }

        private void UpdateText3(String s)
        {
            Func<int> del = delegate ()
            {
                textBox2.AppendText(s);
                return 0;
            };
            Invoke(del);
        }
        public int serverJoinReply(String reply)
        {
            switch (reply)
            {
                case "PLAYERS_FULL#": UpdateText2("Players full"); return 1;
                case "ALREADY_ADDED#": UpdateText2("Already added"); return 2;
                case "GAME_ALREADY_STARTED#": UpdateText2("Game already started"); return 3;
                default: return 0;
            }

        }

        public int serverReply(String reply)
        {

            switch (reply)
            {
                case "PLAYERS_FULL#": UpdateText2("Players full"); return 1;
                case "ALREADY_ADDED#": UpdateText2("Already added"); return 2;
                case "GAME_ALREADY_STARTED#": UpdateText2("Game already started"); return 3;

                case "INVALID_CELL": UpdateText2("Invalid cell"); return 4;
                case "NOT_A_VALID_CONTESTANT": UpdateText2("Invalid contestant"); return 5;
                case "TOO_QUICK#": UpdateText2("Too quick"); return 6;
                case "CELL_OCCUPIED#": UpdateText2("Cell occupied"); return 7;
                case "OBSTACLE#": UpdateText2("Obstacle"); return 8;
                case "PITFALL#": UpdateText2("Pitfall"); return 9;

                case "DEAD#": UpdateText2("Game finished"); return 9;
                case "GAME_HAS_FINISHED#": UpdateText2("dead"); return 9;


                default: return 0;
            }

        }

        public void accept(String msg)
        {

            int number = this.serverReply(msg);

            if (number != 0)
            {
                serverReply(msg);
            }
            else if (number == 0)
            {
                
                if (msg.EndsWith("#"))
                {
                    msg = msg.Remove(msg.Length - 1);
                    char[] charArray = { ':' };
                    String[] tokens = msg.Split(charArray);

                    if (msg.StartsWith("I"))
                    {
                        
                        char[] CharArray2 = { ';' };
                        String[] brickWalls = tokens[2].Split(CharArray2);
                        String[] obstacles = tokens[3].Split(CharArray2);
                        String[] water = tokens[4].Split(CharArray2);

                        UpdateText2("\n\nInitial map deatails\n");
                        UpdateText2("brick wall locations : \n");
                        for (int i = 0; i < brickWalls.Length; i++) { UpdateText3(brickWalls[i] + "\t"); }

                        UpdateText2("\nobstacle locations : ");
                        for (int i = 0; i < obstacles.Length; i++) { UpdateText3(obstacles[i] + "\t"); }

                        UpdateText2("\nwater locations : ");
                        for (int i = 0; i < water.Length; i++) { UpdateText3(water[i] + "\t"); }
                    }
                    else if (msg.StartsWith("S"))
                    {
                        char[] CharArray2 = { ';' };
                        string[] playerDetails = tokens[1].Split(CharArray2);
                        contestant.playerName = playerDetails[0];
                        UpdateText2("\nNew player :" + contestant.playerName);
                        contestant.playerLocationX = int.Parse(playerDetails[1].Substring(0, 1));
                        contestant.playerLocationY = int.Parse(playerDetails[1].Substring(2, 1));
                        contestant.Direction = int.Parse(playerDetails[2]);
                        UpdateText2(contestant.ToString());
                    }
                    else if (msg.StartsWith("G"))
                    {
                        char[] CharArray2 = { ';' };
                        string[] playerDetails = tokens[1].Split(CharArray2);
                        contestant.playerName = playerDetails[0];
                        UpdateText2("\nCurrent deatails of " + contestant.playerName);
                        contestant.playerLocationX = int.Parse(playerDetails[1].Substring(0, 1));
                        contestant.playerLocationY = int.Parse(playerDetails[1].Substring(2, 1));
                        contestant.Direction = int.Parse(playerDetails[2]);
                        UpdateText2(contestant.ToString());
                    }
                    else if (msg.StartsWith("C"))
                    {
                        UpdateText2("\nCurrent coinpiles");
                        coinpile.CoinPileLocationX = int.Parse(tokens[1].Substring(0, 1));
                        coinpile.CoinPileLocationY = int.Parse(tokens[1].Substring(2, 1));
                        coinpile.lifetime = int.Parse(tokens[2]);
                        coinpile.price = int.Parse(tokens[3]);
                        UpdateText2(coinpile.ToString());
                    }
                    else if (msg.StartsWith("L"))
                    {
                        UpdateText2("\nCurrent life packs");
                        lifepack.LifePackLocationX = int.Parse(tokens[1].Substring(0, 1));
                        lifepack.LifePackLocationY = int.Parse(tokens[1].Substring(2, 1));
                        lifepack.lifetime = int.Parse(tokens[2]);
                        UpdateText2(coinpile.ToString());
                    }

                }
                else
                {
                    UpdateText2("Error in message received..");

                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        /*
        private void UpdateKeyEventUI(String s)
        {
            Func<int> del = delegate ()
            {
                txtKeyDisplay.Text = s;
                return 0;
            };
            Invoke(del);
        }*/
        
        private void txtKeyDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                connect("SHOOT#");
                //UpdateKeyEventUI("SHOOT");
            }
            else if (e.KeyCode == Keys.Up)
            {
                connect("UP#");
                //UpdateKeyEventUI("UP");
            }
            else if (e.KeyCode == Keys.Right)
            {
                connect("RIGHT#");
                //UpdateKeyEventUI("RIGHT");
            }
            else if (e.KeyCode == Keys.Down)
            {
                connect("DOWN#");
                //UpdateKeyEventUI("DOWN");
            }
            else if (e.KeyCode == Keys.Left)
            {
                connect("LEFT#");
                //UpdateKeyEventUI("LEFT");
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                connect("SHOOT#");
                //UpdateKeyEventUI("SHOOT");
            }
            else if (e.KeyCode == Keys.Up)
            {
                connect("UP#");
                //UpdateKeyEventUI("UP");
            }
            else if (e.KeyCode == Keys.Right)
            {
                connect("RIGHT#");
                //UpdateKeyEventUI("RIGHT");
            }
            else if (e.KeyCode == Keys.Down)
            {
                connect("DOWN#");
                //UpdateKeyEventUI("DOWN");
            }
            else if (e.KeyCode == Keys.Left)
            {
                connect("LEFT#");
                //UpdateKeyEventUI("LEFT");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
