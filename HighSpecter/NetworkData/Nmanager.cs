using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HighSpecter.NetworkData
{
    /// <summary>
    /// This class controlls network of the computer it runs on. It's functionality is as follows:
    /// Make sure there is a servlet listenining on every IPv4 adress associated with a connection that's UP
    /// Send updates through every servlet
    /// Make sure hosted Network is up
    /// </summary>
    static class Nmanager
    {
        private static volatile Servlet[] _network = new Servlet[0];
        
        private static readonly int RefreshCount = 20; //sets how often network is refreshed
        static int tickCounter = 0;  //counts ticks, when number of ticks reaches NetworkRefreshCount, it refreshes the network paremeters 

        private static System.Diagnostics.Process pHostedNet = new System.Diagnostics.Process();

        private static StreamReader StandardOutput;
        private static int hostNetworkStartAttempts = 0;
        private static bool HostedNetUp = false;
        private static string hNetworkSuccess = "The hosted network started";



        //Point of entry for 
        public static void Tick(Object sender)
        {
                       
            if (_network.Length == 0 || tickCounter > RefreshCount)
            {
                updateConnections();
                tickCounter = 0;
            }
            else
            {
                tickCounter++;
                SendNetworkUpdate();
            }
        }

        private static void SendNetworkUpdate()
        { 
            lock (_network)
            {
                lock(Logic.Status)
                {
                    if (Logic.Status != null)
                    {
                        foreach (Servlet connection in _network)
                        {
                            connection.SendData(Logic.Status);
                        }
                        Logic.Status.LogChanges = "";   //clears the log-changes, as the changes were submitted to clients
                    }
                    else
                    {
                        Logging.Add("Tried To send an empty frame!");
                    }
                }
            }
        }

        private static void updateConnections()
        {
            lock (_network)
            {
                //Start and/or update network IP list

                List<IPAddress> newIpAdresses = new List<IPAddress>(); //for storing IP adresses that the programm did not yet start listening on
                List<IPAddress> presentIpAdresses = new List<IPAddress>(); //for storing all IP addresses of the right type


                foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.Description.Contains("Microsoft Hosted Network Virtual Adapter")) //find the HostedNet adapter
                    {
                        if (netInterface.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Down) // if it's down'
                        {
                            HostedNetUp = false;
                            StartHostedNetwork();  ///start it
                        }
                        else
                        {
                            HostedNetUp = true; 

                        }
                        
                    }
                    if (netInterface.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up) //check if the interface is up
                    {
                        IPInterfaceProperties ipProps = netInterface.GetIPProperties();

                        foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                        {
                            if (addr.Address.AddressFamily == AddressFamily.InterNetwork) //checks if the adress is an IPv4 one
                            {
                                presentIpAdresses.Add(addr.Address);

                                if (_network.Length > 0)
                                {
                                    if (Array.Exists(_network, p => p.LocalIp.Equals(addr.Address)))
                                    {
                                        //IP address is already on the list 
                                    }
                                    else
                                    {
                                        newIpAdresses.Add(addr.Address); //add it to the list, it's not there
                                    }
                                }
                                else
                                {
                                    newIpAdresses.Add(addr.Address);
                                    //networking is not initialised, put everything on the list
                                }
                            }
                        }
                    }
                }


                List<Servlet> newlist = new List<Servlet>();

                int removedAddresses = 0;
                String listofIpAdresses = ""; //string that stores every IP address that hte programm is working with

                foreach (var connection in _network)
                {
                    if (presentIpAdresses.Exists(p => p.Equals(connection.LocalIp))) //check if the connection still exists
                    {
                        newlist.Add(connection);
                        //this is a bad way to retry connections, but I have no better ideas at het moment!
                        if (! connection.IsActive()) //try resetting the connection a bit! 
                        {
                            Logging.Add("Resetting connection " + connection.LocalIp);
                            connection.Reset();
                        }

                        listofIpAdresses += connection.LocalIp.ToString() + ", ";

                    }
                    else
                    {
                        removedAddresses++;
                        connection.Dispose(); //if it is not, dispose of it
                    }

                }

                if (newIpAdresses.Count > 0 || removedAddresses > 0)
                {
                    foreach (var connection in newIpAdresses)
                    {
                        var newconnection = new Servlet(connection);
                        newconnection.Readnetwork();
                        newlist.Add(newconnection);
                        listofIpAdresses += connection.ToString() + ", ";
                    }


                    Logging.Add("IP addresses changed: the connections are " + listofIpAdresses);

                    _network = newlist.ToArray<Servlet>();
                }


            }
        }


        private static async void StartHostedNetwork()
        {
            Logging.Add("Attempting to start hosted network");
            bool success = false;

            String netshOutput; 

            pHostedNet.StartInfo.FileName = "netsh.exe";
            pHostedNet.StartInfo.Arguments = "wlan start hostednetwork";
            pHostedNet.StartInfo.UseShellExecute = false;
            pHostedNet.StartInfo.RedirectStandardOutput = true;
            pHostedNet.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            
            while(! success)
            {
                pHostedNet.Start();
                StandardOutput = pHostedNet.StandardOutput;
                netshOutput = await StandardOutput.ReadLineAsync();
                if(netshOutput.Contains(hNetworkSuccess))
                {
                    //good
                    success = true;
                    Logging.Add("Hosted Network Started");
                    break;
                }
                
                else
                {//something happened, wait and do it again
                    hostNetworkStartAttempts++;
                    Logging.Add("Tried to start HostedNetwork, failed and got " + netshOutput); 
                    await Task.Delay(1000);                   

                }
            }

            updateConnections(); 

        }



        private static void SetHostedNetwork()
        {
            System.Diagnostics.Process p1 = new System.Diagnostics.Process();
            p1.StartInfo.FileName = "netsh.exe";
            p1.StartInfo.Arguments = "wlan set hostednetwork mode=allow ssid=Anywave3 key=asteroidsareamyth";
            p1.StartInfo.UseShellExecute = false;
            p1.StartInfo.RedirectStandardOutput = true;
            p1.Start();
        }


            /*
             * int nIP = 0; 
                foreach (IPAddress ipaddress in iphostentry.AddressList)
                {
                    _network[nIP] = new NWaitForConnection(this, ipaddress);
                    _network[nIP].Readnetwork(); 
                    nIP++;
                }
                
                network_started = true;
             */

            /*
            //if the number of network connections hsa changed, we need to change the number of network interfaces the application is listening to.
            //We want to listed for connections on every network interface. 
            if (iphostentry.AddressList.Length != _network.Length)
            {
                //Move old list to a different name
                NWaitForConnection[] oldnetworklist = _network;

                //create a new array of size equal to number of new interfaces.
                _network = new NWaitForConnection[iphostentry.AddressList.Length];
                int index = 0;  //create an index

                //go throught every ip address on the computer
                foreach (IPAddress ipaddress in iphostentry.AddressList)
                {//check if it's already in the list
                    bool inTheList = false;

                    //go though the list of connection we already have open
                    foreach (NWaitForConnection oldCon in oldnetworklist)
                    { //and if we find a connection that is in the new list but already open
                        if (ipaddress.Equals(oldCon.LocalIp))
                        {  //just movet he reference to the new array
                            _network[index] = oldCon;
                            inTheList = true;
                        }
                    }//if the connection is not on the list
                    if (inTheList == false) //open a new connection on that interface
                        _network[index] = new NWaitForConnection(this, ipaddress);
                    //increment the index
                    index ++; 
                }
                //Finally let's close any connections we have for interfaces that might have been disconnected
                foreach (NWaitForConnection oldCon in oldnetworklist)
                {
                    
                    bool isDisconnected = true;
                    //for each old connection, iterate through the list of adapters currently present
                    foreach (IPAddress ipaddress in iphostentry.AddressList)
                    {//if we find the connection with the same IP adress in the list, we mark the connection as 
                        if (ipaddress.Equals(oldCon.LocalIp))
                            isDisconnected = false;
                    }
                    if (isDisconnected)
                    {
                        oldCon.Dispose();
                    }

                }

            }
             * */

        

    }
}
