using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState {
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
}

public class mg_rewind : MonoBehaviour {

    public float timeLimit = 5f;//límite de tiempo para empezar a borrar elementos
    bool rewinding;//determina si se está rebobinando

    public GameObject rewindableShadowObj;//corresponde a la sombra invocada que rebobinará las acciones del objeto principal

    List<RewindState> rewindStates;//para guardar variables del transform
    List<RewindState> newRewindStates;//para copiar la lista rewindStates

    void FixedUpdate() {
        RewindState state = new RewindState { pos = transform.position, rot = transform.eulerAngles, scale = transform.localScale };//crea un nuevo elemento para contener la posición, rotación y escala actuales del objeto
        rewindStates.Add(state);//añade el nuevo elemento a la lista
        if (timeLimit <= 0)
            rewindStates.RemoveAt(0);//borra el elemento más viejo de la lista en caso de que el tiempo límite se agote
        timeLimit -= Time.deltaTime;
        if (rewinding) {//le aplica la información de Transform a la sombra invocada, desde el estado más actual hacia el más antiguo
            rewindableShadowObj.transform.position = newRewindStates[newRewindStates.Count - 1].pos;
            rewindableShadowObj.transform.eulerAngles = newRewindStates[newRewindStates.Count - 1].rot;
            rewindableShadowObj.transform.localScale = newRewindStates[newRewindStates.Count - 1].scale;
            newRewindStates.RemoveAt(newRewindStates.Count - 1);//elimina el elemento más actual de la lista para que luego sea utilizado el que sigue en la lista
            if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista se quede sin elementos
                rewinding = false;
        }
    }
    public void Rewind () {
        newRewindStates = new List<RewindState>(rewindStates);//copia la lista principal para no afectarla
        if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista no tenga ningún elemento
            return;
        rewindableShadowObj.SetActive(true);//activa el objeto "sombra invocada"
        rewindableShadowObj.transform.SetParent(null);//anula su Parent para que ya no dependa del Transform del objeto original
        rewinding = true;
    }
    public void Stop () {
        rewinding = false;
    }
}
