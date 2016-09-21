using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HighSpecter.NetworkData
{
    public class Nio<TIn, TOut> : IDisposable 
    {

        /*network stream and serialisers*/
        private NetworkStream _networkstream;
        private DataContractSerializer _serialiserOutgoing;
        private DataContractSerializer _serialiserIncoming;

        /*Buffmanager for 2 buffers, 1 for incoming 2 for outgoing data */
        private const int _bufferSize = 2;
        private BufferManager _bufferManager;
        private byte[] _memoryBytesOut;
        private byte[] _memoryBytesIn;
        //Wraps _memoryBytesOut as a stream for use with serialisers

        private long _nofData = 0;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;



        /*Cahncellation token*/
        private static CancellationTokenSource _cts;

        public Nio ()
        {
            
        } 

        /*must be given a *sample* of classes that are incoming and send through the network, and a delegate to be called when UI is updated
         Samples are dicarded*/
        public void Initiate(NetworkStream networkstream, TIn incoming, TOut outgoing, CancellationTokenSource cancellation)
        {
            
            _networkstream = networkstream;
            _networkstream.WriteTimeout = 1000;
            _networkstream.ReadTimeout = 2000;
            
            _cts = cancellation;
            _streamWriter = new StreamWriter(_networkstream);
           _streamReader = new StreamReader(_networkstream);
        }


        /*asyncronous method to recieve data*/
        public async Task<TIn> RecieveData()
        {
            string test = await _streamReader.ReadLineAsync();

            return JsonConvert.DeserializeObject<TIn>(test);

         }

        public async Task<bool> WriteData(TOut data)
        {

            string test = JsonConvert.SerializeObject(data);
            test += Environment.NewLine;
            await _streamWriter.WriteAsync(test);
            await _streamWriter.FlushAsync();
            return true; 
        }


        public void Dispose()
        {
            _streamWriter.Dispose();
            _streamReader.Dispose();
            _networkstream.Dispose();

        }
    }
}
