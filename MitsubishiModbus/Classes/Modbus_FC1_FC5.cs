using System;
using System.Threading;
using System.Reflection;
using System.Net.Sockets;
using System.Diagnostics;

namespace MitsubishiModbus.Classes
{
    public class Modbus_FC1_FC5
    {
        private readonly object _CommunicationMutex;
        private readonly object _InputsMutex;
        private readonly object _OutputsMutex;

        private const int               _CLIENT_SEND_RECV_TIMEOUT = 500;
        private TcpClient               _Client;
        private Thread                  _ClientThread;
        private CancellationTokenSource _ClientBreakConn;

        private readonly byte   _SlaveNumber;
        private readonly ushort _InputsStartAddress;
        private readonly ushort _OutputsStartAddress;
        private readonly ushort _InputsCount;
        private readonly ushort _OutputsCount;
        private volatile bool[] _Inputs;
        private volatile bool[] _Outputs;

        public Modbus_FC1_FC5(ushort inputsCount, ushort outputsCount, ushort inputsStartAddress, ushort outputsStartAddress, byte slaveNumber = 0)
        {
            _CommunicationMutex = new object();
            _InputsMutex        = new object();
            _OutputsMutex       = new object();

            _SlaveNumber         = slaveNumber;
            _InputsStartAddress  = inputsStartAddress;
            _OutputsStartAddress = outputsStartAddress;
            _InputsCount         = inputsCount;
            _OutputsCount        = outputsCount;

            if (_InputsCount > 0)
            {
                _Inputs = new bool[_InputsCount];
            }
            if (_OutputsCount > 0)
            {
                _Outputs = new bool[_OutputsCount];
            }
        }

        public void Connect(string ip, bool initOutputs = false)
        {
            if (Connected)
            {
                throw new Exception("There is existing connection to " + _Client.Client.RemoteEndPoint);
            }
            _Client = new TcpClient();
            _Client.ReceiveTimeout = _CLIENT_SEND_RECV_TIMEOUT;
            _Client.SendTimeout    = _CLIENT_SEND_RECV_TIMEOUT;

            if (!_Client.ConnectAsync(ip, 502).Wait(_CLIENT_SEND_RECV_TIMEOUT))
            {
                throw new Exception("Cannot connect to " + ip + " !");
            }
            if (initOutputs)
            {
                ReadOutputs();
            }
            _ClientBreakConn = new CancellationTokenSource();

            _ClientThread = new Thread(StatusUpdateThread);
            _ClientThread.Priority = ThreadPriority.Highest;
            _ClientThread.IsBackground = true;
            _ClientThread.Start();
        }

        public bool Connected
        {
            get
            {
                return _ClientThread != null && _ClientThread.IsAlive && _Client != null && _Client.Connected;
            }
        }

        public void Disconnect() // Don't kill me
        {
            if (Connected)
            {
                _ClientBreakConn.Cancel();
            }
            else
            {
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < _CLIENT_SEND_RECV_TIMEOUT + 200)
            {
                if (!Connected)
                {
                    break;
                }
            }
            stopwatch.Stop();

            if (Connected)
            {
                try
                { _Client.Close(); }
                catch { }
            }
        }

        public bool GetInput(ushort coilAddress)
        {
            string thisMethodFullName = typeof(Modbus_FC1_FC5) + "." + MethodBase.GetCurrentMethod().Name;

            if (coilAddress >= _InputsCount)
            {
                throw new ArgumentOutOfRangeException(thisMethodFullName + ": invalid address [" + coilAddress.ToString() + "]."
                                                    + " Possible range: [" + 0.ToString() + " -- "
                                                    + (_InputsCount - 1).ToString() + "].");
            }
            lock (_InputsMutex)
            {
                return _Inputs[coilAddress];
            }
        }

        public void SetOutput(ushort coilAddress, bool value)
        {
            string thisMethodFullName = typeof(Modbus_FC1_FC5) + "." + MethodBase.GetCurrentMethod().Name;

            if (coilAddress >= _OutputsCount)
            {
                throw new ArgumentOutOfRangeException(thisMethodFullName + ": invalid address [" + coilAddress.ToString() + "]."
                                                    + " Possible range: [" + 0.ToString() + " -- "
                                                    + (_OutputsCount - 1).ToString() + "].");
            }
            lock (_OutputsMutex)
            {
                _Outputs[coilAddress] = value;
            }
        }

        private void ReadOutputs()
        {
            lock (_OutputsMutex)
            {
                for (ushort i = 0, reg = _OutputsStartAddress; i < _OutputsCount; ++i, ++reg)
                {
                    _Outputs[i] = ReadCoil(reg);
                }
            }
        }

        private void StatusUpdateThread()
        {
            while (!_ClientBreakConn.IsCancellationRequested)
            {
                try
                {
                    lock (_OutputsMutex)
                    {
                        for (ushort i = 0, reg = _OutputsStartAddress; i < _OutputsCount; ++i, ++reg)
                        {
                            WriteSingleCoil(reg, _Outputs[i]);
                        }
                    }
                    lock (_InputsMutex)
                    {
                        for (ushort i = 0, reg = _InputsStartAddress; i < _InputsCount; ++i, ++reg)
                        {
                            _Inputs[i] = ReadCoil(reg);
                        }
                    }
                }
                catch
                {
                    break;
                }
                Thread.Sleep(20);
            }
            if (_Client.Connected)
            {
                try
                { _Client.Close(); }
                catch { }
            }
            lock (_OutputsMutex)
            {
                for (ushort i = 0; i < _OutputsCount; ++i)
                {
                    _Outputs[i] = false;
                }
            }
            lock (_InputsMutex)
            {
                for (ushort i = 0; i < _InputsCount; ++i)
                {
                    _Inputs[i] = false;
                }
            }
        }

        private bool ReadCoil(ushort coilAddress)
        {
            byte[] requestMessage = new byte[12];

            requestMessage[0] = 0;    // Transaction id
            requestMessage[1] = 0;    // Transaction id
            requestMessage[2] = 0x00; // Protocol id
            requestMessage[3] = 0x00; // Protocol id
            requestMessage[4] = 0x00; // Length of the payload  -------------------|
            requestMessage[5] = 0x06; // Length of the payload  -------------------|
                                      //                                           |
            requestMessage[6] = _SlaveNumber;               // Slave number      1-|
            requestMessage[7] = 0x01;                       // Function code     2-|
            requestMessage[8] = (byte)(coilAddress >> 8);   // Register address  3-|
            requestMessage[9] = (byte)(coilAddress & 0xFF); // Register address  4-|
                                                            //                     |
            requestMessage[10] = 0x00;                      // Register count    5-|
            requestMessage[11] = 0x01;                      // Register count    6-|

            byte[] responsePayload;

            lock (_CommunicationMutex)
            {
                _Client.GetStream().Write(requestMessage, 0, requestMessage.Length);

                byte[] responseHead = new byte[6];
                _Client.GetStream().Read(responseHead, 0, responseHead.Length);

                int payloadSize = (responseHead[4] << 8) + responseHead[5];

                responsePayload = new byte[payloadSize];
                _Client.GetStream().Read(responsePayload, 0, payloadSize);
            }
            string thisMethodFullName = typeof(Modbus_FC1_FC5) + "." + MethodBase.GetCurrentMethod().Name;

            if (responsePayload[1] == 0x81) // 0x80 + Function code when some error occurs
            {
                throw new Exception(thisMethodFullName + ": 0x81 function code returned. Exception code: " + responsePayload[2].ToString() + ".");
            }
            if (responsePayload.Length != 4)
            {
                string responsePayloadParsed = String.Empty;

                foreach (byte ch in responsePayload)
                {
                    responsePayloadParsed += "[" + ch.ToString() + "]";
                }
                throw new Exception(thisMethodFullName + ": response payload is unexpected! " + responsePayloadParsed);
            }
            return responsePayload[3] != 0; // Coil status
        }

        private void WriteSingleCoil(ushort coilAddress, bool value)
        {
            byte[] requestMessage = new byte[12];

            requestMessage[0] = 0;    // Transaction id
            requestMessage[1] = 0;    // Transaction id
            requestMessage[2] = 0x00; // Protocol id
            requestMessage[3] = 0x00; // Protocol id
            requestMessage[4] = 0x00; // Length of the payload  -------------------|
            requestMessage[5] = 0x06; // Length of the payload  -------------------|
                                      //                                           |
            requestMessage[6] = _SlaveNumber;               // Slave number      1-|
            requestMessage[7] = 0x05;                       // Function code     2-|
            requestMessage[8] = (byte)(coilAddress >> 8);   // Register address  3-|
            requestMessage[9] = (byte)(coilAddress & 0xFF); // Register address  4-|
                                                            //                     |
            requestMessage[10] = value ? (byte)0xFF         // Value if ON       5-|
                                       : (byte)0x00;        // Value if OFF      5-|
            requestMessage[11] = 0x00;                      // Value static      6-|

            byte[] responsePayload;

            lock (_CommunicationMutex)
            {
                _Client.GetStream().Write(requestMessage, 0, requestMessage.Length);

                byte[] responseHead = new byte[6];
                _Client.GetStream().Read(responseHead, 0, responseHead.Length);

                int payloadSize = (responseHead[4] << 8) + responseHead[5];

                responsePayload = new byte[payloadSize];
                _Client.GetStream().Read(responsePayload, 0, payloadSize);
            }
            string thisMethodFullName = typeof(Modbus_FC1_FC5) + "." + MethodBase.GetCurrentMethod().Name;

            if (responsePayload[1] == 0x85) // 0x80 + Function code when some error occurs
            {
                throw new Exception(thisMethodFullName + ": 0x85 function code returned. Exception code: " + responsePayload[2].ToString() + ".");
            }
            if (responsePayload.Length != 6)
            {
                string responsePayloadParsed = String.Empty;

                foreach (byte ch in responsePayload)
                {
                    responsePayloadParsed += "[" + ch.ToString() + "]";
                }
                throw new Exception(thisMethodFullName + ": response payload is unexpected! " + responsePayloadParsed);
            }
        }
    }
}
