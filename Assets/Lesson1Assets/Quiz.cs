using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Option
{
    public string Type;
    public string Value;
    public string Color;
}

[Serializable]
public class Question
{
    public string Text;
    public Option[] Options;
    public int Answer;
}

[Serializable]
public class Lessontxt
{
    public string Message;
}


[Serializable]
public class Quiz
{
    public string ID;
    public string ImagePath;
    public Question[] Questions;
    public int LessonID;
    public float CameraLookAtX;
    public float CameraLookAtY;
    public float CameraLookAtZ;
    public float CameraLocationX;
    public float CameraLocationY;
    public float CameraLocationZ;
}

[Serializable]
public class LessonJSON
{
    public float CameraLookAtX;
    public float CameraLookAtY;
    public float CameraLookAtZ;
    public float CameraLocationX;
    public float CameraLocationY;
    public float CameraLocationZ;
    public int LessonID;
   // public Lessontxt[] Lessons;
    public Lessontxt[] EasyLesson;
    public Lessontxt[] MediumLesson;
    public Lessontxt[] HardLesson;
}