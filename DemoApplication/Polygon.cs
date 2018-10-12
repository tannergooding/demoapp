using System;
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
            this.Clear();

            this.modifiedVertices.AddRange(this.vertices);
            this.modifiedNormals.AddRange(this.normals);
        }

        public void Clear()
        {
            this.modifiedVertices.Clear();
            this.modifiedNormals.Clear();
        }
    }
}
