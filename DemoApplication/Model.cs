using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
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

        public static Model ParseJsonFile(string path)
        {
            using var fileStream = new StreamReader(path);

            var modelDocument = JsonDocument.Parse(fileStream.BaseStream);
            var modelRoot = modelDocument.RootElement;

            var vertices = modelRoot.GetProperty("vertices");
            var verticeCount = vertices.GetArrayLength();

            var verticeGroups = modelRoot.GetProperty("verticeGroups");
            var verticeGroupCount = verticeGroups.GetArrayLength();

            var normals = modelRoot.GetProperty("normals");
            var normalCount = normals.GetArrayLength();

            var normalGroups = modelRoot.GetProperty("normalGroups");
            var normalGroupCount = normalGroups.GetArrayLength();

            var model = new Model(verticeCount, verticeGroupCount, normalCount, normalGroupCount);

            for (var i = 0; i < verticeCount; i++)
            {
                var verticeData = vertices[i];
                Debug.Assert(verticeData.GetArrayLength() == 3);

                var vertice = new Vector3(verticeData[0].GetSingle(), verticeData[1].GetSingle(), verticeData[2].GetSingle());
                model.Vertices.Add(vertice);
            }

            for (var i = 0; i < verticeGroupCount; i++)
            {
                var verticeGroup = verticeGroups[i].EnumerateArray().Select(element => element.GetInt32()).ToArray();
                model.VerticeGroups.Add(verticeGroup);
            }

            for (var i = 0; i < normalCount; i++)
            {
                var normalData = normals[i];
                Debug.Assert(normalData.GetArrayLength() == 3);

                var normal = new Vector3(normalData[0].GetSingle(), normalData[1].GetSingle(), normalData[2].GetSingle());
                model.Normals.Add(normal);
            }

            for (var i = 0; i < normalGroupCount; i++)
            {
                var normalGroup = normalGroups[i].EnumerateArray().Select(element => element.GetInt32()).ToArray();
                model.NormalGroups.Add(normalGroup);
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
