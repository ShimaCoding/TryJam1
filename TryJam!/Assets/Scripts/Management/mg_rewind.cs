using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState {
    public Vector2 pos;
    public Vector3 rot;
    public Vector3 scale;
}

public class mg_rewind : MonoBehaviour {

    List<RewindState> rewindStates;
    List<RewindState> newRewindStates;
    public float timeLimit = 5f;
    bool rewinding;

    void FixedUpdate() {
        RewindState state = new RewindState { pos = transform.position, rot = transform.eulerAngles, scale = transform.localScale };
        rewindStates.Add(state);
        if (timeLimit <= 0)
            rewindStates.RemoveAt(0);
        timeLimit -= Time.deltaTime;
        if (rewindStates.Count == 0)
            timeLimit = 5;
        if (rewinding) {
            transform.position = newRewindStates[newRewindStates.Count - 1].pos;
            transform.eulerAngles = newRewindStates[newRewindStates.Count - 1].rot;
            transform.localScale = newRewindStates[newRewindStates.Count - 1].scale;
            newRewindStates.RemoveAt(newRewindStates.Count - 1);
        }
    }
    public void Rewind () {
        newRewindStates = new List<RewindState>(rewindStates);
        rewinding = true;
    }
    public void Stop () {
        rewinding = false;
    }
}
