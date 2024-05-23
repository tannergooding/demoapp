// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Mathematics;

namespace BitmapRendering;

public class Model(int verticeCount, int verticeGroupCount, int normalCount, int normalGroupCount)
{
    public readonly List<Vector3> Vertices = new List<Vector3>(verticeCount);
    public readonly List<int[]> VerticeGroups = new List<int[]>(verticeGroupCount);

    public readonly List<Vector3> ModifiedVertices = new List<Vector3>(verticeCount);

    public readonly List<Vector3> Normals = new List<Vector3>(normalCount);
    public readonly List<int[]> NormalGroups = new List<int[]>(normalGroupCount);

    public readonly List<Vector3> ModifiedNormals = new List<Vector3>(normalCount);

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

        var scale = 1.0f;

        if (modelRoot.TryGetProperty("scale", out var scaleProperty))
        {
            scale = scaleProperty.GetSingle();
        }

        var rotation = Quaternion.Identity;

        if (modelRoot.TryGetProperty("rotation", out var rotationProperty))
        {
            rotation = Quaternion.CreateFrom(
                float.DegreesToRadians(rotationProperty[0].GetSingle()),
                float.DegreesToRadians(rotationProperty[1].GetSingle()),
                float.DegreesToRadians(rotationProperty[2].GetSingle())
            );
        }

        var translation = Vector3.Zero;

        if (modelRoot.TryGetProperty("translation", out var translationProperty))
        {
            translation = new Vector3(
                translationProperty[0].GetSingle(),
                translationProperty[1].GetSingle(),
                translationProperty[2].GetSingle()
            );
        }

        var transform = new OrthogonalTransform(rotation, translation);
        var transformMatrix = Matrix4x4.CreateFrom(transform);

        var model = new Model(verticeCount, verticeGroupCount, normalCount, normalGroupCount);

        foreach (var verticeData in vertices.EnumerateArray())
        {
            Debug.Assert(verticeData.GetArrayLength() == 3);

            var vertice = new Vector3(verticeData[0].GetSingle(), verticeData[1].GetSingle(), verticeData[2].GetSingle());
            vertice = vertice.Transform(transformMatrix);
            vertice *= scale;
            model.Vertices.Add(vertice);
        }

        foreach (var verticeGroupData in verticeGroups.EnumerateArray())
        {
            var verticeGroup = verticeGroupData.EnumerateArray().Select(element => element.GetInt32()).ToArray();
            model.VerticeGroups.Add(verticeGroup);
        }

        foreach (var normalData in normals.EnumerateArray())
        {
            Debug.Assert(normalData.GetArrayLength() == 3);

            var normal = new Vector3(normalData[0].GetSingle(), normalData[1].GetSingle(), normalData[2].GetSingle());
            normal = normal.Transform(transformMatrix);
            normal *= scale;
            model.Normals.Add(normal);
        }

        foreach (var normalGroupData in normalGroups.EnumerateArray())
        {
            var normalGroup = normalGroupData.EnumerateArray().Select(element => element.GetInt32()).ToArray();
            model.NormalGroups.Add(normalGroup);
        }

        return model;
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
