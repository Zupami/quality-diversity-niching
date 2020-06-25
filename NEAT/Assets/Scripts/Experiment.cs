using System.Collections;
using System.Collections.Generic;
using System.Xml;
using SharpNeat.Core;
using SharpNeat.Domains;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Phenomes;
using UnityEngine;

public class Experiment : INeatExperiment
{
    public string Name { get; }
    public string Description { get; }
    public int InputCount { get; }
    public int OutputCount { get; }
    public int DefaultPopulationSize { get; }
    public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters { get; }
    public NeatGenomeParameters NeatGenomeParameters { get; }
    public void Initialize(string name, XmlElement xmlConfig) {
        throw new System.NotImplementedException();
    }

    public List<NeatGenome> LoadPopulation(XmlReader xr) {
        throw new System.NotImplementedException();
    }

    public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList) {
        throw new System.NotImplementedException();
    }

    public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder() {
        throw new System.NotImplementedException();
    }

    public IGenomeFactory<NeatGenome> CreateGenomeFactory() {
        throw new System.NotImplementedException();
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm() {
        throw new System.NotImplementedException();
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize) {
        throw new System.NotImplementedException();
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList) {
        throw new System.NotImplementedException();
    }
}
