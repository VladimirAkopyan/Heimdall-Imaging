using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using HighSpecter.NetworkData;


namespace HighSpecter
{
    /// <summary>
    /// This class is instantiated for every IP address where Anywave must listen for connections. 
    /// It will accept an incoming tcp connection (only 1 at a time atm). it will recieve commands, and it will send network updates if needed
    /// It will continuously attempt to restart itself if an unknown exception occurs. 
    /// </summary>
    public class Servlet
    {
        private TcpListener _tcpListen;
        private TcpClient _tcpClient;
   
        private CancellationTokenSource cts = new CancellationTokenSource();
        public readonly IPAddress LocalIp;
        private ushort _port;

        private Nio<Command, TelemetryR> NetworkIo;

        public static UI_NetStatusPanel Userinterface; /// This needs to be changed! It's a UI reference used for updates! 
        private static int ClientNumber = 0; 
        

        private long _sentframes = 0;
        private long _clientsConnected = 0;
        private long _commandsRecived = 0;
        private long _sesionID = 0;

        public int _connectionErorCount = 0;
        private volatile bool Active = false;

        public static List<Servlet> DisposedConnections= new List<Servlet>();
        public static List<Servlet> AllConnections = new List<Servlet>();
        

        /*networkUIupdate is a delegate of type UI_commandUpdate, contained in UI class. 
         * Objects recieved from network will be passed to the delegate */
        public Servlet(IPAddress ipAddress, ushort  port=65432)
        {


            LocalIp = ipAddress;
            _port = port;
            cts.Token.Register(Reset);

            var localSesionId = _sesionID;
        }

        public async void SendData(TelemetryR data)
        {
            var localSesionId = _sesionID;
            if (NetworkIo == null || !_tcpClient.Connected)
            {
               // _userinterface.Update_IO_Displays(0, _clientsConnected, _sentframes, _commandsRecived);
                //cts.Cancel();
            }
            else
            {//we get  "cannot accees disposed object here, this is an important error
                try
                {
                    if (await NetworkIo.WriteData(data))
                    {
                        _sentframes++;
                        Userinterface.Update_IO_Displays(-1, _clientsConnected, _sentframes, _commandsRecived);
                    }
                    else
                    {
                        Userinterface.Update_feedback("Sending Telemetry Failed");
                        if (_sesionID == localSesionId)
                            cts.Cancel();
                    }
                }
                catch(Exception exception)
                {
                    Userinterface.Update_feedback("Sending Telemetry Failed");
                    Logging.Add(exception.ToString() + exception.Message); 
                    if (_sesionID == localSesionId)
                        cts.Cancel();
                }

            }
        }


        public async void Readnetwork()
        {
            long localSesionId = _sesionID;
            var localCancellation = cts;

            if (!AllConnections.Exists(p => p == this)) //if it is not in the list of dysfunctional connections, ad it there
                AllConnections.Add(this);

            try
            {
                _tcpListen = new TcpListener(LocalIp, _port);
                Userinterface.Update_IP_Config(LocalIp, _port);
                _tcpListen.Start();

                Active = true; 
                Userinterface.Update_IO_Displays(0, _clientsConnected, _sentframes, _commandsRecived);
                using (_tcpClient = await _tcpListen.AcceptTcpClientAsync())
                {
                    
                    using (NetworkIo = new Nio<Command, TelemetryR>())
                    {

                       
                        NetworkIo.Initiate
                        (_tcpClient.GetStream(), new Command(false, 1, 1), new TelemetryR(), cts);
                        _clientsConnected++;
                        Userinterface.Update_IO_Displays(1, _clientsConnected, _sentframes, _commandsRecived, true);

                        while (!localCancellation.IsCancellationRequested)
                        {
                            
                            try
                            {
                                Command temp = await NetworkIo.RecieveData();
                                _commandsRecived++;
                                Userinterface.Update_IO_Displays(1, _clientsConnected, _sentframes, _commandsRecived);
                                Userinterface.Update_Command(temp);
                            }
                            catch (ArgumentNullException exception)
                            {
                                Logging.Add(exception.ToString() + exception.Message);
                                /* Indicates Disconnection*/
                                if (_sesionID == localSesionId)
                                    cts.Cancel();
                                break;
                            }
                            catch (System.IO.IOException exception)
                            {
                                Logging.Add(exception.ToString() + exception.Message);
                                if (_sesionID == localSesionId)
                                    cts.Cancel();
                                break;
                            }
                            
                        }
                    }
                }
            }
            catch (Exception  exception)
            {
                //_userinterface.Update_feedback("Port is already in use, choosing random port");
                /*Here was a really bad idea with changing ports
                var rand = new Random();
                _port = (ushort)rand.Next(0, 65000);
                 * 
                 * 
                 */
                if (exception.Message.Contains("address is not valid "))
                {
                    _connectionErorCount++;
                    if(! DisposedConnections.Exists(p =>p == this)) //if it is not in the list of dysfunctional connections, ad it there
                        DisposedConnections.Add(this);
                    Active = false;
                    Dispose();
                    Logging.Add("Attempted to start listeninig on invalid IP address " + LocalIp.ToString());
                }
                else if (exception.Message.Contains("Only one usage of each socket address (protocol/network address/port) is normally permitted"))
                {
                    _connectionErorCount++;
                    if (!DisposedConnections.Exists(p => p == this)) //if it is not in the list of dysfunctional connections, ad it there
                        DisposedConnections.Add(this);
                    Active = false;
                    Dispose();

                    string listofDisposedconnections = "Disposed COnnections:";

                    string listofAllconnections = "All COnnections:";

                    foreach (Servlet connection in DisposedConnections)
                        listofDisposedconnections += connection.LocalIp + "\n";

                    foreach (Servlet connection in AllConnections)
                        listofAllconnections += connection.LocalIp + "\n";

                    Logging.Add("Attempted to start listening on " + LocalIp.ToString() + " but socket address is already in use \n" + listofAllconnections + listofDisposedconnections);
                    
                }
                else if (exception.Message.Contains("Cannot access a disposed object"))
                {
                    if (!DisposedConnections.Exists(p => p == this)) //if it is not in the list of dysfunctional connections, ad it there
                        DisposedConnections.Add(this);
                    Active = false;
                    Dispose();
                    Logging.Add("Connection " + LocalIp.ToString() + "was closed"); 
                }
                else
                {
                    Active = false;
                    Logging.Add(exception.ToString() + exception.Message);
                    if (_sesionID == localSesionId)
                        cts.Cancel();
                }

            }
        }

        public void Reset()
        {
            Active = false;
            Interlocked.Increment(ref _sesionID);
            Userinterface.Update_IO_Displays(0, _clientsConnected, _sentframes, _commandsRecived);
           // _userinterface.Update_IO_Displays(0, _clientsConnected, _sentframes, _commandsRecived);
            _tcpListen.Stop();
            
            //await Task.Delay(300);
            cts = new CancellationTokenSource();
            cts.Token.Register(Reset);
            Readnetwork();
        }

        public void Dispose()
        {
            Active = false;
            if (_tcpListen != null)
            {
                _tcpListen.Stop();
                

            }
            if (_tcpClient != null)
                _tcpClient.Close();
        }

        public bool IsActive()
        {
            return Active; 
        }

        
    }
}
