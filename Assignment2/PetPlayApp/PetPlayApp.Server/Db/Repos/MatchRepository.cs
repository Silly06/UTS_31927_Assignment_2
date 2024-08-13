using PetPlayApp.Server.Models;

namespace PetPlayApp.Server.Db.Repos
{
    public class MatchRepository : Repository<Match>
    {
        public MatchRepository(DatabaseContext context) : base(context)
        {
        }

        public void RemoveAll()
        {
            var matches = GetAll();
            foreach (Match match in matches)
            {
                _context.Remove(match);
            }
            SaveChanges();
        }
    }
}
