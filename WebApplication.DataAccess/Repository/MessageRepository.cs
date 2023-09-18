using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.DataAccess.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IQueryable<Message> GetAllUserMessages(string userId)
        {
            var messages = _db.Messages.Where(x => x.UserId == userId); 
            return messages;    
        }

    }
}
