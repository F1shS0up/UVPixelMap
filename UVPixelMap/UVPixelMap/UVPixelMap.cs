using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace UVPixelMap
{

    public class UVMap
    {
        //Create color array of the texture
        internal Color[,] colorArray2D;
        //How wide is one texture
        internal int textureWidth;


        public UVMap(Texture2D texture, int textureWidth)
        {
            this.textureWidth = textureWidth;
            //Create new 1d array with size of all pixels in texture
            Color[] colorArray = new Color[textureWidth * textureWidth];
            //set each color in color array to the texture color
            texture.GetData<Color>(colorArray);
            //make iterations int which will be used later on...
            int iterations = 0;
            //Set color array length which is just a square
            colorArray2D = new Color[textureWidth, textureWidth];
            for (int y = 0; y < textureWidth; y++)
            {
                for (int x = 0; x < textureWidth; x++)
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
        internal int textureWidth;
        internal int textureHeight;
        public Overlay(Texture2D texture, int textureWidth, int textureHeight)
        {
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            Color[] colorArray = new Color[textureWidth * textureHeight];
            texture.GetData<Color>(colorArray);
            int iterations = 0;

            colorArray2D = new Color[textureWidth, textureHeight];
            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {

                    colorArray2D[x, y] = colorArray[iterations];
                    iterations++;
                }
            }
        }
    }
    public class Source
    {
        //TextureWithNoColor
        private Texture2D texture;

        public Texture2D coloredTexture;

        private Color[] colorsIntexture;
        public Source(Texture2D texture)
        {
            this.texture = texture;
            colorsIntexture = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorsIntexture);
            coloredTexture = texture;
        }
        public void ChangeUVMap(UVMap uvmap, GraphicsDevice graphicsDevice)
        {
            //create new 1d array which is going to be the new generated texture
            Color[] colors = new Color[texture.Width * texture.Height];
            //new colored texture with no colors
            Texture2D ColoredTexture = new Texture2D(graphicsDevice, texture.Width, texture.Height);

            int iterations = 0;
            foreach (Color A in colorsIntexture)
            {
                //get the position from source
                int x = A.R;
                int y = A.G;

                //set the color array the new color on that position from new uvmap
                colors[iterations] = uvmap.colorArray2D[x, y];
                iterations++;
            }
            //create texture from array which holds the new colors
            ColoredTexture.SetData<Color>(colors);
            //set it
            coloredTexture = ColoredTexture;
        }

    }
    public static class Functions
    {
        //Create source from overlay and uvmap

        public static Texture2D texture;

        private static Texture2D[] Split(Texture2D original, int partWidth, int partHeight, out int xCount, out int yCount)
        {
            yCount = original.Height / partHeight;//The number of textures in each horizontal row
            xCount = original.Width / partWidth;//The number of textures in each vertical column
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }
        public static UVMap[] Decrypt(Texture2D original, int partWidth, int partHeight)
        {
            int xCount, yCount;
            Texture2D[] splittedTextures = Split(original, partWidth, partHeight, out xCount, out yCount);
            Debug.WriteLine(xCount);
            UVMap[] r = new UVMap[xCount];
            for (int i = 0; i < xCount; i++)
            {
                r[i] = new UVMap(splittedTextures[i], partWidth);
            }
            return r;
        }
        public static Source CreateSource(UVMap map, Overlay overlay, GraphicsDevice graphicsDevice)
        {
            //Throw error if map and overlay doesnt have the same size


            int xSize = overlay.textureWidth;
            int ySize = overlay.textureHeight;
            //Create texture for the source with the size of the map and overlay texture
            Texture2D source = new Texture2D(graphicsDevice, xSize, ySize);
            //create 1d array with the size of pixels in map and overlay
            Color[] colorArray2D = new Color[xSize * ySize];
            //create iterations int like in UVMap
            int iterations = 0;
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    //set the 1d array to the color from uv map in position of overlay
                    colorArray2D[iterations] = GetColorFromUVMap(x, y, map, overlay);
                    //if color is transparent
                    if (colorArray2D[iterations] == new Color(0, 0, 0))
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
            for (int y = 0; y < map.textureWidth; y++)
            {
                for (int x = 0; x < map.textureWidth; x++)
                {
                    //if the color from overlay is same as color from map se it
                    if (map.colorArray2D[x, y] == color)
                    {
                        return new Color(x, y, 0);
                    }
                }
            }

            return Color.White;
        }
    }
}
