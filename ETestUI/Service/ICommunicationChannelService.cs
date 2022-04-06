using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETestUI.Service
{
    public interface ICommunicationChannelService
    {
        public bool Open(string com);
        public void Close();
        public string Send(string str);
    }
}
