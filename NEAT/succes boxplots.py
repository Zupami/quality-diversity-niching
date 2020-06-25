import matplotlib.pyplot as plt
import csv
import os
import numpy as np

totals = []
totals_labels = []
total_numbers = []

root = "C:/Users/Midas/Documents/PersonalRepo/Projects/NEAT/Experiment 3"
filelist = [i for i in os.listdir(root) if i.endswith(".txt")]
for filename in filelist:
    numbers = []
    total = 0
    with open(os.path.join(root, filename), 'r') as file:
        for line in file:
            number = int(line)
            if number == 400:
                continue
            numbers.append(int(line))
            total += 1
    totals.append(total / 8)
    totals_labels.append(os.path.splitext(filename)[0][1:])
    total_numbers.append(numbers)

x = np.arange(len(totals))
x = [2*i for i in x]
#plt.subplot(211)
plt.bar(x, totals, tick_label=totals_labels, width=1,
        color=["blue", "blue", "blue", "blue", "red", "red", "red", "red", "green", "black"])
plt.ylim(top=1)
plt.ylabel("Proportion")
plt.title("Histogram of successful runs")

#x = np.arange(1, len(totals) + 1)
#plt.subplot(212)
#plt.boxplot(total_numbers)
#plt.xticks(x, totals_labels)

plt.show()
