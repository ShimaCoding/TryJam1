using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindState {
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
    public string action;//para indicar el nombre de la función y posibles variables, para hacerlo, se escribe primero el nombre de la función y luego las variables, separándolas usando ; y sin dejar ningún espacio, utilizar solo números, puntos y usar comas para separar entre dígitos de la mmisma variable (como un Vector2), no usar paréntesis ni niuna otra tontera.
}

public class mg_rewind : MonoBehaviour {

    public float timeLimit = 3f;//límite de tiempo para empezar a borrar elementos
    bool rewinding;//determina si se está rebobinando
    bool replaying;//determmina si se están reproduciendo los estados rebobinados
    string action = null;

    public GameObject rewindableShadowObj;//corresponde a la sombra invocada que rebobinará las acciones del objeto principal
    public GameObject mainBodyObj;//para extraer información del Transform del objeto "body"

    List<RewindState> rewindStates = new List<RewindState>();//para guardar variables del transform
    List<RewindState> newRewindStates = new List<RewindState>();//para copiar la lista rewindStates
    List<RewindState> newReplayStates = new List<RewindState>();//para copiar los elementos utilizados de la lista newRewindStates

    private void Update () {
        if (Input.GetMouseButtonDown(0))
            RewindStart();
    }

    void FixedUpdate() {
        RewindState state = new RewindState { pos = transform.position, rot = mainBodyObj.transform.eulerAngles, scale = mainBodyObj.transform.localScale, action = null };//crea un nuevo elemento para contener la posición, rotación y escala actuales del objeto
        if (action != null) {
            state.action = action;
            action = null;
        }
        rewindStates.Add(state);//añade el nuevo elemento a la lista
        if (timeLimit <= 0)
            rewindStates.RemoveAt(0);//borra el elemento más viejo de la lista en caso de que el tiempo límite se agote
        timeLimit -= Time.deltaTime;
        if (rewinding) {//le aplica la información de Transform a la sombra invocada, desde el estado más actual hacia el más antiguo
            rewindableShadowObj.transform.position = newRewindStates[newRewindStates.Count - 1].pos;
            rewindableShadowObj.transform.eulerAngles = newRewindStates[newRewindStates.Count - 1].rot;
            rewindableShadowObj.transform.localScale = newRewindStates[newRewindStates.Count - 1].scale;
            newReplayStates.Add(newRewindStates[newRewindStates.Count - 1]);
            newRewindStates.RemoveAt(newRewindStates.Count - 1);//elimina el elemento más actual de la lista para que luego sea utilizado el que sigue en la lista
            if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista se quede sin elementos
                RewindStop();
        }
        if (replaying) {//la misma lecera que rewind pero la revés, no seas pao
            rewindableShadowObj.transform.position = newReplayStates[newReplayStates.Count - 1].pos;
            rewindableShadowObj.transform.eulerAngles = newReplayStates[newReplayStates.Count - 1].rot;
            rewindableShadowObj.transform.localScale = newReplayStates[newReplayStates.Count - 1].scale;
            if (newReplayStates[newReplayStates.Count - 1].action != null) {//esta lecera revisa si hay un string guardado en el elemento de la lista, luego lo separa en distintos strings usando el ";" y agarra el primer string para indicarle al objeto y su parent que ejecute cualquier código con ese nombre, además de pasar el string original como una variable para ser descifrada al otro lado
                string[] tempString = newReplayStates[newReplayStates.Count - 1].action.Split(';');
                rewindableShadowObj.SendMessageUpwards(tempString[0], newReplayStates[newReplayStates.Count - 1].action, SendMessageOptions.DontRequireReceiver);
            }
            newReplayStates.RemoveAt(newReplayStates.Count - 1);
            if (newReplayStates.Count <= 0) {//lo cancela en caso de que la lista se quede sin elementos
                replaying = false;
                rewindableShadowObj.SetActive(false);
                rewindableShadowObj.transform.SetParent(transform);
            }
        }
    }
    public void RewindStart () {
        if (rewinding || replaying)
            return;
        newRewindStates = new List<RewindState>(rewindStates);//copia la lista principal para no afectarla
        if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista no tenga ningún elemento
            return;
        rewindableShadowObj.transform.localPosition = mainBodyObj.transform.localPosition;
        rewindableShadowObj.transform.eulerAngles = mainBodyObj.transform.eulerAngles;
        rewindableShadowObj.transform.localScale = mainBodyObj.transform.localScale;
        rewindableShadowObj.SetActive(true);//activa el objeto "sombra invocada"
        rewindableShadowObj.transform.SetParent(null);//anula su Parent para que ya no dependa del Transform del objeto original
        rewinding = true;
    }
    public void RewindStop () {
        rewinding = false;
        if(newReplayStates.Count > 0)//ejecuta el Replay en caso de que SI exista una lista de Rewind
            replaying = true;
    }

    public void AddAction(string methodName, string variable) {//asigna la variable "action" con la información del evento para luego ser aplicada al siguiente elemento guardado en rewindStates
        if(variable != null)
            action = methodName + ";" + variable;
        else action = methodName + ";null";
    }
}
