using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DiceGame.Combat.Entities.Exceptions
{
    public class CharacterNotFoundException : Exception
    {
        public CharacterNotFoundException(int id)
            : base($"Character with Id={id} not found")
        {
        }
    }
}
