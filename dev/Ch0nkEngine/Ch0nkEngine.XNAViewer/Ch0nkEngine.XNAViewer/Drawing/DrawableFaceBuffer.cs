using System;
using System.Collections.Generic;
using System.Linq;
using Ch0nkEngine.Data;
using Ch0nkEngine.Data.Data;
using Ch0nkEngine.Data.Data.Materials.Types;
using Microsoft.Xna.Framework;

namespace Ch0nkEngine.XNAViewer.Drawing
{
    public class DrawableFaceBuffer
    {
        private DrawableFaceCollection[] _cubeCollections;

        public DrawableFaceBuffer(Game game, params Block[] blocks)
            : this(game, blocks.ToList())
        {
        }

        public DrawableFaceBuffer(Game game, IEnumerable<Block> blocks)
        {
            Dictionary<String, List<Block>> materialGroups = new Dictionary<string, List<Block>>();
            foreach (var block in blocks)
            {
                //String materialName = MaterialTranslator.GetMaterialName(block.MaterialType);
                if (block.Material is AirMaterial)
                    continue;

                if (!materialGroups.ContainsKey(block.Material.MaterialName))
                    materialGroups.Add(block.Material.MaterialName, new List<Block>());
                materialGroups[block.Material.MaterialName].Add(block);
            }

            List<DrawableFaceCollection> cubeCollections = new List<DrawableFaceCollection>();
            foreach (KeyValuePair<string, List<Block>> materialGroup in materialGroups)
            {
                cubeCollections.Add(new DrawableFaceCollection(game, materialGroup.Key, materialGroup.Value));
            }

            _cubeCollections = cubeCollections.ToArray();
        }

        public void Draw(GameTime gameTime, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, Vector3 cameraPosition, Vector3 cameraDirection)
        {
            foreach (DrawableFaceCollection drawableCubeCollection in _cubeCollections)
            {
                drawableCubeCollection.Draw(gameTime, worldMatrix, viewMatrix, projectionMatrix, cameraPosition, cameraDirection);
            }
        }
    }
}
