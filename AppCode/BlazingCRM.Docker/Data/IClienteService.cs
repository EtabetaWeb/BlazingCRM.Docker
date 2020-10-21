using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingCRM.Docker.Data
{
    public interface IClienteService
    {
        Task<List<Cliente>> ElencoClienti();
        Task<bool> AggiungiCliente(Cliente cliente);
        Task<bool> ModificaCliente(string id, Cliente cliente);
        Task<Cliente> DatiCliente(string id);
        Task<bool> CancellaCliente(string id);
    }
}
