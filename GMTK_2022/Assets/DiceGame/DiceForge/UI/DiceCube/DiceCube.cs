using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class DiceCube : MonoBehaviour
    {
        [SerializeField] Renderer frontFace;
        [SerializeField] Renderer topFace;
        [SerializeField] Renderer rightFace;
        [SerializeField] Renderer leftFace;
        [SerializeField] Renderer bottomFace;
        [SerializeField] Renderer backFace;

        [SerializeField] Material baseMaterial;

        public void InitDice(Dice dice)
        {
            SetUIFace(frontFace, dice.GetFace(FaceSides.Front));
            SetUIFace(topFace, dice.GetFace(FaceSides.Top));
            SetUIFace(rightFace, dice.GetFace(FaceSides.Right));
            SetUIFace(leftFace, dice.GetFace(FaceSides.Left));
            SetUIFace(bottomFace, dice.GetFace(FaceSides.Bottom));
            SetUIFace(backFace, dice.GetFace(FaceSides.Back));
        }

        private void SetUIFace(Renderer renderer, Face face)
        {
            var mat = new Material(baseMaterial);
            var sprite = Sprites.Instance.Get(face.SpriteName);
            mat.mainTexture = sprite.texture;
            renderer.material = mat;
        }
    }
}
