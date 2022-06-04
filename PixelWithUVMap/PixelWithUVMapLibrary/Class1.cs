using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PixelWithUVMapLibrary
{
    public class UVMap
    {
        internal Color[,] colorArray2D;
        internal int oneTileWidth;
        public UVMap(Texture2D texture, int oneTileWidth)
        {
            this.oneTileWidth = oneTileWidth;

            Color[] colorArray = new Color[oneTileWidth * oneTileWidth - 1];
            texture.GetData<Color>(colorArray);
            int iterations = 0;

            colorArray2D = new  Color[oneTileWidth - 1, oneTileWidth - 1];
            for(int y = 0; y < oneTileWidth; y++)
            {
                for(int x = 0; x < oneTileWidth; x++)
                {
                    colorArray2D[x, y] = colorArray[iterations];
                    iterations++;
                }
            }


        }
    }
    public class Overlay
    {
        internal Color[,] colorArray2D;
        internal int oneTileWidth;
        public Overlay(Texture2D texture, int oneTileWidth)
        {
            this.oneTileWidth = oneTileWidth;

            Color[] colorArray = new Color[oneTileWidth * oneTileWidth];
            texture.GetData<Color>(colorArray);
            int iterations = 0;

            colorArray2D = new Color[oneTileWidth, oneTileWidth];
            for (int y = 0; y < oneTileWidth; y++)
            {
                for (int x = 0; x < oneTileWidth; x++)
                {
                    
                    colorArray2D[x, y] = colorArray[iterations];
                    iterations++;
                }
            }
        }
    }
    public static class DontKniwName
    {
        public static Texture2D CreateSource(UVMap map, Overlay overlay, GraphicsDevice graphicsDevice)
        {

            if(map.oneTileWidth != overlay.oneTileWidth)
            {
                throw new ArgumentException("UVMap tileWidth cannot differ from the Overlay tileWidth");
            }
            int xSize = map.oneTileWidth;
            int ySize = map.oneTileWidth;

            Texture2D source = new Texture2D(graphicsDevice, xSize, ySize);

            Color[] colorArray2D = new Color[xSize * ySize];
            int iterations = 0;
            for (int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    
                    colorArray2D[iterations] = GetColorFromUVMap(x, y, map, overlay);
                    iterations++;
                }
            }

            source.SetData<Color>(colorArray2D);

            return source;
        }
        private static Color GetColorFromUVMap(int xCurrent, int yCurrent, UVMap map, Overlay overlay)
        {
            Color color = overlay.colorArray2D[xCurrent, yCurrent];
            for(int y = 0; y < map.oneTileWidth; y++)
            {
                for(int x = 0; x < map.oneTileWidth; x++)
                {
                    if(map.colorArray2D[x, y] == color)
                    {
                        return new Color(x, y, 0);
                    }
                }
            }
            
            return Color.White;
        }
    }
}
