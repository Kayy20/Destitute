using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class EncounterSO : ScriptableObject
{
    public string encounterName;
    public string encounterProblem;
    public AnswerType.WhereItHappens where;
    public int numAnswers;
    public string[] encounterAnswers;
    public int[] numRequirements;
    public int[] numRewards;
    public List<List<AnswerType>> answerReq;
    public List<List<AnswerType>> answerRew;

    public List<AnswerTypeSO> answerReqOut;
    public List<AnswerTypeSO> answerRewOut;

    public AudioSoundSO sound;

    public Sprite backgroundImage;

    // For when ulitilizing for saving
    public void OrganizeOutgoingLists()
    {

        AnswerTypeSO.CheckLatestNum();

        // Requirements
        answerReqOut = new List<AnswerTypeSO>();
        answerRewOut = new List<AnswerTypeSO>();
        for (int i = 0; i < numAnswers; i++)
        {
            for (int j = 0; j < numRequirements[i]; j++)
            {

                

                if (answerReq[i][j].Type != AnswerType.AnsType.Other)
                {
                    answerReqOut.Add(new AnswerTypeSO(answerReq[i][j]));
                }
                else if (answerReq[i][j].Other != AnswerType.OtherReward.Nothing)
                {
                    answerReqOut.Add(new AnswerTypeSO(answerReq[i][j]));
                }
                else
                {
                    answerReqOut.Add(null);
                }
            }
        // Rewards;
        
            for (int j = 0; j < numRewards[i]; j++)
            {
                if (answerRew[i][j].Type != AnswerType.AnsType.Other)
                {
                    answerRewOut.Add(new AnswerTypeSO(answerRew[i][j]));
                }
                else if (answerRew[i][j].Other != AnswerType.OtherReward.Nothing)
                {
                    answerRewOut.Add(new AnswerTypeSO(answerRew[i][j]));
                }
                else
                {
                    answerRewOut.Add(null);
                }
            }
        }
    }
    
    // For when utilizing in game
    public void OrganizeIncomingLists()
    {
        answerReq = new List<List<AnswerType>>();
        answerRew = new List<List<AnswerType>>();
        int k = 0;
        for (int i = 0; i < numAnswers; i++)
        {
            List<AnswerType> reqList = new List<AnswerType>();
            for (int j = 0; j < numRequirements[i]; j++)
            {

                if (answerReqOut[k] == null)
                {
                    reqList.Add(new AnswerType());
                }
                else
                {
                    reqList.Add(new AnswerType(answerReqOut[k]));
                }
                k++;
            }
            answerReq.Add(reqList);
        }
        k = 0;
        for (int i = 0; i < numAnswers; i++)
        {
            List<AnswerType> rewList = new List<AnswerType>();
            
            for (int j = 0; j < numRewards[i]; j++)
            {
                if (answerRewOut[k] == null)
                {
                    rewList.Add(new AnswerType());
                }
                else
                {
                    rewList.Add(new AnswerType(answerRewOut[k]));
                }
                k++;
            }

            answerRew.Add(rewList);
        }
        
    }

}
