using System.Collections.Generic;

namespace DemoApplication
{
    internal class Polygon
    {
        public List<Mathematics.Vector3D> vertices;
        public List<int[]> verticeGroups;

        public List<Mathematics.Vector3D> modifiedVertices;

        public List<Mathematics.Vector3D> normals;
        public List<int[]> normalGroups;

        public List<Mathematics.Vector3D> modifiedNormals;

        public void Reset()
        {
            Clear();

            modifiedVertices.AddRange(vertices);
            modifiedNormals.AddRange(normals);
        }

        public void Clear()
        {
            modifiedVertices.Clear();
            modifiedNormals.Clear();
        }
    }
}
