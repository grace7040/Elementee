using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FreeDraw
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]  // REQUIRES A COLLIDER2D to function

    public class Drawable : MonoBehaviour
    {
        public static Color PenColor;
        public static int PenWidth = 6;   //radius


        public delegate void BrushFunction(Vector2 worldPosition);
        // This is the function called when a left click happens
        // Pass in your own custom one to change the brush type
        // Set the default function in the Awake method
        public BrushFunction CurrentBrush;

        

        public bool ResetCanvasOnPlay = true;


        // The colour the canvas is reset to each time
        Color _resetColor = new Color(0, 0, 0, 0);  // By default, reset the canvas to be transparent
        int _drawingLayer = 1 << (int)Layer.DrawObject;
        // Used to reference THIS specific file without making all methods static
        //public static Drawable drawable;


        // MUST HAVE READ/WRITE enabled set in the file editor of Unity
        Sprite _drawableSprite;
        Texture2D _drawableTexture;

        Vector2 _previousDragPosition;
        Color[] _cleanColorsArray;
        Color _transparent;
        Color[] _curColors;
        bool _mouseWasPreviouslyHeldDown = false;
        bool _noDrawingOnCurrentDrag = false;
        bool _hasDrawn;

        public bool HasDrawn { get { return _hasDrawn; } }

        private void OnEnable()
        {
            _hasDrawn = false;
        }



        //////////////////////////////////////////////////////////////////////////////
        // BRUSH TYPES. Implement your own here


        // When you want to make your own type of brush effects,
        // Copy, paste and rename this function.
        // Go through each step
        public void BrushTemplate(Vector2 world_position)
        {
            // 1. Change world position to pixel coordinates
            var pixelPos = WorldToPixelCoordinates(world_position);

            // 2. Make sure our variable for pixel array is updated in this frame
            _curColors = _drawableTexture.GetPixels();

            ////////////////////////////////////////////////////////////////
            // FILL IN CODE BELOW HERE

            // Do we care about the user left clicking and dragging?
            // If you don't, simply set the below if statement to be:
            //if (true)

            // If you do care about dragging, use the below if/else structure
            if (_previousDragPosition == Vector2.zero)
            {
                // THIS IS THE FIRST CLICK
                // FILL IN WHATEVER YOU WANT TO DO HERE
                // Maybe mark multiple pixels to colour?
                MarkPixelsToColour(pixelPos, PenWidth, PenColor);
            }
            else
            {
                // THE USER IS DRAGGING
                // Should we do stuff between the previous mouse position and the current one?
                ColourBetween(_previousDragPosition, pixelPos, PenWidth, PenColor);
            }
            ////////////////////////////////////////////////////////////////

            // 3. Actually apply the changes we marked earlier
            // Done here to be more efficient
            ApplyMarkedPixelChanges();
            
            // 4. If dragging, update where we were previously
            _previousDragPosition = pixelPos;
        }



        
        // Default brush type. Has width and colour.
        // Pass in a point in WORLD coordinates
        // Changes the surrounding pixels of the world_point to the static pen_colour
        public void PenBrush(Vector2 worldPoint)
        {
            var pixelPos = WorldToPixelCoordinates(worldPoint);

            _curColors = _drawableTexture.GetPixels();

            if (_previousDragPosition == Vector2.zero)
            {
                // If this is the first time we've ever dragged on this image, simply colour the pixels at our mouse position
                MarkPixelsToColour(pixelPos, PenWidth, PenColor);
            }
            else
            {
                // Colour in a line from where we were on the last update call
                ColourBetween(_previousDragPosition, pixelPos, PenWidth, PenColor);
            }
            ApplyMarkedPixelChanges();

            //Debug.Log("Dimensions: " + pixelWidth + "," + pixelHeight + ". Units to pixels: " + unitsToPixels + ". Pixel pos: " + pixel_pos);
            _previousDragPosition = pixelPos;
        }


        // Helper method used by UI to set what brush the user wants
        // Create a new one for any new brushes you implement
        public void SetPenBrush()
        {
            // PenBrush is the NAME of the method we want to set as our current brush
            CurrentBrush = PenBrush;
        }
//////////////////////////////////////////////////////////////////////////////






        // This is where the magic happens.
        // Detects when user is left clicking, which then call the appropriate function
        void Update()
        {
            // Is the user holding down the left mouse button?
            var mouseHeldDown = Input.GetMouseButton(0);
            if (mouseHeldDown && !_noDrawingOnCurrentDrag)
            {
                // Convert mouse coordinates to world coordinates
                var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Check if the current mouse position overlaps our image
                var hit = Physics2D.OverlapPoint(mouseWorldPosition, _drawingLayer);
                if (hit != null && hit.transform != null)
                {
                    // We're over the texture we're drawing on!
                    // Use whatever function the current brush is
                    _hasDrawn = true;
                    CurrentBrush(mouseWorldPosition);
                }

                else
                {
                    // We're not over our destination texture
                    _previousDragPosition = Vector2.zero;
                    if (!_mouseWasPreviouslyHeldDown)
                    {
                        // This is a new drag where the user is left clicking off the canvas
                        // Ensure no drawing happens until a new drag is started
                        _noDrawingOnCurrentDrag = true;
                    }
                }
            }
            // Mouse is released
            else if (!mouseHeldDown)
            {
                _previousDragPosition = Vector2.zero;
                _noDrawingOnCurrentDrag = false;
            }
            _mouseWasPreviouslyHeldDown = mouseHeldDown;
        }



        // Set the colour of pixels in a straight line from start_point all the way to end_point, to ensure everything inbetween is coloured
        public void ColourBetween(Vector2 startPoint, Vector2 endPoint, int width, Color color)
        {
            // Get the distance from start to finish
            var distance = Vector2.Distance(startPoint, endPoint);
            var direction = (startPoint - endPoint).normalized;

            var curPosition = startPoint;

            // Calculate how many times we should interpolate between start_point and end_point based on the amount of time that has passed since the last update
            float lerpSteps = 1 / distance;

            for (float lerp = 0; lerp <= 1; lerp += lerpSteps)
            {
                curPosition = Vector2.Lerp(startPoint, endPoint, lerp);
                MarkPixelsToColour(curPosition, width, color);
            }
        }





        public void MarkPixelsToColour(Vector2 centerPixel, int penThickness, Color colorOfPan)
        {
            // Figure out how many pixels we need to colour in each direction (x and y)
            var centerX = (int)centerPixel.x;
            var centerY = (int)centerPixel.y;
            //int extra_radius = Mathf.Min(0, pen_thickness - 2);

            for (int x = centerX - penThickness; x <= centerX + penThickness; x++)
            {
                // Check if the X wraps around the image, so we don't draw pixels on the other side of the image
                if (x >= (int)_drawableSprite.rect.width || x < 0)
                    continue;

                for (int y = centerY - penThickness; y <= centerY + penThickness; y++)
                {
                    MarkPixelToChange(x, y, colorOfPan);
                }
            }
        }
        public void MarkPixelToChange(int x, int y, Color color)
        {
            // Need to transform x and y coordinates to flat coordinates of array
            var arrayPos = y * (int)_drawableSprite.rect.width + x;

            // Check if this is a valid position
            if (arrayPos > _curColors.Length || arrayPos < 0)
                return;

            _curColors[arrayPos] = color;
        }
        public void ApplyMarkedPixelChanges()
        {
            _drawableTexture.SetPixels(_curColors);
            _drawableTexture.Apply();
        }


        // Directly colours pixels. This method is slower than using MarkPixelsToColour then using ApplyMarkedPixelChanges
        // SetPixels32 is far faster than SetPixel
        // Colours both the center pixel, and a number of pixels around the center pixel based on pen_thickness (pen radius)
        public void ColourPixels(Vector2 centerPixel, int penThickness, Color colorOfPan)
        {
            // Figure out how many pixels we need to colour in each direction (x and y)
            var centerX = (int)centerPixel.x;
            var centerY = (int)centerPixel.y;
            //int extra_radius = Mathf.Min(0, pen_thickness - 2);

            for (int x = centerX - penThickness; x <= centerX + penThickness; x++)
            {
                for (int y = centerY - penThickness; y <= centerY + penThickness; y++)
                {
                    _drawableTexture.SetPixel(x, y, colorOfPan);
                }
            }

            _drawableTexture.Apply();
        }


        public Vector2 WorldToPixelCoordinates(Vector2 world_position)
        {
            // Change coordinates to local coordinates of this image
            var localPos = transform.InverseTransformPoint(world_position);

            // Change these to coordinates of pixels
            var pixelWidth = _drawableSprite.rect.width;
            var pixelHeight = _drawableSprite.rect.height;
            var unitsToPixels = pixelWidth / _drawableSprite.bounds.size.x * transform.localScale.x;

            // Need to center our coordinates
            var centeredX = localPos.x * unitsToPixels + pixelWidth / 2;
            var centeredY = localPos.y * unitsToPixels + pixelHeight / 2;

            // Round current mouse position to nearest pixel
            var pixelPos = new Vector2(Mathf.RoundToInt(centeredX), Mathf.RoundToInt(centeredY));

            return pixelPos;
        }


        // Changes every pixel to be the reset colour
        public void ResetCanvas()
        {
            _drawableTexture.SetPixels(_cleanColorsArray);
            _drawableTexture.Apply();
            _hasDrawn = false;
        }


        
        void Awake()
        {
            // DEFAULT BRUSH SET HERE
            CurrentBrush = PenBrush;

            _drawableSprite = this.GetComponent<SpriteRenderer>().sprite;
            
            _drawableTexture = _drawableSprite.texture;

            // Initialize clean pixels to use
            _cleanColorsArray = new Color[(int)_drawableSprite.rect.width * (int)_drawableSprite.rect.height];
            for (int x = 0; x < _cleanColorsArray.Length; x++)
                _cleanColorsArray[x] = _resetColor;

            // Should we reset our canvas image when we hit play in the editor?
            if (ResetCanvasOnPlay)
                ResetCanvas();
        }

        public void UpdateCanvas()
        {
            _drawableSprite = this.GetComponent<SpriteRenderer>().sprite;
            _drawableTexture = _drawableSprite.texture;
            _drawableTexture.Apply();
        }
    }
}