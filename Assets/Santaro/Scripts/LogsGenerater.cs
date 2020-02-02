using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogsGenerater : MonoBehaviour
{
    [SerializeField] private GameObject logObject = null;
    [SerializeField] private int firstGenerateNum = 5;
    [SerializeField] private float generateInterval = 5f;
    [SerializeField] private int generateNum = 3;
    private float countTime = 0f;
    private float screenWidth = 16f;
    private float screenHeight = 4f;

    private void Start()
    {
        Generate(this.firstGenerateNum);
    }
    private void Update()
    {
        this.countTime += Time.deltaTime;
        if(this.generateInterval > 0 && this.countTime >= this.generateInterval)
        {
            Generate(this.generateNum);
            this.countTime = 0f;
        }
    }

    private void Generate(int generateNum)
    {
        if (generateNum < 1) return;
        float widthInterval = this.screenWidth / generateNum; //1区分のxの大きさ
        for(int i = 0; i < generateNum; i++)
        {
            Vector2 position = new Vector2(UnityEngine.Random.Range(widthInterval * i, widthInterval * (i + 1)) - screenWidth/2, UnityEngine.Random.Range(0, screenHeight) - screenHeight/2 - 2f);
            Instantiate(logObject, position, Quaternion.identity);
        }
    }
}
