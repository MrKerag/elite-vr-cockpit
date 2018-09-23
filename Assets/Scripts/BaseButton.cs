﻿using System.Collections.Generic;
using UnityEngine;

namespace EVRC
{
    abstract public class BaseButton : MonoBehaviour, IHighlightable, IActivateable
    {
        public static Color invalidColor = Color.red;
        public Color color;
        public Color highlightColor;
        public bool useHudColorMatrix = true;
        protected IButtonImage buttonImage;
        protected bool highlighted = false;

        virtual protected void OnEnable()
        {
            buttonImage = GetComponent<IButtonImage>();
            if (buttonImage == null)
            {
                Debug.LogWarningFormat("A button image is missing from {0}", name);
            }
            EDStateManager.HudColorMatrixChanged.Listen(OnHudColorMatrixChange);
            Refresh();
        }

        virtual protected void OnDisable()
        {
            EDStateManager.HudColorMatrixChanged.Remove(OnHudColorMatrixChange);
        }

        virtual protected void Update() { }

        private void OnHudColorMatrixChange(HudColorMatrix arg0)
        {
            Refresh();
        }

        public void OnHover()
        {
            highlighted = true;
            Refresh();
        }

        public void OnUnhover()
        {
            highlighted = false;
            Refresh();
        }

        /**
         * Transforms colors with the HUD color matrix, if the option is set
         */
        protected Color TransformColor(Color color)
        {
            return EDStateManager.ConditionallyApplyHudColorMatrix(useHudColorMatrix, color);
        }

        virtual protected void Refresh()
        {
            if (!IsValid())
            {
                buttonImage.SetColor(invalidColor);
            }
            else if (highlighted)
            {
                buttonImage.SetColor(TransformColor(highlightColor));
            }
            else
            {
                buttonImage.SetColor(TransformColor(color));
            }
        }

        /**
         * Check that can be overridden to indicate if a button is invalid
         */
        virtual public bool IsValid()
        {
            return true;
        }

        public void Activate(ControllerInteractionPoint interactionPoint)
        {
            Activate();
        }

        abstract public void Activate();
    }
}
