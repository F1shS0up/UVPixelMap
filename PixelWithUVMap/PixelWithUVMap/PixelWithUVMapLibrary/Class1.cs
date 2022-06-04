using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PixelWithUVMapLibrary
{
   
    public class UVMap
    {
        //Create color array of the texture
        internal Color[,] colorArray2D;
        //How wide is one texture
        internal int oneTileWidth;
       
        
        public UVMap(Texture2D texture, int oneTileWidth)
        {
            this.oneTileWidth = oneTileWidth;
            //Create new 1d array with size of all pixels in texture
            Color[] colorArray = new Color[oneTileWidth * oneTileWidth];
            //set each color in color array to the texture color
            texture.GetData<Color>(colorArray);
            //make iterations int which will be used later on...
            int iterations = 0;
            //Set color array length which is just a square
            colorArray2D = new Color[oneTileWidth, oneTileWidth];
            for(int y = 0; y < oneTileWidth; y++)
            {
                for(int x = 0; x < oneTileWidth; x++)
                {
                    //Set the color on the x and y cordinates to the color array 
                    colorArray2D[x, y] = colorArray[iterations];
                    //increase iteration
                    iterations++;
                }
            }


        }
    }
    public class Overlay
    {
        //the same as uvmap just for different texture

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
    public class Source
    {
        public Texture2D texture;

        public Texture2D coloredTexture;

        Color[] colorsIntexture;
        public Source(Texture2D texture)
        {
            this.texture = texture;
            colorsIntexture = new Color[texture.Width * texture.Width];
            texture.GetData<Color>(colorsIntexture);
        }
        public void ChangeUVMap(UVMap uvmap, GraphicsDevice graphicsDevice)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            Texture2D ColoredTexture = new Texture2D(graphicsDevice, texture.Width, texture.Width);

            int iterations = 0;
            foreach(Color A in colorsIntexture)
            {
                int x = A.R;
                int y = A.G;

                colors[iterations] = uvmap.colorArray2D[x, y];
                iterations++;
            }
            ColoredTexture.SetData<Color>(colors);

            coloredTexture = ColoredTexture;
        }
       
    }
    public static class Functions
    {
        //Create source from overlay and uvmap
        
        public static Source CreateSource(UVMap map, Overlay overlay, GraphicsDevice graphicsDevice)
        {
            //Throw error if map and overlay doesnt have the same size
            if(map.oneTileWidth != overlay.oneTileWidth)
            {
                throw new ArgumentException("UVMap tileWidth cannot differ from the Overlay tileWidth");
            }

            int xSize = map.oneTileWidth;
            int ySize = map.oneTileWidth;
            //Create texture for the source with the size of the map and overlay texture
            Texture2D source = new Texture2D(graphicsDevice, xSize, ySize);
            //create 1d array with the size of pixels in map and overlay
            Color[] colorArray2D = new Color[xSize * ySize];
            //create iterations int like in UVMap
            int iterations = 0;
            for (int y = 0; y < ySize; y++)
            {
                for(int x = 0; x < xSize; x++)
                {
                    //set the 1d array to the color from uv map in position of overlay
                    colorArray2D[iterations] = GetColorFromUVMap(x, y, map, overlay);
                    //if color is transparent
                    if(colorArray2D[iterations] == new Color(0, 0, 0))
                    {
                        colorArray2D[iterations] = Color.Transparent;
                    }
                    //increase iterations
                    iterations++;
                }
            }
            // set the texture values
            source.SetData<Color>(colorArray2D);
            // return new source
            return new Source(source);
        }
        private static Color GetColorFromUVMap(int xCurrent, int yCurrent, UVMap map, Overlay overlay)
        {
            //create color that is on x and y from the overlay
            Color color = overlay.colorArray2D[xCurrent, yCurrent];
            for(int y = 0; y < map.oneTileWidth; y++)
            {
                for(int x = 0; x < map.oneTileWidth; x++)
                {
                    //if the color from overlay is same as color from map se it
                    if(map.colorArray2D[x, y] == color)
                    {
                        return new Color(x , y , 0);
                    }
                }
            }
            
            return Color.White;
        }
    }
}
