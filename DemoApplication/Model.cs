using System;
using System.Collections.Generic;
using System.IO;
using Mathematics;

namespace DemoApplication
{
    public class Model
    {
        public readonly List<Vector3> Vertices;
        public readonly List<int[]> VerticeGroups;

        public readonly List<Vector3> ModifiedVertices;

        public readonly List<Vector3> Normals;
        public readonly List<int[]> NormalGroups;

        public readonly List<Vector3> ModifiedNormals;

        public static Model ParseXFile(string name)
        {
            var basePath = Path.Combine(Environment.CurrentDirectory, "models", name);

            var verticeReader = new StreamReader(Path.Combine(basePath, "vertices.txt"));
            var verticeGroupReader = new StreamReader(Path.Combine(basePath, "verticeGroups.txt"));
            var normalReader = new StreamReader(Path.Combine(basePath, "normals.txt"));
            var normalGroupReader = new StreamReader(Path.Combine(basePath, "normalGroups.txt"));

            var verticeCount = int.Parse(verticeReader.ReadLine());
            var verticeGroupCount = int.Parse(verticeGroupReader.ReadLine());
            var normalCount = int.Parse(normalReader.ReadLine());
            var normalGroupCount = int.Parse(normalGroupReader.ReadLine());

            var model = new Model(verticeCount, verticeGroupCount, normalCount, normalGroupCount);

            for (var i = 0; i < verticeCount; i++)
            {
                var parts = verticeReader.ReadLine().Split(';');
                model.Vertices.Add(new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2])));
            }

            var group = new List<int>(4);

            for (var i = 0; i < verticeGroupCount; i++)
            {
                var parts = verticeGroupReader.ReadLine().Split(';');
                var groupCount = int.Parse(parts[0]);
                var subParts = parts[1].Split(',');

                for (var n = 0; n < groupCount; n++)
                {
                    group.Add(int.Parse(subParts[n]));
                }

                model.VerticeGroups.Add(group.ToArray());
                group.Clear();
            }

            for (var i = 0; i < normalCount; i++)
            {
                var parts = normalReader.ReadLine().Split(';');
                model.Normals.Add(new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2])));
            }

            for (var i = 0; i < normalGroupCount; i++)
            {
                var parts = normalGroupReader.ReadLine().Split(';');
                var groupCount = int.Parse(parts[0]);
                var subParts = parts[1].Split(',');

                for (var n = 0; n < groupCount; n++)
                {
                    group.Add(int.Parse(subParts[n]));
                }

                model.NormalGroups.Add(group.ToArray());
                group.Clear();
            }

            return model;
        }

        public Model(int verticeCount, int verticeGroupCount, int normalCount, int normalGroupCount)
        {
            Vertices = new List<Vector3>(verticeCount);
            VerticeGroups = new List<int[]>(verticeGroupCount);
            ModifiedVertices = new List<Vector3>(verticeCount);

            Normals = new List<Vector3>(normalCount);
            NormalGroups = new List<int[]>(normalGroupCount);
            ModifiedNormals = new List<Vector3>(normalCount);
        }

        public void Clear()
        {
            ModifiedVertices.Clear();
            ModifiedNormals.Clear();
        }

        public void Reset()
        {
            Clear();

            ModifiedVertices.AddRange(Vertices);
            ModifiedNormals.AddRange(Normals);
        }
    }
}
