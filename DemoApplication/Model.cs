using System.Collections.Generic;

namespace DemoApplication
{
    public class Model
    {
        public List<Mathematics.Vector3> vertices;
        public List<int[]> verticeGroups;

        public List<Mathematics.Vector3> modifiedVertices;

        public List<Mathematics.Vector3> normals;
        public List<int[]> normalGroups;

        public List<Mathematics.Vector3> modifiedNormals;

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
