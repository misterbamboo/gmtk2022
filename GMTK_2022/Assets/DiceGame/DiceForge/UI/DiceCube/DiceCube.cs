using System;
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

        private Dice dice;

        public void InitDice(Dice dice)
        {
            this.dice = dice;
            UpdateUIDiceFaces();
        }

        public void ReplaceFace(FaceSides targetFaceSide, Face face)
        {
            dice.SetFace(targetFaceSide, face);
            UpdateUIDiceFaces();
        }

        public Face GetDiceFace(FaceSides faceSide)
        {
            return dice.GetFace(faceSide);
        }

        public FaceSides GetDisplayDiceFaceSide()
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0, -5), Vector3.forward, out RaycastHit hitInfo, 10f, LayerMask.GetMask("UIDiceCube")))
            {
                return GetFaceSide(hitInfo.collider.gameObject);
            }
            return FaceSides.Front;
        }

        private void UpdateUIDiceFaces()
        {
            SetUIFace(frontFace, dice.GetFace(FaceSides.Front));
            SetUIFace(topFace, dice.GetFace(FaceSides.Top));
            SetUIFace(rightFace, dice.GetFace(FaceSides.Right));
            SetUIFace(leftFace, dice.GetFace(FaceSides.Left));
            SetUIFace(bottomFace, dice.GetFace(FaceSides.Bottom));
            SetUIFace(backFace, dice.GetFace(FaceSides.Back));
        }

        private FaceSides GetFaceSide(GameObject selectedFace)
        {
            return
                selectedFace == frontFace.gameObject ? FaceSides.Front :
                selectedFace == topFace.gameObject ? FaceSides.Top :
                selectedFace == rightFace.gameObject ? FaceSides.Right :
                selectedFace == leftFace.gameObject ? FaceSides.Left :
                selectedFace == bottomFace.gameObject ? FaceSides.Bottom :
                selectedFace == backFace.gameObject ? FaceSides.Back :
                FaceSides.Front;
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
