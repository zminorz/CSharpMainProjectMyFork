using UnityEngine;

namespace Utilities
{
    public static class MapGenerator
    {
        public static bool[,] Generate(int width, int height, float density, int randomSeed)
        {
            var result = new bool[width, height];

            var blockWidth = width / 3;
            var blockHeight = height / 3;
            var blockMap = new bool[blockWidth, blockHeight];
            var filledCount = blockWidth * blockHeight * Mathf.Clamp01(density);
            
            Random.InitState(randomSeed);
            for (var i = 0; i < filledCount; i++)
            {
                var x = Random.Range(0, blockWidth);
                var y = Random.Range(0, blockHeight);
                blockMap[x, y] = true;
            }

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    result[x, y] = blockMap[x / 3, y / 3];
                }
            }

            int CountNeighbours(int x, int y)
            {
                var count = 0;
                if (x > 0 && result[x - 1, y]) count++;
                if (x < width - 1 && result[x + 1, y]) count++;
                if (y > 0 && result[x, y - 1]) count++;
                if (y < height - 1 && result[x, y + 1]) count++;

                return count;
            }

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var count = CountNeighbours(x, y);
                    if (count < 4 && result[x, y])
                    {
                        result[x, y] = Random.Range(0, 2) > 0;
                    }
                }
            }

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var count = CountNeighbours(x, y);
                    if (count < 2 && result[x, y])
                    {
                        result[x, y] = false;
                    }
                }
            }

            for (var x = 0; x < width; x++)
            {
                result[x, 0] = true;
                result[x, 1] = true;
                result[x, 2] = true;
                result[x, 3] = true;
                result[x, height - 4] = true;
                result[x, height - 3] = true;
                result[x, height - 2] = true;
                result[x, height - 1] = true;
            }
            
            for (var y = 0; y < height; y++)
            {
                result[0, y] = true;
                result[1, y] = true;
                result[2, y] = true;
                result[3, y] = true;
                result[width - 4, y] = true;
                result[width - 3, y] = true;
                result[width - 2, y] = true;
                result[width - 1, y] = true;
            }
            
            return result;
        }
    }
}