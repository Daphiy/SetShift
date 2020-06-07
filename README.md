# A custom behavioural task 

This is an implementation of a behavioural task with a questionnaire at the beginning and feedback questions at the end. 
If you want to asjust this code for your purposes (another behavioural task), feel free to contact me at daphiya@gmail.com

This behavioural task consists of two games : 
1. A slight variation of the Wisconsin Card Sorting Task, where one card has to be matched to one of 4 piles 
2. A similiar task but with two cards, where the goal is to determine if they match or not 

It is very easy to adapt the code to similiar types of tasks, for example by changing the cards to a different type of stimuli. 

# Anonymised data 

As this task includes a questionnaire about depression, the data has to be saved in a completely anonymised manner. 
I do this by generating a random number between 0 and 100000 for each participant that enters the game. This is then their id throughout the game. Every action the participant makes in the game is saved in a separate row on google sheets, in the following format: 
id, phase, trial, response, time

id = randomly generated id 
phase = what task/ questionnaire
trial = current trial number within the phase 
response = user's response 
time = time taken to respond 

# File overview 

I implemented it in Unity, coding in C#.  

The code is in Assets\Scripts\SetShifting

The questionnaire and feedback questions are in Assets\Scripts\SetShifting\Data

The stimuli (different cards and instruction images) are in Assets\Sprites 

The plug-in for saving data remotely is in Assets\RemoteLogger
