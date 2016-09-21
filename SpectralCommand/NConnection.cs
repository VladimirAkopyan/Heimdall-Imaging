using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using HighSpecter.NetworkData;

namespace SpectralCommand
{
    internal class NConnection
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();
        private TcpClient _connection;
        private Nio<TelemetryR, Command> _NetworkIo;
        public List<TelemetryR> Telemetry = new List<TelemetryR>();
        private readonly MainWindow UI;

        public NConnection(MainWindow caller)
        {
            UI = caller;
            cts = new CancellationTokenSource();
            cts.Token.Register(Reset);
        }

        /* Takes user input, IP and Port*/

        public async void Connect(string address, ushort port)
        {
            //try to parse user input into a valid IP and Port
            _connection = new TcpClient();
            try
            {
                await _connection.ConnectAsync(address, port);
                MainWindow._gstatus = MainWindow.State.ConnectedOff;
                UI.UpdateUi("Connection Succesfull", true);

                using (_NetworkIo = new Nio<TelemetryR, Command>())
                {
                    _NetworkIo.Initiate(_connection.GetStream(), new TelemetryR(), new Command(false, 0, 0), cts);

                    while (!cts.IsCancellationRequested)
                    {

                            var temp = await _NetworkIo.RecieveData();
                            Telemetry.Add(temp);
                            UI.UpdateUi(temp);


                    }
                }
            }
            catch (Exception exception)
            {
                MainWindow._gstatus = MainWindow.State.Idle;
                UI.UpdateUi(exception.Message);
                cts.Cancel();
            }
        }

        public void send_data(Command data)
        {
                _NetworkIo.WriteData(data);
        }

        /*Closes All Connections and releases resources*/

        public void Reset()
        {
            //If cancellation has not been requested yet, request it
            cts.Cancel();
            cts = new CancellationTokenSource();
            cts.Token.Register(Reset);

            if (_connection != null)
            {
                //Close the TCP connection
                _connection.Close();
                _connection = null;
            }
        }
    }
}