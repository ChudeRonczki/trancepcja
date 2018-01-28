using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnInterval = 4f;
    public float[] probabilites = { 1f, .3f, .1f };
    public Tran[] prefabs = new Tran[3];
    public ParticleSystem[] PreSpawnParticles = new ParticleSystem[3];
    public ParticleSystem AfterSpawnParticles;

    SpawnPoint[] spawnPoints;
    SpawnPoint lastUsed;

	void Start ()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        StartCoroutine(SpawnCycle());
	}

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            var emptySpawns = spawnPoints.Where(spawn => spawn.Empty).ToArray();
            if (emptySpawns.Length > 0)
            {
                var spawnToUse = emptySpawns[Random.Range(0, emptySpawns.Length)];
                if (spawnToUse == lastUsed)
                    spawnToUse = emptySpawns[Random.Range(0, emptySpawns.Length)];

                lastUsed = spawnToUse;
                float roll = Random.Range(0f, probabilites.Sum());

                float acc = 0f;
                Tran chosenPrefab = prefabs[0];
                ParticleSystem particles = PreSpawnParticles[0];

                for (int i = 0; i < prefabs.Length; ++i)
                {
                    acc += probabilites[i];
                    if (roll <= acc)
                    {
                        chosenPrefab = prefabs[i];
                        particles = PreSpawnParticles[i];
                        break;
                    }
                }
                
                if (particles) Instantiate(particles, spawnToUse.transform.position, Quaternion.identity);

                yield return new WaitForSeconds(3.0f);

                Instantiate(chosenPrefab, spawnToUse.transform.position, Quaternion.identity);
                if (AfterSpawnParticles) Instantiate(AfterSpawnParticles, spawnToUse.transform.position, Quaternion.identity);
            }
        }
    }
}
