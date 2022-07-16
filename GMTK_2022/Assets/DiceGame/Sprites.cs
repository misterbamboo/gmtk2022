using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class Sprites : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();
        private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
        public static Sprites Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            foreach (var sprite in sprites)
            {
                spriteDict.Add(sprite.name, sprite);
            }
        }

        public Sprite Get(string name)
        {
            if (spriteDict.ContainsKey(name))
            {
                return spriteDict[name];
            }
            else
            {
                throw new Exception($"{name} is not a valid sprite name");
            }
        }
    }
}

