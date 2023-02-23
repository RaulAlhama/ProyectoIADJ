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


public class BlendedSteering : SteeringBehaviour
{
   public List<BehaviorAndWeight> behaviors;

    public void AplicarArbitro(SteeringBehaviour[] listSteerings){

        behaviors = new List<BehaviorAndWeight>();
        
        foreach (SteeringBehaviour steer in listSteerings){
            behaviors.Add(new BehaviorAndWeight(steer,steer.weight));
        }
   }

    public override Steering GetSteering(AgentNPC agent)
    {
        
        Steering steer = new Steering();

        foreach (BehaviorAndWeight behavior in behaviors){
            steer.linear += behavior.weight * behavior.behavior.GetSteering(agent).linear;
            steer.angular += behavior.weight * behavior.behavior.GetSteering(agent).angular;
         }

         if (steer.linear.magnitude > (steer.linear.normalized * agent.MaxAcceleration).magnitude){
           steer.linear = steer.linear.normalized * agent.MaxAcceleration;
        }


        return steer;

    }

}
