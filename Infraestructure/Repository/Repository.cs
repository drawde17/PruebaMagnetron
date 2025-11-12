using Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected MagnetronContext _context;
        protected DbSet<TEntity> _dbSet;

        public Repository(MagnetronContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public void Add(TEntity data) => _dbSet.Add(data);


        public void Delete(int id)
        {
            var dataToDelete = _dbSet.Find(id);
            _dbSet.Remove(dataToDelete);
        }

        public IEnumerable<TEntity> Get() => _dbSet.ToList();

        public TEntity Get(int id) => _dbSet.Find(id);

        public void Save() => _context.SaveChanges();

        public void Update(TEntity data)
        {
            _dbSet.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
        }
    }
}
