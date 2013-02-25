using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CompuWatch
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract] 
        bool IsOnline();

    }
    public class Contracts : IService
    {
        public bool IsOnline()
        {
            return true;
        }
    }
}
