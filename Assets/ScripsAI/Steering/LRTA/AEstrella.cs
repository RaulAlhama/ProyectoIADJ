using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AEstrella : MonoBehaviour
{

    private GridFinal mundo;
    private AgentNPC player;
    private Agent npcVirtual;
    private double[,] grafoMovimiento;
    private const double infinito = Double.PositiveInfinity;
    private int iObjetivo;
    private int jObjetivo;
    private const float DIS_MINIMA = 0.3f; 

    public struct Coordenadas{
        public int x;
        public int y;
    }

    public AEstrella(GridFinal wld, AgentNPC pl){
        mundo = wld;
        player = pl;

    }

    public void comprobarCamino(List<Vector3> caminosAzul){

        if (caminosAzul.Count==1)
        {
            player.setLLegada(true);
            return;
        }

        int i;
        int j;
        mundo.getCoordenadas(player.Position,out i,out j);
        Coordenadas puntoActual = new Coordenadas();
         
        puntoActual.x = i;
        puntoActual.y = j;
                          
        int x;
        int y;
        mundo.getCoordenadas(caminosAzul[0],out x,out y);

        Vector3 aux = caminosAzul[0];
            
        //Debug.Log("puntoActual.x = " + puntoActual.x + " caminosAzul[0].x = " + x + " puntoActual.y = " + puntoActual.y + " caminosAzul[0].y = " + y);
        if(puntoActual.x == x && puntoActual.y == y){
            float dis = (aux-player.Position).magnitude;
            //Debug.Log("dis = " + dis);
            if(dis < DIS_MINIMA)
            {
                caminosAzul.RemoveAt(0);
                aux = caminosAzul[0]; 
                npcVirtual.transform.position = aux;
                player.setTarget(npcVirtual);

                string str="Punto del camino alcanzado: ";
                foreach (Vector3 v in caminosAzul)
                    str = str + v + " ";
                //Debug.Log(str);

                str="Punto del camino alcanzado: ";
                foreach (Vector3 v in caminosAzul)
                {
                    int a;
                    int b;
                    mundo.getCoordenadas(v, out a, out b);
                    str = str + "(" + a + "," + b + ") ";
                }
                Debug.Log(str);
                
                return;
            } 

        }

    }

    public List<Vector3> aestrella(){

        // Lista de nodos vecinos
        List<Node_A> abierta = new List<Node_A>();
        // Lista de nodos visitados
        List<Node_A> cerrada = new List<Node_A>();

        int i;
        int j;

        mundo.getCoordenadas(player.Position,out i,out j);
        Coordenadas puntoActual = new Coordenadas();
         
        puntoActual.x = i;
        puntoActual.y = j;

        Coordenadas puntoObjetivo = new Coordenadas();

        //iObjetivo = 34;
        //jObjetivo = 15;
        puntoObjetivo.x = iObjetivo;
        puntoObjetivo.y = jObjetivo;

        if ((puntoActual.x == puntoObjetivo.x) && (puntoActual.y == puntoObjetivo.y)){
            List<Vector3> res = new List<Vector3>();
            res.Add(Vector3.zero);
            return res;
        }

        // Inicializamos el nodo objetivo
        Node_A nodoObjetivo = new Node_A(puntoObjetivo, null, iObjetivo,jObjetivo);

        // Inicializamos el nodo inicial
        Node_A nodoInicial = new Node_A(puntoActual, null, iObjetivo, jObjetivo);
        Node_A nodoActual = nodoInicial;

        // Añadimos a la lista de nodos sin expandir el nodo inicial
        abierta.Add(nodoActual);
        
        string str = "";
        foreach (Node_A nodo in abierta)
            str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ")";
        //Debug.Log("Lista abierta: " + str);

        str = "";
        foreach (Node_A nodo in cerrada)
            str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ")";
        //Debug.Log("Lista cerrada: " + str);

        int z = 0;
        // Mientras que no esté vacía la lista de nodos sin expandir seguimos
        while (abierta.Count > 0){

            // Escogemos de la lista de nodos abiertos el de menor valor f
            nodoActual = menorF(abierta);
            eliminarNodo(nodoActual, abierta);
            cerrada.Add(nodoActual);

            //Debug.Log("(" + z + ") Nodo actual: (" + nodoActual.corde.x + "," + nodoActual.corde.y + ")" + " - Nodo objetivo: (" +  nodoObjetivo.corde.x + "," +  nodoObjetivo.corde.y + ")");
            
            // Si se trata del nodo final, acabamos la función
            if ((nodoActual.corde.x == nodoObjetivo.corde.x) && (nodoActual.corde.y == nodoObjetivo.corde.y)){
                List<Node_A> res = new List<Node_A>();
                List<Vector3> vec = new List<Vector3>();
                while ((nodoActual.corde.x != nodoInicial.corde.x) || (nodoActual.corde.y != nodoInicial.corde.y)){
                    res.Insert(0, nodoActual);
                    nodoActual = nodoActual.padre;
                }
                //Debug.Log("Longitud del camino: " + res.Count);
                str = "Fin de la ejecución: ";
                foreach (Node_A nodo in res)
                    str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ")";
                Debug.Log(str);

                foreach (Node_A nodo in res){
                    Vector3 vec_aux = mundo.getPosicionReal(nodo.corde.x,nodo.corde.y) + new Vector3(2,0,2); // 1,0,1
                    vec.Add(vec_aux);
                }

                str="Vectores finales: ";
                foreach (Vector3 v in vec)
                    str = str + v + " ";
                //Debug.Log(str);

                Vector3 aux = vec[0]; 
                npcVirtual.transform.position = aux;
                player.setTarget(npcVirtual);

                return vec;
            }
                
            // Generamos la lista de vecinos del nodo actual 
            List<Node_A> vecinos = generaEspacioLocal(nodoActual);

            // Actualizamos los pesos de los nodos vecinos
            //puntoActual = actualizaPesos(puntos, puntoActual, abierta, cerrada);

            // Actualizamos los pesos de los nodos vecinos y cambiamos el puntoActual
           
            str = "";
            foreach (Node_A nodo in vecinos)
                str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ") ("+ nodo.g + "+" + nodo.h + ") ---";
            //Debug.Log("Nodos vecinos: " + str);
        

            foreach (Node_A nodo in vecinos){
                
                //Debug.Log("Turno del nodo: " + " (" + nodo.corde.x + "," + nodo.corde.y + ")");

                if (nodoEsta(nodo, cerrada)){
                    //Debug.Log("Entra en el primer if");
                    continue;
                }
                
                //nodo.g = nodoActual.g + Math.Abs(nodo.corde.x - nodoActual.corde.x) + Math.Abs(nodo.corde.y - nodoActual.corde.y);
                nodo.g = nodoActual.g + Math.Sqrt(Math.Pow(nodo.corde.x-nodoActual.corde.x,2)+Mathf.Pow(nodo.corde.y-nodoActual.corde.y,2));
                //nodo.h = Math.Abs(nodo.corde.x - nodoObjetivo.corde.x) + Math.Abs(nodo.corde.y - nodoObjetivo.corde.y);
                nodo.h = Math.Sqrt(Math.Pow(nodo.corde.x-nodoObjetivo.corde.x,2)+Mathf.Pow(nodo.corde.y-nodoObjetivo.corde.y,2));
                nodo.f = nodo.g + nodo.h + ApplyTerreno(nodo.corde.x, nodo.corde.y);

                //Debug.Log(" (" + nodo.corde.x + "," + nodo.corde.y + ") ("+ nodo.g + "+" + nodo.h + ")");

                if (mayorG(nodo, abierta)){
                        continue;
                }

                nodo.padre = nodoActual;
                abierta.Add(nodo);
            } 

            str = "";
            foreach (Node_A nodo in abierta)
                str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ")";
            //Debug.Log("Lista abierta: " + str);

            str = "";
            foreach (Node_A nodo in cerrada)
                str = str + "   " + " (" + nodo.corde.x + "," + nodo.corde.y + ")";
            //Debug.Log("Lista cerrada: " + str);

            z++;
        }

        List<Vector3> zero = new List<Vector3>();
        zero.Add(new Vector3(0f,0f,0f));
        return zero;

    }

    public void setPosicionNpcVirtual(Vector3 pos){

        npcVirtual.transform.position = pos;
    }
    public void setOrientacionNpcVirtual(float or){

        npcVirtual.Orientation = or;
    }
    public void setGrafoMovimiento(double[,] grm){

        grafoMovimiento = grm;
    }

    public void setObjetivos(int i,int j,Agent npcVr){

        iObjetivo = i;
        jObjetivo = j;
        npcVirtual = npcVr;
    }

    public Node_A menorF(List<Node_A> nodos){

        double menor = Double.PositiveInfinity;
        Node_A result = null;
        foreach(Node_A n in nodos){

            if(n.f < menor){

                menor = n.f;
                result = n;
            }
        }

        return result;
    }

    public bool nodoEsta(Node_A nodo, List<Node_A> nodos){
        foreach (Node_A n in nodos){
            if ((n.corde.x == nodo.corde.x) && (n.corde.y == nodo.corde.y))
                return true;
        }
        return false;
    }

    public bool mayorG(Node_A nodo, List<Node_A> nodos){
        foreach (Node_A n in nodos){
            if ((n.corde.x == nodo.corde.x) && (n.corde.y == nodo.corde.y))
            {
                //Debug.Log("comparacion de g: " + nodo.g + " y " + n.g);
                if (nodo.g > n.g)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    public static void eliminarNodo(Node_A nodo, List<Node_A> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].corde.x == nodo.corde.x && list[i].corde.y == nodo.corde.y)
            {
                list.RemoveAt(i);
            }
        }
    }



    private List<Node_A> generaEspacioLocal(Node_A nodoActual){

        int i = nodoActual.corde.x;
        int j = nodoActual.corde.y;
        List<Node_A> nodos = new List<Node_A>();
        
        /*
        if(grafoMovimiento[i,j] != infinito){ // centro

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i,j] != 0)
                grafoMovimiento[i,j] = Double.PositiveInfinity;

        }*/
        if((i-1 >= 0)  && grafoMovimiento[i-1,j] != infinito){ // izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j] != 0)
                grafoMovimiento[i-1,j] = Double.PositiveInfinity;

        }
        if((i-1 >= 0 && j+1 < 100) && grafoMovimiento[i-1,j+1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ // arriba-izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j+1] != 0)
                grafoMovimiento[i-1,j+1] = Double.PositiveInfinity;

        }
        if((i+1 < 100 && j+1 < 100) && grafoMovimiento[i+1,j+1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ // arriba-derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j+1] != 0)
                grafoMovimiento[i+1,j+1] = Double.PositiveInfinity;

        }
        if((i-1 >= 0 && j-1 >= 0) && grafoMovimiento[i-1,j-1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ // abajo-izquierda

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j-1] != 0)
                grafoMovimiento[i-1,j-1] = Double.PositiveInfinity;

        }
        if((j-1 >= 0 && i+1 < 100) && grafoMovimiento[i+1,j-1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ // abajo-derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j-1] != 0)
                grafoMovimiento[i+1,j-1] = Double.PositiveInfinity;

        }
        if((j+1 < 100) &&grafoMovimiento[i,j+1] != infinito){ // arriba

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i,j+1] != 0)
                grafoMovimiento[i,j+1] = Double.PositiveInfinity;

        }
        if((i+1 < 100) && grafoMovimiento[i+1,j] != infinito){ // derecha

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j] != 0)
                grafoMovimiento[i+1,j] = Double.PositiveInfinity;

        }
        if((j-1 >= 0) && grafoMovimiento[i,j-1] != infinito){ // abajo

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i,j-1] != 0)
                grafoMovimiento[i,j-1] = Double.PositiveInfinity;

        }
        return nodos;
    }

    private float ApplyTerreno(int i, int j)
    {

        Vector3 posicion = mundo.getPosicionReal(i,j) + new Vector3(2,0,2);

        // Tracemos un rayo hacia abajo desde la posición del objeto
        RaycastHit hit;
        if (Physics.Raycast(posicion+Vector3.up, Vector3.down, out hit, 2f))
        {
            // Si el rayo colisiona con un objeto en la capa "groundLayerMask", podemos determinar el tipo de suelo
        
            switch(hit.collider.gameObject.tag) 
            {
                case "Cesped":
                    return 0.0f;
                case "Camino":
                    return -1.0f;
                case "Tierra":
                    return 1.0f;
                default:
                    return 0.0f;
            }

        }

        return 0.0f;

    }

    /*
    private Node_A actualizaPesos(List<Node_A> nodos, Node_A puntoActual, List<Node_A> abierta, List<Node_A> cerrada){

        foreach (Node_A nodo in nodos){

            nuevo_coste = nodo.f + (Math.Abs(nodo.corde.x - puntoActual.corde.x) + Math.Abs(nodo.corde.y - puntoActual.corde.y));
            if (abierta.Contains(nodo)){
                if (nodo.g <= nuevo_coste){
                    continue;
                }
            }
            else if (cerrada.Contains(nodo)){
                if (nodo.g <= nuevo_coste){
                    cerrada.Remove(nodo);
                    abierta.Add(nodo);
                }
            }
            else{
                abierta.Add(nodo);
                nodo.h =  Math.Abs(nodo.corde.x - iObjetivo) + Math.Abs(nodo.corde.y - jObjetivo);
            }

            nodo.g = nuevo_coste;
            puntoActual = nodo.padre;
        } 

        cerrada.Add(puntoActual);      

    }*/
}
