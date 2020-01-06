using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TWitcher3.Core.Games
{
    public interface IGameCommandFactory
    {
        IGameCommandReference Create(string sender, string rawCommandText);
    }
}
