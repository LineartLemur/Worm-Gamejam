using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Utility {
    public static class NavmeshUtility {
        private static Vector3[] cachedNavmeshCorners = new Vector3[100];

        public static T GetClosestCheap<T>(Vector3 from, IEnumerable<(Vector3, T)> targets) {
            
            Vector3 mypos = from;
            float minDist = 999999;
            T minTarget = default;
            foreach ((Vector3 position, T target)  in targets) {
                float dist = (position - mypos).sqrMagnitude;
                if (dist < minDist) {
                    minDist = dist;
                    minTarget = target;
                }
            }

            return minTarget;
        }

        public static T GetClosest<T>(this NavMeshAgent navMeshAgent, IEnumerable<(Vector3,T)> targets) {
            const bool CHEAP = true;

            if (CHEAP) {
                return GetClosestCheap(navMeshAgent.transform.position, targets);
            } else {
                NavMeshPath path = new NavMeshPath();
            
                float minDist = 9999999999;
                T minTarget = default;
                bool foundTarget = false;
                foreach ((Vector3 position, T target) in targets) {
                    navMeshAgent.CalculatePath(position, path);
                    int length = path.GetCornersNonAlloc(cachedNavmeshCorners);
                    float totalD = 0;

                    if (length == 0) {
                        //target unreachable
                        continue;
                    }
                    for (int i = 1; i < length; i++) {
                        totalD += (cachedNavmeshCorners[i - 1] - cachedNavmeshCorners[i]).magnitude;
                    }

                    if (totalD < minDist) {
                        foundTarget = true;
                        minDist = totalD;
                        minTarget = target;
                    }
                    //Debug.Log($"{target} has a d of {totalD} in l {length}");
                }

                if (!foundTarget) {
                    //Debug.Log("Couldn't find target");
                }
                //Debug.Log($"we picked {target}");
                return minTarget;
            }
        }
    }
}