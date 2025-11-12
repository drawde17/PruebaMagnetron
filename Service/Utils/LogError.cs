using Domain.DTO;
using Domain.Models;
using Infraestructure;
using Infraestructure.Repository;

namespace Service.Utils
{
    public class LogError<T>
    {
        private MagnetronContext _context;
        public LogError(MagnetronContext context)
        {
            _context = context;
        }
        public Response<T> Set(string service, Exception ex)
        {
            var repoLog = new Repository<Log>(_context);
            repoLog.Add(new Log()
            {
                Error = service,
                Message = ex.Message
            });
            repoLog.Save();
            return new Response<T>()
            {
                Status = false,
                Message = "There have been an error, try again."
            };
        }
    }
}
