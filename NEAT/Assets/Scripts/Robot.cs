using System;
using System.Collections;
using System.Collections.Generic;
using NoveltySearch.Individuals;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.Neat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Phenomes;
using UnityEngine;

public class Robot : MonoBehaviour {
    public float Speed;
    public float RotationSpeed;
    public NeatGenome Genome;
    public bool ManualInput;

    [HideInInspector] public bool GoalReached = false;

    private GameObject _goal;
    private IBlackBox _blackBox;

    void Start() {
        _goal = GameObject.Find("Goal");
    }
    
    void FixedUpdate() {
        _blackBox.ResetState();
        float[] rangeFinders = GetRangeFinders();
        for (int i = 0; i < 6; i++) {
            _blackBox.InputSignalArray[i] = rangeFinders[i];
        }

        float[] slices = GetSlices();
        for (int i = 0; i < 4; i++) {
            _blackBox.InputSignalArray[i + 6] = slices[i];
        }
        _blackBox.Activate();

        float speedOutput = (float)(_blackBox.OutputSignalArray[0] / 2);
        float rotationOutput = (float)(_blackBox.OutputSignalArray[1] - 0.5);

        if (ManualInput) {
            speedOutput = 0;
            rotationOutput = 0;
            if (Input.GetKey(KeyCode.W)) {
                speedOutput += 0.5f;
            }

            if (Input.GetKey(KeyCode.S)) {
                speedOutput -= 0.5f;
            }

            if (Input.GetKey(KeyCode.A)) {
                rotationOutput += 0.5f;
            }

            if (Input.GetKey(KeyCode.D)) {
                rotationOutput -= 0.5f;
            }
        }

        Rigidbody2D body = GetComponent<Rigidbody2D>();
        float rotation = body.rotation * Mathf.Deg2Rad;

        Vector2 direction = new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation)) * Speed * speedOutput;
        float rotationSpeed = RotationSpeed * rotationOutput;

        body.position += direction;
        body.rotation += rotationSpeed;
    }

    float[] GetRangeFinders() {
        float[] rangeFinders = new float[6];
        Rigidbody2D body = GetComponent<Rigidbody2D>();

        rangeFinders[0] = GetRangeFinderDistance(body.rotation * Mathf.Deg2Rad);
        rangeFinders[1] = GetRangeFinderDistance((body.rotation + 45) * Mathf.Deg2Rad);
        rangeFinders[2] = GetRangeFinderDistance((body.rotation + 90) * Mathf.Deg2Rad);
        rangeFinders[3] = GetRangeFinderDistance((body.rotation - 45) * Mathf.Deg2Rad);
        rangeFinders[4] = GetRangeFinderDistance((body.rotation - 90) * Mathf.Deg2Rad);
        rangeFinders[5] = GetRangeFinderDistance((body.rotation + 180) * Mathf.Deg2Rad);

        return rangeFinders;
    }
    
    float GetRangeFinderDistance(float rotation) {
        Vector2 direction = new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + direction * transform.localScale, direction);
        return hit.distance / 20;
    }

    float[] GetSlices() {
        float[] slices = new float[4];
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        float angle = Vector2.Angle(Vector2.up, _goal.transform.position - transform.position);
        if (transform.position.x > _goal.transform.position.x) {
            angle = 360 - angle;
        }
        angle += body.rotation + 45;
        slices[(int)((angle % 360 + 360) % 360 / 90)] = 1;
        return slices;
    }

    public void Initialize(NeatGenome genome) {
        IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = new NeatGenomeDecoder(NetworkActivationScheme.CreateAcyclicScheme());
        try {
            _blackBox = genomeDecoder.Decode(genome);
        }
        catch (Exception) {
            if (CyclicNetworkTest.IsNetworkCyclic(genome)) {
                Debug.Log("Cyclic");
            }
        }
        Genome = genome;
    }

    public double GetGoalDistance() {
        return (transform.position - _goal.transform.position).magnitude;
    }
}
