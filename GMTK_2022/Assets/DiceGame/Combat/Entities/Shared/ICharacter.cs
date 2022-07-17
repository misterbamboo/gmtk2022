using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DiceGame.Combat.Entities.Shared
{
    public interface ICharacter
    {
        float Attack { get; }
        int Id { get; }

        void Hit(float damages);
        bool IsDead();
    }
}
