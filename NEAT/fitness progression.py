import matplotlib.pyplot as plt
import csv
import os
import numpy as np

def conv(x):
    return x.replace(',', '.').encode()

root = "C:/Users/Midas/Documents/PersonalRepo/Projects/NEAT/Experiment 1"
dirlist = os.listdir(root)
figure = 1
for expdir in dirlist:
    best = []
    avg = []
    first = True
    filenr = 0
    filelist = os.listdir(os.path.join(root, expdir))
    filelist = [i for i in filelist if i.endswith(".txt")]
    for filename in filelist:
        best.append([])
        avg.append([])
        with open(os.path.join(root, expdir, filename), 'r') as csvfile:
            plots= csv.reader(csvfile, delimiter=';')
            for row in plots:
                if(len(row) < 3):
                    continue
                best[filenr].append(float(row[0].replace(',','.')))
                avg[filenr].append(float(row[1].replace(',','.')))
        filenr += 1

    avgbest = np.mean(best, axis=0)
    avgavg = np.mean(avg, axis=0)
    
    plt.subplot(4, 5, figure)
    plt.axis(ymax=20)
    plt.plot(avgbest, color='blue')
    plt.plot(avgavg, color='orange')
    plt.title(expdir)
    figure += 1
plt.show()

