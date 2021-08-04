using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;
    private Material material;

    private float moveDelay = 0, moveDelayMin = 1f, moveDelayMax = 5f;

    private IEnumerator currentRoutine;
    private float maxScale = 2.5f, scaleDuration = 1f;

    void Start()
    {
        moveDelay = Random.Range(moveDelayMin, moveDelayMax);

        InvokeRepeating("MoveCheck", 1, moveDelay);

        StartCoroutine(StartScale());

        transform.rotation = Quaternion.Euler(GetStartingRotation());

        material = Renderer.material;

        StartCoroutine(LerpColour());
    }

    void Update()
    {
        transform.Rotate(new Vector3 (7f, 25f, 18f) * Time.deltaTime);
    }

    /// <summary>
    /// Check if the cube is currently playing a move routine, if so then stop. Set a new routine and choose a random position, then play routine.
    /// </summary>
    void MoveCheck()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = MoveTo(Random.onUnitSphere * 5);
        StartCoroutine(currentRoutine);
    }

    /// <summary>
    /// Whilst the position of the cube is not at the destination, move towards at a set speed.
    /// </summary>
    IEnumerator MoveTo(Vector3 position)
    {
        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 5f * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Set the start scale and calculate max size. Loop between scaling up and down between tese 2 sizes.
    /// </summary>
    IEnumerator StartScale()
    {
        Vector3 startSize = transform.localScale;
        Vector3 maxSize = startSize * maxScale;

        while (true)
        {
            yield return Scale(startSize, maxSize);
            yield return Scale(maxSize, startSize);
        }
    }

    /// <summary>
    /// Scale the object over time based on 2 parameters passed into the method.
    /// </summary>
    IEnumerator Scale(Vector3 startScale, Vector3 endScale)
    {
        float time = 0;

        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, time);
            yield return null;
        }
    }

    Vector3 GetStartingRotation()
    {
        Vector3 rotation = new Vector3(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359));

        return rotation;
    }

    /// <summary>
    /// Lerp to a randomly generated colour every 5 seconds.
    /// </summary>
    IEnumerator LerpColour()
    {
        while (true)
        {
            Color randomColour = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 1f);
            Color startColour = material.color;

            float time = 0;

            while (time < 5f)
            {
                material.color = Color.Lerp(startColour, randomColour, time / 5f);
                time += Time.deltaTime;

                yield return null;
            }

            material.color = randomColour;
        }
    }
}
