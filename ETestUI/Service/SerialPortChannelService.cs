using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Service
{
    public class SerialPortChannelService : ICommunicationChannelService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private SerialPort serialPort = new SerialPort();
        private object lockobj = new object();
        public bool Open(string com)
        {
            try
            {
                this.serialPort.PortName = com;
                serialPort.BaudRate = 19200;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.Even;
                serialPort.StopBits = StopBits.One;
                this.serialPort.Open();
                this.serialPort.DiscardInBuffer();
                this.serialPort.DiscardOutBuffer();
                this.serialPort.WriteTimeout = 500;
                this.serialPort.ReadTimeout = 500;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }
        public void Close()
        {
            try
            {
                serialPort.Close(); 
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public string Send(string str)
        {
            throw new NotImplementedException();
        }
        //public string Send(string str)
        //{

        //}
    }
}
