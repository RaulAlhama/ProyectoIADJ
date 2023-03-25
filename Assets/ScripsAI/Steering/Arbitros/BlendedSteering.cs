using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BehaviorAndWeight
   {
    public SteeringBehaviour behavior;
    public float weight;

    public BehaviorAndWeight(SteeringBehaviour behavior, float weight){
        this.behavior = behavior;
        this.weight = weight;
    }
   }


public class BlendedSteering //: SteeringBehaviour
{
   
    public List<BehaviorAndWeight> behaviors;

    public Steering GetSteering(AgentNPC agent, SteeringBehaviour[] listSteerings)
    {
        //Lista que va a contener los distintos steering con sus respectivos pesos
        behaviors = new List<BehaviorAndWeight>();
        
        foreach (SteeringBehaviour ster in listSteerings){
            behaviors.Add(new BehaviorAndWeight(ster,ster.weight));
        }
        
        Steering steer = new Steering(); //Steering final a aplicar

        Steering s;

        foreach (BehaviorAndWeight behaviorAndWeight in behaviors){
            if(behaviorAndWeight.behavior is Arrive){
                if(agent.Rotation == 0){ //Ejecutamos después del movimiento angular
                    s = behaviorAndWeight.behavior.GetSteering(agent);
                    steer.linear += behaviorAndWeight.weight * s.linear;
                }
            } else{
                s = behaviorAndWeight.behavior.GetSteering(agent);
                steer.linear += behaviorAndWeight.weight * s.linear;
                steer.angular += behaviorAndWeight.weight * s.angular;
            }
            //Debug.Log("BlendedSteering.cs: " + "Movimiento: " + behaviorAndWeight.behavior + " Vector: " + behaviorAndWeight.weight * s.linear + " Peso: " + behaviorAndWeight.weight);
         }

         if (steer.linear.magnitude > (steer.linear.normalized * agent.MaxAcceleration).magnitude){
           //steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        }


        return steer;

    }

}
