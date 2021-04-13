﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CatLike.HexMap
{
    public class HexMapEditor : MonoBehaviour
    {
        public Color[] ColorArray = new Color[0];
        public HexGrid HexGrid;

        private Color activeColor;

        private void Awake()
        {
            SelectColor(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleInput();
            }
        }
        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out RaycastHit hit))
            {
                HexGrid.ColorCell(hit.point, activeColor);
            }
        }
        public void SelectColor(int index)
        {
            activeColor = ColorArray[index];
        }
    }
}