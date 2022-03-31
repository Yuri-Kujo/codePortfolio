using Cinemachine;
using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCheck : MonoBehaviour
{
    private AudioSource bgmMan;
    [Header("Datos nº de vueltas y checkpoints alcanzados")]
    public int lapCounter;
    public bool canCompleteLap;
    private LapTimeManager raceTimer;
    private PositionManager posMan;
    private CheckpointList cpList;
    public int lastCheckpointHit;
    public int positionScore;
    public int kartPosition;
    [Header("Parametros cambiados para el final de la carrera")]
    public Transform racerTransform;
    public Vector3 newEndPosition;
    private GameObject endGameUI;
    [Header("Ayuda pal bot cuando se va en reversa como gil")]
    public bool isABot;
    public int highestCheckPointHit;
    public float secondsToTeleport;
    private float secondsWithoutHittingCheckpoint;
    private ArcadeKart arcadeKart;

    void Start()
    {
        bgmMan = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        cpList = GameObject.Find("GameManager").GetComponent<CheckpointList>();
        posMan = GameObject.Find("GameManager").GetComponent<PositionManager>();
        raceTimer = GameObject.Find("GameManager").GetComponent<LapTimeManager>();
        endGameUI = GameObject.Find("EndUI");
        arcadeKart = GetComponent<ArcadeKart>();
    }
    void Update()
    {
        positionScore = (lapCounter * 1000) + lastCheckpointHit;
        if(!isABot)
        {
            FinalLap();
        }

        if(isABot && arcadeKart.m_CanMove)
        {
            secondsWithoutHittingCheckpoint += Time.deltaTime;
            if(arcadeKart.m_CanMove && secondsWithoutHittingCheckpoint >= secondsToTeleport)
            {
                TeleportBot();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Checkpoint"))
        {
            canCompleteLap = true;
        }

        if(other.CompareTag("AntiReverse"))
        {
            canCompleteLap = false;
        }

        if(other.CompareTag("FinishLine") && canCompleteLap)
        {
            if(lapCounter == cpList.maxLaps)
            {
                arcadeKart.SetCanMove(false);
                posMan.ordenLlegada.Add(gameObject);
                Debug.Log(gameObject.name + " llegó en puesto: " + posMan.ordenLlegada.Count);
                StartCoroutine("RaceEnd");
            }
            canCompleteLap = false;
            highestCheckPointHit = 0;
            lapCounter++;
        }

        if(cpList.checkpointCol.Contains(other.gameObject))
        {
            lastCheckpointHit = cpList.checkpointCol.IndexOf(other.gameObject);
            if(arcadeKart.m_CanMove && lastCheckpointHit > highestCheckPointHit)
            {
                secondsWithoutHittingCheckpoint = 0;
                highestCheckPointHit = lastCheckpointHit;
            }
        }
    }

    private void FinalLap()
    {
        if(lapCounter == cpList.maxLaps)
        {
            bgmMan.pitch = 1.2f;
        }

        else
        {
            bgmMan.pitch = 1f;
        }
    }

    void TeleportBot()
    {
        secondsWithoutHittingCheckpoint = 0;
        if(gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        if(highestCheckPointHit + 2 > cpList.checkpointCol.Count)
        {
            transform.position = cpList.checkpointCol[cpList.checkpointCol.Count].transform.position;
            transform.rotation = cpList.checkpointCol[cpList.checkpointCol.Count].transform.rotation;
        }
        else
        {
            transform.position = cpList.checkpointCol[highestCheckPointHit + 1].transform.position;
            transform.rotation = cpList.checkpointCol[highestCheckPointHit + 1].transform.rotation;
        }
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 16, ForceMode.VelocityChange);
        //Debug.Log(gameObject.name + " teletransportado a Checkpoint " + (highestCheckPointHit + 2));
    }

    IEnumerator RaceEnd()
    {
        if(!isABot)
        {
            raceTimer.timeRunning = false;
            GetComponent<AntiKinematic>().enabled = false;
            bgmMan.Stop();
            CinemachineVirtualCamera winCamera = GameObject.Find("WinCamera").GetComponent<CinemachineVirtualCamera>();
            winCamera.LookAt = gameObject.transform;
            winCamera.Follow = gameObject.transform;
            winCamera.Priority = 50;
            GameObject.Find("PositionUI").SetActive(false);
            GameObject.Find("LapUI").SetActive(false);
        }
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        EndAnimations();
        yield break;
    }

    IEnumerator Stun()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Animator anim = GetComponent<KartPlayerAnimator>().PlayerAnimator;
        anim.SetBool("Hit", true);
        rb.isKinematic = true;
        yield return new WaitForSeconds(1.5f);
        rb.isKinematic = false;
        anim.SetBool("Hit", false);
        yield break;
    }

    void EndAnimations()
    {
        racerTransform.localPosition = newEndPosition;
        racerTransform.localRotation = Quaternion.Euler(0,0,0);
        int racePos;

        for(racePos = 0; racePos < posMan.ordenLlegada.Count; racePos++ )
        {
            if (gameObject == posMan.ordenLlegada[racePos])
            {
                break;
            }
        }
        switch(racePos)
        {
            case 0:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("First", true);
                if(!isABot)
                {
                    endGameUI.transform.GetChild(0).gameObject.SetActive(true);
                }
                break;
            case 1:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("Second", true);
                if (!isABot)
                {
                    endGameUI.transform.GetChild(1).gameObject.SetActive(true);
                }
                break;
            case 2:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("Second", true);
                if (!isABot)
                {
                    endGameUI.transform.GetChild(2).gameObject.SetActive(true);
                }
                break;
            case 3:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("Fourth", true);
                if (!isABot)
                {
                    endGameUI.transform.GetChild(3).gameObject.SetActive(true);
                }
                break;
            case 4:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("Fourth", true);
                if (!isABot)
                {
                    endGameUI.transform.GetChild(3).gameObject.SetActive(true);
                }
                break;
            case 5:
                gameObject.GetComponent<KartPlayerAnimator>().PlayerAnimator.SetBool("Fourth", true);
                if (!isABot)
                {
                    endGameUI.transform.GetChild(3).gameObject.SetActive(true);
                }
                break;
        }
    }
}
