using System;
using System.Collections.Generic;
using System.IO;
using NoveltySearch.Variations;

public static class DataCollection {
    private static string _currentExperiment;
    private static DateTime _start;
    private static List<string> _experiments = new List<string>();

    public static void NewExperiment(EvolutionParameters parameters) {
        DateTime now = DateTime.Now;
        if (_currentExperiment != null) {
            //WriteData(new []{(now - _start).TotalMilliseconds.ToString()});
        }
        string experimentString = parameters.Selection + "-" +
                                  parameters.Container + "-[" +
                                  string.Join<Variation>("-", parameters.Variations) + "]";

        foreach (string experiment in _experiments) {
            if (experiment.Contains(experimentString)) {
                _currentExperiment = experiment;
                _start = now;
                return;
            }
        }

        _currentExperiment = now.ToString("yyyyMMddHHmmss") + "-" + experimentString + ".txt";
        _experiments.Add(_currentExperiment);
        _start = now;
    }

    public static void WriteData(string[] data) {
        if (_currentExperiment == null) {
            return;
        }
        using (StreamWriter writer = new StreamWriter(_currentExperiment, true)) {
            writer.WriteLine(string.Join(";", data));
        }
    }
}
