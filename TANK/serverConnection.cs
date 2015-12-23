﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace TANK
{
    class serverConnection
  
    {
        bool errorOcurred = false;
        Socket connection = null; //The socket that is listened to     
        TcpListener listener = null;

        public string waitForConnection()
        {
            string s = "";
            try
            {
                
                    //Creating listening Socket
                    this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);

                s+="waiting for server response\n";

                //Starts listening
                this.listener.Start();

                //Establish connection upon server request
                while (true)
                {

                    connection = listener.AcceptSocket();   //connection is connected socket

                    s+="Connetion is established\n";

                    //Fetch the messages from the server
                    int asw = 0;
                    //create a network stream using connecion
                    NetworkStream serverStream = new NetworkStream(connection);
                    List<Byte> inputStr = new List<byte>();

                    //fetch messages from  server
                    while (asw != -1)
                    {
                        asw = serverStream.ReadByte();
                        inputStr.Add((Byte)asw);
                    }

                    String messageFromServer = Encoding.UTF8.GetString(inputStr.ToArray());

                    serverResponce msg = new serverResponce();
                    s+="Response from server \n" + messageFromServer+msg.accept(messageFromServer);
                    
                    //clientConnection.Connect(new KeyEvents().getKeyCommand());
                    serverStream.Close();                         //close the netork stream
                    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Communication (RECEIVING) Failed! \n " + e.StackTrace);
                errorOcurred = true;
            }
            finally
            {
                if (connection != null)
                    if (connection.Connected)
                        connection.Close();
                if (errorOcurred)
                    this.waitForConnection();
            }
            return s;
        }

    }
}
