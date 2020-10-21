using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingCRM.Docker.Data
{
    public class ClienteService : IClienteService
    {
        private readonly SqlDbContext _dbContext;
        public ClienteService(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Cliente>> ElencoClienti()
        {
            return await _dbContext.Clienti.ToListAsync();
        }
        public async Task<bool> AggiungiCliente(Cliente cliente)
        {
            cliente.Id = Guid.NewGuid().ToString();
            _dbContext.Add(cliente);
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public async Task<Cliente> DatiCliente(string id)
        {
            return await _dbContext.Clienti.FindAsync(id);
        }
        public async Task<bool> ModificaCliente(string id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return false;
            }
            _dbContext.Entry(cliente).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancellaCliente(string id)
        {
            var idcliente = await _dbContext.Clienti.FindAsync(id);
            if (idcliente == null)
            {
                return false;
            }
            _dbContext.Clienti.Remove(idcliente);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
