using System;

namespace DiceGame.Combat.Presentation.Exceptions
{
    public class CharacterTransformNotFound : Exception
    {
        public CharacterTransformNotFound(int characterId) 
            : base($"Character transform with CharacterId: {characterId}, not found")
        {
        }
    }
}
