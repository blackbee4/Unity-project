using System.Collections.Generic;
using UnityEngine;

namespace Week6.BoxGame
{
    public class BoxGeneticManager : MonoBehaviour
    {
        public int agentCount;
        public GameObject agentPrefab;
        public Transform goal;
        public List<BoxAgent> population;

        public int maxBehaviors;
        public float waitSeconds = 1f;
        public float generationTimeout;

        private float _totalFitness;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            population = new List<BoxAgent>(agentCount);
            for (int i = 0; i < agentCount; i++)
            {
                BoxAgent go = Instantiate(agentPrefab).GetComponent<BoxAgent>();
                go.transform.position = transform.position;
                go.Genes = new int[maxBehaviors];
                for (int j = 0; j < maxBehaviors; j++)
                    go.Genes[j] = Random.Range(0, 4);
                go.Target = goal;
                population.Add(go);
            }
            
            Invoke("RunGeneration", .1f);
        }

        public void RunGeneration()
        {
            generationTimeout = waitSeconds * (maxBehaviors + 1);
            
            for (int i = 0; i < agentCount; i++)
            {
                population[i].WaitSeconds = waitSeconds;
                population[i].StartMove();
            }

            Invoke("Replace", generationTimeout);
        }

        public void Replace()
        {
            population.Sort();
            
            _totalFitness = 0;
            for (int i = 0; i < agentCount; i++)
            {
                _totalFitness += population[i].Fitness;
            }

            List<BoxAgent> newPopulation = new List<BoxAgent>();
            for (int i = 0; i < agentCount; i++)
            {
                BoxAgent a = Selection();
                BoxAgent b = Selection();

                BoxAgent child = Crossover(a, b);
                child.Mutate(0.08f);
                newPopulation.Add(child);
            }

            for (int i = 0; i < agentCount; i++)
            {
                Destroy(population[i].gameObject);
            }
            
            population = newPopulation;
            RunGeneration();
        }

        public BoxAgent Selection()
        {
            for (int i = 0; i < agentCount; i++)
                if (Random.Range(0f, 1f) < population[i].Fitness / _totalFitness)
                    return population[i];

            return population[0];
        }

        public BoxAgent Crossover(BoxAgent a, BoxAgent b)
        {
            BoxAgent child = Instantiate(agentPrefab).GetComponent<BoxAgent>();
            child.Genes = new int[maxBehaviors];
            for (int i = 0; i < maxBehaviors; i++)
            {
                if (Random.Range(0f, 1f) <= 0.5f)
                    child.Genes[i] = a.Genes[i];
                else
                    child.Genes[i] = b.Genes[i];
            }

            return child;
        }
    }
}