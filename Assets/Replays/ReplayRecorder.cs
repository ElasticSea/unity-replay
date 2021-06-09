using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Project.Scripts.Gameplay.Replays;
using ElasticSea.Framework.Util;
using Replays;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{
    [SerializeField] private Transform[] transforms;

    private List<Snapshot> Snapshots = new List<Snapshot>();

    // Update is called once per frame
    private void Update()
    {
        var states = new TransformState[transforms.Length];
        for (var i = 0; i < states.Length; i++)
        {
            var transform = transforms[i];
            var state = new TransformState();
            state.Position = transform.position;
            state.Rotation = transform.rotation.eulerAngles;
            state.Scale = transform.lossyScale;
            states[i] = state;
        }

        var snapshot = new Snapshot();
        snapshot.Time = Time.time;
        snapshot.States = states;

        Snapshots.Add(snapshot);
    }

    public void Save()
    {
        var replay = new Replay
        {
            Transforms = transforms.Select(t => t.name).ToArray(),
            Snapshots = Snapshots.ToList()
        };

        var bytes = ReplaySeriliazer.Write(replay);

        var file = new FileInfo(Path.Combine(Application.persistentDataPath, "Replays", "replayTest.rpl"));
        Utils.EnsureDirectory(file.Directory.FullName);
        File.WriteAllBytes(file.FullName, bytes);
    }
}