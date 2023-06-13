using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AEstrella
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


    // Función que comprueba si se ha alcanzado el siguiente nodo del camino
    public void comprobarCamino(List<Vector3> caminosAzul){

        // Obtenemos las coordenadas del punto actual
        int i;
        int j;
        mundo.getCoordenadas(player.Position,out i,out j);
        Coordenadas puntoActual = new Coordenadas();
         
        puntoActual.x = i;
        puntoActual.y = j;

        // Obtenemos las coordenadas del siguiente nodo                          
        int x;
        int y;
        mundo.getCoordenadas(caminosAzul[0],out x,out y);

        Vector3 aux = caminosAzul[0];

        // Si hemos llegado a la casilla del nodo actual, calculamos la distancia que le queda para llegar al centro de la casilla
        if(puntoActual.x == x && puntoActual.y == y){
            float dis = (aux-player.Position).magnitude;
            
            // Si le queda menos de la distancia mínima, elegimos el siguiente nodo
            if(dis < DIS_MINIMA)
            {
                // Se elimina el nodo que acabamos de alcanzar
                caminosAzul.RemoveAt(0);
                // Si no quedan nodos en la lista, se establece la llegada del player a true
                if (caminosAzul.Count==0)
                {
                    player.setLLegada(true);
                    return;
                }
                // En caso contrario, se actualiza el nodo actual y se le asigna un npc virtual al player
                aux = caminosAzul[0]; 
                npcVirtual.transform.position = aux;
                player.setTarget(npcVirtual);
                
                return;
            } 

        }

    }

    // Función que calcula el camino
    public List<Vector3> aestrella(int[,] peligro, bool path){

        // Lista de nodos vecinos
        List<Node_A> abierta = new List<Node_A>();
        // Lista de nodos visitados
        List<Node_A> cerrada = new List<Node_A>();

        // Se calcula el punto actual del player
        int i;
        int j;

        mundo.getCoordenadas(player.Position,out i,out j);
        Coordenadas puntoActual = new Coordenadas();
         
        puntoActual.x = i;
        puntoActual.y = j;

        // Se calcula las coordenadas del objetivo
        Coordenadas puntoObjetivo = new Coordenadas();

        puntoObjetivo.x = iObjetivo;
        puntoObjetivo.y = jObjetivo;

        // Si hemos llegado al objetivo, le indicamos al player que no se mueva
        if ((puntoActual.x == puntoObjetivo.x) && (puntoActual.y == puntoObjetivo.y)){
            List<Vector3> res = new List<Vector3>();
            res.Add(player.Position);
            return res;
        }

        // Inicializamos el nodo objetivo
        Node_A nodoObjetivo = new Node_A(puntoObjetivo, null, iObjetivo,jObjetivo);

        // Inicializamos el nodo inicial
        Node_A nodoInicial = new Node_A(puntoActual, null, iObjetivo, jObjetivo);
        Node_A nodoActual = nodoInicial;

        // Añadimos a la lista de nodos sin expandir el nodo inicial
        abierta.Add(nodoActual);
    
        // Mientras que no esté vacía la lista de nodos sin expandir seguimos
        while (abierta.Count > 0){

            // Escogemos de la lista de nodos abiertos el de menor valor f
            nodoActual = menorF(abierta);
            // Eliminamos el nodo elegido de la lista abierta y lo añadimos a la lista cerrada
            eliminarNodo(nodoActual, abierta);
            cerrada.Add(nodoActual);
            
            // Si se trata del nodo final, acabamos la función y creamos una lista de vectores con los nodos del camino
            if ((nodoActual.corde.x == nodoObjetivo.corde.x) && (nodoActual.corde.y == nodoObjetivo.corde.y)){
                List<Node_A> res = new List<Node_A>();
                List<Vector3> vec = new List<Vector3>();
                while ((nodoActual.corde.x != nodoInicial.corde.x) || (nodoActual.corde.y != nodoInicial.corde.y)){
                    res.Insert(0, nodoActual);
                    nodoActual = nodoActual.padre;
                }

                foreach (Node_A nodo in res){
                    Vector3 vec_aux = mundo.getPosicionReal(nodo.corde.x,nodo.corde.y) + new Vector3(2,0,2); // 1,0,1
                    vec.Add(vec_aux);
                }

                // Le asignamos el primer vector de la lista como npc virtual
                Vector3 aux = vec[0]; 
                npcVirtual.transform.position = aux;
                player.setTarget(npcVirtual);

                return vec;
            }
                
            // Generamos la lista de vecinos del nodo actual 
            List<Node_A> vecinos = generaEspacioLocal(nodoActual);

            // Recorremos los nodos vecinos
            foreach (Node_A nodo in vecinos){
                
                // Si el nodo vecino no está en la lista cerrada, pasamos a la siguiente iteración
                if (nodoEsta(nodo, cerrada)){
                    continue;
                }
                
                // Calculamos los pesos del nodo vecino
                nodo.g = nodoActual.g + Math.Sqrt(Math.Pow(nodo.corde.x-nodoActual.corde.x,2)+Mathf.Pow(nodo.corde.y-nodoActual.corde.y,2));
                nodo.h = Math.Sqrt(Math.Pow(nodo.corde.x-nodoObjetivo.corde.x,2)+Mathf.Pow(nodo.corde.y-nodoObjetivo.corde.y,2));
                if (path)
                {
                    nodo.f = nodo.g + nodo.h + ApplyTerreno(nodo.corde.x, nodo.corde.y) + (peligro[nodo.corde.x,nodo.corde.y] * 5);
                }
                else
                {
                    nodo.f = nodo.g + nodo.h + ApplyTerreno(nodo.corde.x, nodo.corde.y);
                }

                // Si el peso del nodo vecino es mayor que su contraparte de la lista abierta, pasamos a la siguiente iteración
                if (mayorG(nodo, abierta)){
                    continue;
                }

                // Establecemos como padre del nodo vecino el nodo actual
                nodo.padre = nodoActual;
                // Añadimos el nodo vecino a la lista de nodos abiertos
                abierta.Add(nodo);
            } 

        }

        List<Vector3> zero = new List<Vector3>();
        zero.Add(player.Position);
        return zero;

    }

    // Función que establece la posición del npcvirtual
    public void setPosicionNpcVirtual(Vector3 pos){

        npcVirtual.transform.position = pos;
    }

    // Función que establece la orientación del npcvirtual
    public void setOrientacionNpcVirtual(float or){

        npcVirtual.Orientation = or;
    }

    // Función que establece el valor del grafo de movimiento
    public void setGrafoMovimiento(double[,] grm){

        grafoMovimiento = grm;
    }

    // Función que establece las coordenadas del objetivo y su npcvirtual
    public void setObjetivos(int i,int j,Agent npcVr){

        iObjetivo = i;
        jObjetivo = j;
        npcVirtual = npcVr;
    }

    // Función que devuelve el nodo de menor f de una lista
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

    // Función que devuelve si se encuentra un nodo en una lista
    public bool nodoEsta(Node_A nodo, List<Node_A> nodos){
        foreach (Node_A n in nodos){
            if ((n.corde.x == nodo.corde.x) && (n.corde.y == nodo.corde.y))
                return true;
        }
        return false;
    }

    // Función que comprueba si un nodo tiene un valor de g mayor que su aparcición en una lista
    public bool mayorG(Node_A nodo, List<Node_A> nodos){
        foreach (Node_A n in nodos){
            if ((n.corde.x == nodo.corde.x) && (n.corde.y == nodo.corde.y))
            {
                if (nodo.g > n.g)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    // Función que elimina un nodo de una lista
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


    // Función que genera una lista con los nodos adyacentes al nodo introducido por parámetro
    private List<Node_A> generaEspacioLocal(Node_A nodoActual){

        int i = nodoActual.corde.x;
        int j = nodoActual.corde.y;
        List<Node_A> nodos = new List<Node_A>();
        

        // Creamos el nodo de la izquierda y lo añadimos a la lista de nodos vecinos
        if((i-1 >= 0)  && grafoMovimiento[i-1,j] != infinito){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j] != 0)
                grafoMovimiento[i-1,j] = Double.PositiveInfinity;

        }
        // Creamos el nodo de arriba-izquierda y lo añadimos a la lista de nodos vecinos
        if((i-1 >= 0 && j+1 < 100) && grafoMovimiento[i-1,j+1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j+1] != 0)
                grafoMovimiento[i-1,j+1] = Double.PositiveInfinity;

        }
        // Creamos el nodo de arriba-derecha y lo añadimos a la lista de nodos vecinos
        if((i+1 < 100 && j+1 < 100) && grafoMovimiento[i+1,j+1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j+1] == infinito)){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j+1] != 0)
                grafoMovimiento[i+1,j+1] = Double.PositiveInfinity;

        }
        // Creamos el nodo de abajo-izquierda y lo añadimos a la lista de nodos vecinos
        if((i-1 >= 0 && j-1 >= 0) && grafoMovimiento[i-1,j-1] != infinito && !(grafoMovimiento[i-1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i-1;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i-1,j-1] != 0)
                grafoMovimiento[i-1,j-1] = Double.PositiveInfinity;

        }
        // Creamos el nodo de abajo-derecha y lo añadimos a la lista de nodos vecinos
        if((j-1 >= 0 && i+1 < 100) && grafoMovimiento[i+1,j-1] != infinito && !(grafoMovimiento[i+1,j] == infinito && grafoMovimiento[i,j-1] == infinito)){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            //nA.g = 0.4f;
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j-1] != 0)
                grafoMovimiento[i+1,j-1] = Double.PositiveInfinity;

        }
        // Creamos el nodo de arriba y lo añadimos a la lista de nodos vecinos
        if((j+1 < 100) &&grafoMovimiento[i,j+1] != infinito){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j+1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i,j+1] != 0)
                grafoMovimiento[i,j+1] = Double.PositiveInfinity;

        }
         // Creamos el nodo de derecha y lo añadimos a la lista de nodos vecinos
        if((i+1 < 100) && grafoMovimiento[i+1,j] != infinito){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i+1;
            nPunto.y = j;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i+1,j] != 0)
                grafoMovimiento[i+1,j] = Double.PositiveInfinity;

        }
         // Creamos el nodo de abajo y lo añadimos a la lista de nodos vecinos
        if((j-1 >= 0) && grafoMovimiento[i,j-1] != infinito){ 

            Coordenadas nPunto = new Coordenadas();
            nPunto.x = i;
            nPunto.y = j-1;
            Node_A nA = new Node_A(nPunto, nodoActual, iObjetivo, jObjetivo);
            nodos.Add(nA);
            if(grafoMovimiento[i,j-1] != 0)
                grafoMovimiento[i,j-1] = Double.PositiveInfinity;

        }
        // Devolvemos la lista de nodos vecinos
        return nodos;
    }

    // Función que calcula el peso añadido debido al tipo de terreno
    private float ApplyTerreno(int i, int j)
    {

        // Calculamos el vector posición correspondiente a las coordenadas introducidas por parámetro
        Vector3 posicion = mundo.getPosicionReal(i,j) + new Vector3(2,0,2);

        // Trazamos un rayo hacia abajo desde la posición del objeto
        RaycastHit hit;
        if (Physics.Raycast(posicion+Vector3.up, Vector3.down, out hit, 2f))
        {
            // Si el rayo colisiona con un objeto en la capa "groundLayerMask", podemos determinar el tipo de suelo
            switch(hit.collider.gameObject.tag) 
            {
                case "Cesped":
                    return 25.0f;
                case "Tierra":
                    return 50.0f;
                default:
                    return 0.0f;
            }

        }

        return 0.0f;

    }

}
