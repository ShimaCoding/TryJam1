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

    public GameObject mainBodyObj;//para extraer información del Transform del objeto "body"
    public GameObject rewindableShadowContainerObj;
    public GameObject rewindableShadowObj;//corresponde a la sombra invocada que rebobinará las acciones del objeto principal
    public Transform shadowEmisionTrans;
    [Space]
    public Collider[] shadowColliders;

    List<RewindState> rewindStates = new List<RewindState>();//para guardar variables del transform
    List<RewindState> newRewindStates = new List<RewindState>();//para copiar la lista rewindStates
    List<RewindState> newReplayStates = new List<RewindState>();//para copiar los elementos utilizados de la lista newRewindStates

    void FixedUpdate() {
        RewindState state = new RewindState { pos = transform.position, rot = transform.eulerAngles, scale = mainBodyObj.transform.localScale, action = null };//crea un nuevo elemento para contener la posición, rotación y escala actuales del objeto
        if (action != null) {
            state.action = action;
            action = null;
        }
        rewindStates.Add(state);//añade el nuevo elemento a la lista
        if (timeLimit <= 0)
            rewindStates.RemoveAt(0);//borra el elemento más viejo de la lista en caso de que el tiempo límite se agote
        timeLimit -= Time.deltaTime;
        if (rewinding) {//le aplica la información de Transform a la sombra invocada, desde el estado más actual hacia el más antiguo
            rewindableShadowContainerObj.transform.position = newRewindStates[newRewindStates.Count - 1].pos;
            rewindableShadowContainerObj.transform.eulerAngles = newRewindStates[newRewindStates.Count - 1].rot;
            rewindableShadowObj.transform.localScale = newRewindStates[newRewindStates.Count - 1].scale;
            newReplayStates.Add(newRewindStates[newRewindStates.Count - 1]);
            newRewindStates.RemoveAt(newRewindStates.Count - 1);//elimina el elemento más actual de la lista para que luego sea utilizado el que sigue en la lista
            if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista se quede sin elementos
                RewindStop();
            else {
                newReplayStates.Add(newRewindStates[newRewindStates.Count - 1]);
                newRewindStates.RemoveAt(newRewindStates.Count - 1);
                if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista se quede sin elementos
                    RewindStop();
            }
        }
        if (replaying) {//la misma lecera que rewind pero la revés, no seas pao
            rewindableShadowContainerObj.transform.position = newReplayStates[newReplayStates.Count - 1].pos;
            rewindableShadowContainerObj.transform.eulerAngles = newReplayStates[newReplayStates.Count - 1].rot;
            rewindableShadowObj.transform.localScale = newReplayStates[newReplayStates.Count - 1].scale;
            if (newReplayStates[newReplayStates.Count - 1].action != null) {//esta lecera revisa si hay un string guardado en el elemento de la lista, luego lo separa en distintos strings usando el ";" y agarra el primer string para indicarle al objeto y su parent que ejecute cualquier código con ese nombre, además de pasar el string original como una variable para ser descifrada al otro lado
                string[] tempString = newReplayStates[newReplayStates.Count - 1].action.Split(';');
                rewindableShadowObj.SendMessageUpwards(tempString[0], newReplayStates[newReplayStates.Count - 1].action, SendMessageOptions.DontRequireReceiver);
            }
            newReplayStates.RemoveAt(newReplayStates.Count - 1);
            if (newReplayStates.Count <= 0) {//lo cancela en caso de que la lista se quede sin elementos
                replaying = false;
                rewindableShadowContainerObj.SetActive(false);
                rewindableShadowContainerObj.transform.SetParent(transform);
                if (shadowEmisionTrans != null) {
                    shadowEmisionTrans.SetParent(rewindableShadowContainerObj.transform);
                    shadowEmisionTrans.localPosition = Vector3.zero;
                }
            }
        }
    }
    public void RewindStart () {
        AudioManager.instance.Play("ReverseDraw");
        if (rewinding || replaying)
            return;
        newRewindStates = new List<RewindState>(rewindStates);//copia la lista principal para no afectarla
        if (newRewindStates.Count <= 0)//lo cancela en caso de que la lista no tenga ningún elemento
            return;
        rewindableShadowContainerObj.transform.localPosition = mainBodyObj.transform.localPosition;
        rewindableShadowContainerObj.transform.eulerAngles = mainBodyObj.transform.eulerAngles;
        rewindableShadowObj.transform.localScale = mainBodyObj.transform.localScale;
        foreach(Collider col in shadowColliders)
            col.enabled = false;
        rewindableShadowContainerObj.SetActive(true);//activa el objeto "sombra invocada"
        rewindableShadowContainerObj.transform.SetParent(null);//anula su Parent para que ya no dependa del Transform del objeto original
        rewinding = true;
    }
    public void RewindStop () {
        rewinding = false;
        if (newReplayStates.Count > 0) {//ejecuta el Replay en caso de que SI exista una lista de Rewind
            foreach (Collider col in shadowColliders)
                col.enabled = true;
            replaying = true;
        }
    }

    /// <summary>
    /// Priner string es pal nombre de la función y el segundo pa las variables, no deben haber espacios ni ningún símbolo aparte de "," y ".".
    /// </summary>
    /// <param name="methodName">Nombre exacto de la función.</param>
    /// <param name="variable">Valores de la variable usando solo "," y ".", omitiendo espacios, paréntesis, etc (puedes meter más variables separándolas con ";".</param>
    /// <returns>ULTRA GEY.</returns>
    public void AddAction(string methodName, string variable = null) {//asigna la variable "action" con la información del evento para luego ser aplicada al siguiente elemento guardado en rewindStates
        if(variable != null)
            action = methodName + ";" + variable;
        else action = methodName + ";null";
    }
}
