using Godot;

//This is broken and i don't know way.

namespace Trinketos.HaciendaSimulator
{
    public static class TerrainPainter
    {
        public enum BrushType { Raise, Lower, Smooth }

        static public void UpdateTerrainHeightmap(MeshInstance3D terrainMesh, HeightMapShape3D heightMapShape3D, Texture2D heightMapTexture, Vector3 mousePosition, int brushIntensity, int brushSize, BrushType brushType)
        {
            ShaderMaterial material = terrainMesh.MaterialOverride as ShaderMaterial;
            Texture2D texture = Brush(terrainMesh, heightMapTexture.GetImage(), mousePosition, brushIntensity, brushSize, brushType);
            material.SetShaderParameter("heightmap", texture);
            heightMapShape3D.UpdateMapDataFromImage(texture.GetImage(), 0, 60);
        }

        public static void UpdateTerrainSplatmap(MeshInstance3D terrainMesh, Texture2D splatMapTexture, Vector3 mousePosition, Color color, int brushSize, BrushType brushType)
        {
            ShaderMaterial material = terrainMesh.MaterialOverride as ShaderMaterial;
            material.SetShaderParameter("heightmap", ApplySplatmapBrush(terrainMesh, splatMapTexture.GetImage(), mousePosition, brushSize, color));

        }


        static public Color GetPixelData(Vector3 position, Image image)
        {

            int width = image.GetWidth();
            int height = image.GetHeight();

            int x = (int)(position.X * width);
            int z = (int)(position.Y * height);

            Color pixelColor = image.GetPixel(x, z);
            return pixelColor;
        }

        static public Texture2D Brush(MeshInstance3D terrainMesh, Image image, Vector3 position, int intensity, float brushSize, BrushType brushType)
        {
            switch (brushType)
            {
                case BrushType.Raise:
                    return ApplyRaiseEffect(terrainMesh, image, position, intensity, brushSize);
                case BrushType.Lower:
                    return ApplyLowerEffect(terrainMesh, image, position, intensity, brushSize);
                case BrushType.Smooth:
                    return ApplySmoothEffect(terrainMesh, image, position, brushSize);
                default:
                    return null;

            }
        }

        static private Texture2D ApplyRaiseEffect(MeshInstance3D terrainMesh, Image image, Vector3 hitPosition, float intensity, float brushSize)
        {
            int width = image.GetWidth();
            int height = image.GetHeight();

            image.Convert(Image.Format.Rf);

            int centerX = Mathf.Clamp((int)(hitPosition.X / terrainMesh.Scale.X * width), 0, width - 1);
            int centerZ = Mathf.Clamp((int)(hitPosition.Z / terrainMesh.Scale.Z * height), 0, height - 1);

            for (float x = -brushSize; x <= brushSize; x++)
            {
                for (float z = -brushSize; z <= brushSize; z++)
                {
                    float distance = Mathf.Sqrt(x * x + z * z);
                    float effect = Mathf.Max(0, 1 - (distance / brushSize));

                    int pixelX = (int)Mathf.Clamp((float)centerX + x, 0.0f, (float)width - 1.0f);
                    int pixelZ = (int)Mathf.Clamp((float)centerZ + z, 0.0f, (float)height - 1.0f);

                    Color pixelColor = image.GetPixel(pixelX, pixelZ);
                    float newHeight = GetRealHeight(terrainMesh, new Vector3(pixelX, 0, pixelZ), image);
                    newHeight = Mathf.Clamp(pixelColor.R + intensity * effect, 0, 1);
                    image.SetPixel(pixelX, pixelZ, new Color(newHeight, newHeight, newHeight));

                }
            }
            return ImageTexture.CreateFromImage(image);
        }

        static private Texture2D ApplyLowerEffect(MeshInstance3D terrainMesh, Image image, Vector3 hitPosition, float intensity, float brushSize)
        {
            return ApplyRaiseEffect(terrainMesh, image, hitPosition, -intensity, brushSize); // Mismo código pero con intensidad negativa
        }

        static private Texture2D ApplySmoothEffect(MeshInstance3D terrainMesh, Image image, Vector3 hitPosition, float brushSize)
        {

            int width = image.GetWidth();
            int height = image.GetHeight();

            image.Convert(Image.Format.Rf);

            int centerX = Mathf.Clamp((int)(hitPosition.X / terrainMesh.Scale.X * width), 0, width - 1);
            int centerZ = Mathf.Clamp((int)(hitPosition.Z / terrainMesh.Scale.Z * height), 0, height - 1);

            float avgHeight = GetAverageHeight(centerX,centerZ,(int)brushSize,image);

            for (int x = -(int)brushSize; x <= brushSize; x++)
            {
                for (int z = -(int)brushSize; z <= brushSize; z++)
                {
                    int pixelX = Mathf.Clamp(centerX + x, 0, width - 1);
                    int pixelZ = Mathf.Clamp(centerZ + z, 0, height - 1);

                    
                    image.SetPixel(pixelX, pixelZ, new Color(avgHeight, avgHeight, avgHeight));
                }
            }
            return ImageTexture.CreateFromImage(image);
        }

        static public Texture2D ApplySplatmapBrush(MeshInstance3D terrainMesh, Image image, Vector3 hitPosition, float brushSize, Color paintColor)
        {

            int width = image.GetWidth();
            int height = image.GetHeight();

            image.Convert(Image.Format.Rf);

            int centerX = Mathf.Clamp((int)(hitPosition.X / terrainMesh.Scale.X * width), 0, width - 1);
            int centerZ = Mathf.Clamp((int)(hitPosition.Z / terrainMesh.Scale.Z * height), 0, height - 1);

            for (int x = -(int)brushSize; x <= (int)brushSize; x++)
            {
                for (int z = -(int)brushSize; z <= (int)brushSize; z++)
                {
                    int pixelX = Mathf.Clamp(centerX + x, 0, width - 1);
                    int pixelZ = Mathf.Clamp(centerZ + z, 0, height - 1);

                    float distance = Mathf.Sqrt(x * x + z * z);
                    float effect = Mathf.Max(0, 1 - (distance / brushSize)); // Gradiente suave

                    Color pixelColor = image.GetPixel(pixelX, pixelZ);
                    Color newColor = pixelColor.Lerp(paintColor, effect); // Mezcla progresiva del material
                    image.SetPixel(pixelX, pixelZ, newColor);
                }
            }

            return ImageTexture.CreateFromImage(image);
        }

        static private float GetRealHeight(MeshInstance3D terrainMesh, Vector3 hitPosition, Image image)
        {
            int width = image.GetWidth();
            int height = image.GetHeight();

            int x = Mathf.Clamp((int)(hitPosition.X / terrainMesh.Scale.X * width), 0, width - 1);
            int z = Mathf.Clamp((int)(hitPosition.Z / terrainMesh.Scale.Z * width), 0, width - 1);

            float heightValue = image.GetPixel(x, z).R * 60;
            return heightValue;
        }

        static private float GetAverageHeight(int x, int z, int radius, Image image)
        {
            int width = image.GetWidth();
            int height = image.GetHeight();

            float totalHeight = 0;
            int count = 0;

            for (int offsetX = -radius; offsetX <= radius; offsetX++)
            {
                for (int offsetZ = -radius; offsetZ <= radius; offsetZ++)
                {
                    int pixelX = Mathf.Clamp(x + offsetX, 0, width - 1);
                    int pixelZ = Mathf.Clamp(z + offsetZ, 0, height - 1);
                    totalHeight += image.GetPixel(pixelX, pixelZ).R;
                    count++;
                }
            }
            return count > 0 ? totalHeight / count : 0;
        }
    }
}
