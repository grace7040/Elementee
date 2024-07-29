using UnityEngine;


namespace FreeDraw
{
    // Helper methods used to set drawing settings
    public class DrawingSettings : MonoBehaviour
    {
        float _transparency = 1f;

        // Changing pen settings is easy as changing the static properties Drawable.PenColour and Drawable.PenWidth
        public void SetMarkerColour(Color newColor)
        {
            Drawable.PenColor = newColor;
        }
        // new_width is radius in pixels
        public void SetMarkerWidth(int newWidth)
        {
            Drawable.PenWidth = newWidth;
        }
        public void SetTransparency(float amount)
        {
            _transparency = amount;
            Color c = Drawable.PenColor;
            c.a = amount;
            Drawable.PenColor = c;
        }

        public void SetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 0f));
        }

        public void PartialSetEraser()
        {
            SetMarkerColour(new Color(255f, 255f, 255f, 0.5f));
        }
    }
}
