using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//lots of credit to this https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity
public class Conductor : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI ratingUI;
    public Animator squawkIndicator;

    public AudioClip perfectSound;
    public AudioClip goodSound;
    public AudioClip badSound;
    public AudioSource sfx;
    public AudioSource music;

    public Bopper left;
    public Bopper center;
    public Bopper right;

    public float secPerBeat;
    public float startOffset;
    public float currentSongSecond;
    public float currentBeat;
    public float dspSongTime;
    public enum BeatType { NONE, SQUAWK, CIN, COUT, LIN, LOUT, RIN, ROUT}
    public List<BeatType> events;
    public int currentEvent = 0;
    public float goodThreshold = .5f;
    public float perfectThreshold = 0.2f;

    bool indicated = false;

    float score = 0f;

    private void Start()
    {
        Movement.onSquawk += Squawked;
        left.bop += Bopped;
        center.bop += Bopped;
        right.bop += Bopped;
    }

    public void PlaySong(Song song)
    {
        events = song.events;
        music.clip = song.song;
        secPerBeat = 60f / song.bpm;
        dspSongTime = (float)AudioSettings.dspTime;
        startOffset = song.startOffset;
        music.Play();
    }

    void Update()
    {
        if (music.isPlaying)
        {
            currentSongSecond = (float)(AudioSettings.dspTime - dspSongTime - startOffset);
            currentBeat = currentSongSecond / secPerBeat;
            if(currentEvent - currentBeat < 1f && currentEvent - currentBeat > 0.002 && !indicated)//beat coming
            {
                Debug.Log("Incoming");
                EventComing();
                indicated = true;
                StartCoroutine("CheckInput", currentEvent);
            }

            if (currentEvent - currentBeat < 0.002) //if beat is on
            {
                Debug.Log("Beat");
                StopAllCoroutines();
                if(currentEvent + 1 < events.Count - 1)
                {
                    currentEvent++;
                    indicated = false;
                }
                else
                {
                    EndSong();
                }
            }
        }
       
    }

    void EndSong()
    {
        music.Stop();
    }

    void EventComing()
    {
        if (events[currentEvent+1]!= BeatType.NONE)
        {
            switch (events[currentEvent+1])
            {
                case BeatType.NONE:
                    break;
                case BeatType.SQUAWK:
                    squawkIndicator.Play("SquawkComing");
                    break;
                case BeatType.COUT:
                    center.Coming(0);
                    break;
                case BeatType.CIN:
                    center.Coming(1);
                    break;
                case BeatType.LOUT:
                    left.Coming(0);
                    break;
                case BeatType.LIN:
                    left.Coming(1);
                    break;
                case BeatType.ROUT:
                    right.Coming(0);
                    break;
                case BeatType.RIN:
                    right.Coming(1);
                    break;
            }
        }
    }
    void CheckEvent(BeatType type)
    {
        if (music.isPlaying)
        {
            if (events[currentEvent] == type) //if we got the input right
            {
                if (currentEvent - currentBeat < goodThreshold && currentEvent - currentBeat > perfectThreshold) //the current beat (when we squawked) is within the threshold
                {
                    Score(1);
                }
                else if (currentEvent - currentBeat < perfectThreshold)
                {
                    Score(2);
                }
                else if (currentEvent - currentBeat > goodThreshold)
                {
                    Score(0);
                }
            }
        }
    }

    IEnumerator CheckInput(int eventBeat)
    {
        while (eventBeat - currentBeat < goodThreshold) //while in the beat threshold
        {
            if (currentEvent == eventBeat)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                break;
            }
        }
        Score(0);
        yield return null;
    }

    void Score(int rating)
    {
        AudioClip clip = badSound;
        switch (rating)
        {
            case 0:
                ratingUI.text = "Bad";
                ratingUI.color = Color.red;
                clip = badSound;
                break;
            case 1:
                ratingUI.text = "Good";
                ratingUI.color = Color.yellow;
                score += 100;
                clip = goodSound;
                break;
            case 2:
                ratingUI.text = "Perfect";
                ratingUI.color = Color.green;
                score += 300;
                clip = perfectSound;
                break;
        }
        sfx.PlayOneShot(clip);
        scoreUI.text = "Score: " + score;
    }

    void Bopped(int dir, int bopper) //in out, which bopper
    {
        //Debug.Log("Bopped in direction " + dir + " on " + bopper);
        BeatType eventType = BeatType.NONE;
        if(dir == 0)
        {
            if(bopper == 0)
            {
                eventType = BeatType.LOUT;
            }
            else if(bopper == 1)
            {
                eventType = BeatType.COUT;
            }
            else if(bopper == 2)
            {
                eventType = BeatType.ROUT;
            }
        }
        else
        {
            if (bopper == 0)
            {
                eventType = BeatType.LIN;
            }
            else if (bopper == 1)
            {
                eventType = BeatType.CIN;
            }
            else if (bopper == 2)
            {
                eventType = BeatType.RIN;
            }
        }
        AudioClip clip;
        CheckEvent(eventType);
    }

    void Squawked()
    {
        CheckEvent(BeatType.SQUAWK);
    }

}
