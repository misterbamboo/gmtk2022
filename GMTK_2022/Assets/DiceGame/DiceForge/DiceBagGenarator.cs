using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceGame
{
    public class DiceBagGenarator : MonoBehaviour
    {
        [SerializeField] DiceCube diceCubePrefab;
        private DiceBag diceBag;

        void Start()
        {
            diceBag = new DiceBag(GenerateDices());
            for (int i = 0; i < diceBag.TotalCount; i++)
            {
                var dice = diceBag.Draw(1).First();
                var x = UnityEngine.Random.Range(Screen.safeArea.x, Screen.safeArea.size.x);
                var y = UnityEngine.Random.Range(Screen.safeArea.y, Screen.safeArea.size.y);
                var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y));

                var diceCube = Instantiate(diceCubePrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                diceCube.InitDice(dice);
            }
        }

        private static IEnumerable<Dice> GenerateDices()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return new Dice(GenerateFaces());
            }
        }

        private static IEnumerable<Face> GenerateFaces()
        {
            var colors = Enum.GetValues(typeof(DiceColors)).Cast<DiceColors>().ToList();
            for (int i = 1; i <= 6; i++)
            {
                var color = colors[UnityEngine.Random.Range(0, colors.Count())];
                yield return new Face(color, (FaceSides)i, i);
            }
        }
    }
}
