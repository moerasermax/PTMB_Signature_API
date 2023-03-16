using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PTMB_Signature_API.Model.Abstract;
using PTMB_Signature_API.Interface;
namespace PTMB_Signature_API.Interface
{
    public interface I_Controller_Signer : I_Signature,I_Mission,I_Login
    {

    }
}
