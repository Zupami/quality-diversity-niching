using System;
using System.Collections;
using System.Collections.Generic;
using NoveltySearch;
using NoveltySearch.Containers;
using NoveltySearch.Individuals;
using NoveltySearch.Selections;
using NoveltySearch.Variations;
using SharpNeat.Core;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Utility;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour {
    public GameObject Robot;
    public int PopulationSize;
    public int MaxFrames;
    public uint MaxGen;
    public Slider FrameRate;
    public GameObject ContainerDisplayer;
    public GameObject ContainerDisplay;
    public Text Text;
    public bool DisplayContainer;

    private Selection _selection;
    private Container _container;
    private Variation[] _variations;

    private IGenomeFactory<NeatGenome> _genomeFactory;
    private int _frames;
    private uint _gen;
    private int _epoch;
    private int _experiment;
    private Individual[] _population = new Individual[0];
    private List<GameObject> _containerDisplays = new List<GameObject>();
    private EvolutionParameters[] _parameters;
    private double _best;
    private DateTime _prev = DateTime.Now;
    private int _prevCounter;

    void Start() {
        _frames = MaxFrames;
        _gen = MaxGen;
        RNG.Init();

        NeatGenomeParameters neatGenomeParameters = new NeatGenomeParameters {InitialInterconnectionsProportion = 0.5f, FeedforwardOnly = true};
        _genomeFactory = new NeatGenomeFactory(10, 2, neatGenomeParameters);

        _parameters = new[] {
            new EvolutionParameters(new NoveltySelection(5), new NoveltySearch.Containers.Grid(-31, 10, -4, 14, 41, 18), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new NoveltySelection(5), new Archive(1), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new ContainerSelection(), new NoveltySearch.Containers.Grid(-31, 10, -4, 14, 41, 18), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new ContainerSelection(), new Archive(1), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new FitnessSelection(), new DummyContainer(), new Variation[]{new Crossover(), new Mutation()}),
            new EvolutionParameters(new ClearingSelection(10, 6), new DummyContainer(), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new ClearingSelection(20, 8), new DummyContainer(), new Variation[] {new Crossover(), new Mutation()}),
            new EvolutionParameters(new DeterministicCrowdingSelection(), new DummyContainer(), new Variation[]{new Mutation()}),
            new EvolutionParameters(new RestrictedTournamentSelection(8), new DummyContainer(), new Variation[]{new Mutation()}),
            new EvolutionParameters(new DummySelection(), new DummyContainer(), new Variation[]{new Crossover(), new Mutation()}), 
        };

        /*NeatGenome genome = _genomeFactory.CreateGenome(0);
        while (!CyclicNetworkTest.IsNetworkCyclic(genome)) {
            genome = genome.CreateOffspring(0);
        }
        UnityEditor.EditorApplication.isPlaying = false;*/
    }

    void FixedUpdate() {
        Time.fixedDeltaTime = 1.0f / FrameRate.value;
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        foreach (Individual individual in _population) {
            if (((RobotIndividual)individual).Robot.GoalReached) {
                DataCollection.WriteData(new []{_gen.ToString()});
                _frames = 0;
                _gen = 0;
                Initialize();
                _epoch++;
                return;
            }
        }
        
        if (_frames == MaxFrames) {
            _frames = 0;
            if (_gen == MaxGen) {
                DataCollection.WriteData(new[] { _gen.ToString() });
                _gen = 0;
                Initialize();
                _epoch++;
            }
            else {
                _gen++;
                if (!DisplayContainer) {
                    ShowPopulation();
                }
                NextGeneration();
                if (DisplayContainer) {
                    ShowContainer();
                }
            }
        }
        else {
            _frames++;
        }

        if (_prevCounter < 5) {
            _prevCounter++;
        }
        else {
            DateTime now = DateTime.Now;
            Text.text = (int)(6 / (now - _prev).TotalSeconds) + "\n" + _experiment + "\n" + _epoch + "\n" + _gen;
            _prev = now;
            _prevCounter = 0;
        }
    }

    void Initialize() {
        if (_population.Length > 0) {
            for (int i = 0; i < PopulationSize; i++) {
                Destroy(((RobotIndividual)_population[i]).Robot.gameObject);
            }
        }

        _population = new Individual[PopulationSize];
        List<NeatGenome> genomeList = _genomeFactory.CreateGenomeList(PopulationSize, 0);
        for (int i = 0; i < PopulationSize; i++) {
            Robot robot = Instantiate(Robot).GetComponent<Robot>();
            robot.Initialize(genomeList[i]);
            RobotIndividual individual = new RobotIndividual(robot, null);
            _population[i] = individual;
        }

        if (_epoch >= _parameters.Length) {
            _experiment++;
            _epoch = 0;
        }
        EvolutionParameters parameters = _parameters[_epoch];
        _selection = parameters.Selection;
        _container = parameters.Container;
        _container.Clear();
        _variations = parameters.Variations;

        DataCollection.NewExperiment(parameters);
        _best = 0;
    }

    void NextGeneration() {
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < PopulationSize; i++) {
           toDestroy.Add(((RobotIndividual)_population[i]).Robot.gameObject);
        }

        Individual best = _population[0];
        double totalFitness = best.Fitness;
        for (int i = 1; i < PopulationSize; i++) {
            RobotIndividual individual = (RobotIndividual)_population[i];
            totalFitness += individual.Fitness;
            if (individual.Fitness > best.Fitness) {
                best = individual;
            }
            _container.Add(individual);
        }
        if (best.Fitness > _best) {
            _best = best.Fitness;
        }
        //DataCollection.WriteData(new [] {_best.ToString(), (totalFitness / PopulationSize).ToString(), _container.GetAmount().ToString()});
        
        _population = _selection.Select(_population, _container);
        foreach (Variation variation in _variations) {
            _population = variation.Vary(_population, _gen);
        }
        
        for (int i = 0; i < PopulationSize; i++) {
            Destroy(toDestroy[i]);

            RobotIndividual individual = (RobotIndividual)_population[i];
            Robot robot = Instantiate(Robot).GetComponent<Robot>();
            robot.Initialize(individual.Genome);
            _population[i] = new RobotIndividual(robot, individual.Parents);
        }
    }

    void ShowContainer() {
        foreach (GameObject containerDisplay in _containerDisplays) {
            Destroy(containerDisplay);
        }

        foreach (Individual individual in _container.GetIndividuals()) {
            double[] behaviour = individual.Behaviour;
            _containerDisplays.Add(Instantiate(ContainerDisplay, new Vector2((float)behaviour[0], (float)behaviour[1]), Quaternion.identity, ContainerDisplayer.transform));
        }
    }

    void ShowPopulation() {
        foreach (GameObject containerDisplay in _containerDisplays) {
            Destroy(containerDisplay);
        }

        foreach (Individual individual in _population) {
            double[] behaviour = individual.Behaviour;
            _containerDisplays.Add(Instantiate(ContainerDisplay, new Vector2((float)behaviour[0], (float)behaviour[1]), Quaternion.identity, ContainerDisplayer.transform));
        }
    }
}
