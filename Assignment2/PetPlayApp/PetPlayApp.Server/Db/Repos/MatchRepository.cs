using PetPlayApp.Server.Models.Match;

namespace PetPlayApp.Server.Db.Repos
{
    public class MatchRepository : Repository<Match>
    {
        public MatchRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
